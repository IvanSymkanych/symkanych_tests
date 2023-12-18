using System;
using Helpers;
using UnityEngine;

namespace Modules.Tools
{
    public class ProgressBarController : IProgressBarController
    {
        public event Action OnValueMinimum;
        public bool IsMaxValue => _value >= MaxValue;
        public int MaxValue { get; private set; }

        private const int MinValue = 0;

        private readonly ProgressBarView _progressBarView;

        private int _value;

        public ProgressBarController(ProgressBarView progressBarView, int maxValue, int startValue)
        {
            _progressBarView = progressBarView;
            MaxValue = maxValue;
            _value = startValue;

            SetValue(_value);
            _progressBarView.SetProgressInstantly(GetPercentValue());
        }

        public void SetValueAnimated(int value)
        {
            SetValue(value);
            _progressBarView.SetProgressAnimated(GetPercentValue());
        }

        public void AddValueAnimated(int value)
        {
            SetValue(_value + value);
            _progressBarView.SetProgressAnimated(GetPercentValue());
        }

        public float GetPercentValue() =>
            GameHelper.GetPercent(_value, MaxValue);

        private void SetValue(int value)
        {
            _value = Mathf.Clamp(value, MinValue, MaxValue);

            if (_value == MinValue)
                OnValueMinimum?.Invoke();
        }
    }
}