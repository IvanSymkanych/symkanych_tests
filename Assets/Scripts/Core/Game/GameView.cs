using Core.StateMachine;
using Modules.Tools;
using Modules.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.Game
{
    public class GameView : MonoBehaviour
    {
        public KillProgressView KillProgressView => killProgressView;
        public ProgressBarView HealthBarView => healthBar;
        public ProgressBarView EnergyBarView => energyBar;

        [SerializeField] private GameObject gameView;
        [SerializeField] private GameObject pauseView;
        [SerializeField] private GameOverView gameOverView;
        [SerializeField] private GameObject mobileInputViewContainer;
        [SerializeField] private KillProgressView killProgressView;
        [SerializeField] private ProgressBarView healthBar;
        [SerializeField] private ProgressBarView energyBar;
        [SerializeField] private Button pauseButton;
        [SerializeField] private Button unpauseButton;
        
        private ApplicationStateMachine _applicationStateMachine;
        private IRewardService _rewardService;

        [Inject]
        public void Construct(
            ApplicationStateMachine stateMachine,
            IRewardService rewardService)
        {
            _applicationStateMachine = stateMachine;
            _rewardService = rewardService;
        }

        public void Initialize()
        {
            gameOverView.Initialize(_applicationStateMachine);
            healthBar.Initialize();
            energyBar.Initialize();
            pauseButton.onClick.AddListener(Pause);
            unpauseButton.onClick.AddListener(Unpause);
            ChangeMobileViewState();
        }

        public void GameOver()
        {
            gameView.SetActive(false);
            gameOverView.gameObject.SetActive(true);
            gameOverView.SetScoreText(_rewardService.Score);
            _rewardService.SaveScore();
        }

        private void Pause()
        {
            pauseButton.gameObject.SetActive(false);
            pauseView.SetActive(true);
            Time.timeScale = 0;
        }

        private void Unpause()
        {
            pauseButton.gameObject.SetActive(true);
            pauseView.SetActive(false);
            Time.timeScale = 1;
        }

        private void ChangeMobileViewState()
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            mobileInputViewContainer.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
#else 
           mobileInputViewContainer.SetActive(true);
#endif
        }
    }
}