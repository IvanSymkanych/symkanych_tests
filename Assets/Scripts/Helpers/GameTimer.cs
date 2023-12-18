using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Extensions
{
    public class GameTimer : IDisposable
    {
        public event Action<double> OnElapsed;
        public event Action OnTimeIsOver;

        public bool TimerStarted { get; private set; }
        private double TimeLeft { get; set; }
        private bool _isOnPause;
        private bool _hasBeenPaused;
        private int _interval;

        private CancellationTokenSource _cancellationTokenSource;
        private CancellationToken _token = CancellationToken.None;

        private bool Ready => TimeLeft <= 0;

        public void Dispose()
        {
            if (!TimerStarted)
                return;

            TimerStarted = false;
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }

        public void StartTimer(int time, int interval = 1000)
        {
            _interval = interval;
            TimeLeft = time;
            _cancellationTokenSource = new CancellationTokenSource();
            _token = _cancellationTokenSource.Token;
            UpdateTimer().GetAwaiter();
            TimerStarted = true;
            OnElapsed?.Invoke(TimeLeft);
        }

        public void PauseTimer()
        {
            _isOnPause = true;
            _hasBeenPaused = true;
        }

        public void ResumeTimer() => _isOnPause = false;

        public void StopTimer()
        {
            Dispose();
            OnTimeIsOver?.Invoke();
        }

        public int GetTimeLeft() => (int)TimeLeft;

        private async Task UpdateTimer()
        {
            while (!Ready)
            {
                if (_isOnPause)
                {
                    _hasBeenPaused = true;

                    if (!await DelayInterval())
                        return;

                    continue;
                }

                if (_token.IsCancellationRequested)
                    return;

                var timeFromLastTick = Time.realtimeSinceStartup;

                if (!await DelayInterval())
                    return;

                if (_hasBeenPaused)
                {
                    _hasBeenPaused = false;
                    continue;
                }

                Tick(timeFromLastTick);

                OnElapsed?.Invoke(Mathf.Round((float)TimeLeft));
            }

            StopTimer();
        }

        private async Task<bool> DelayInterval()
        {
            try
            {
                await Task.Delay(_interval, _token);
            }
            catch (TaskCanceledException)
            {
                return false;
            }

            return true;
        }

        private void Tick(float timeFromLastTick)
        {
            if (Ready)
                return;

            var deltaTime = Time.realtimeSinceStartup - timeFromLastTick;
            TimeLeft -= deltaTime;

            if (TimeLeft < 0)
                TimeLeft = 0;
        }
    }
}