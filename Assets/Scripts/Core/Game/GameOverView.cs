using Core.StateMachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Game
{
    public class GameOverView : MonoBehaviour
    {
        [SerializeField] private Button restartButton;
        [SerializeField] private Button homeButton;
        [SerializeField] private TextMeshProUGUI scoreText;

        private ApplicationStateMachine _stateMachine;

        public void Initialize(ApplicationStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            AddListeners();
        }
        
        public void SetScoreText(int value) => scoreText.SetText($"Score: {value}");
        private void RestartButtonClicked() => _stateMachine.Enter<GameApplicationState>().Forget();
        private void HomeButtonClicked() => _stateMachine.Enter<LobbyApplicationState>().Forget();

        private void AddListeners()
        {
            restartButton.onClick.AddListener(RestartButtonClicked);
            homeButton.onClick.AddListener(HomeButtonClicked);
        }

        private void OnEnable()
        {
            Cursor.lockState = CursorLockMode.Confined; 
        }
    }
}