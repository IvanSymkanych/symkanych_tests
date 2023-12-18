using UnityEngine;
using UnityEngine.UI;

namespace Core.Lobby
{
    [RequireComponent(typeof(Canvas))]
    public class LobbyView : MonoBehaviour
    {
        public HighScoreView HighScoreView => highScoreView;
        public LoadGameButtonView LoadGameButtonView => loadGameButtonView;
        
        [SerializeField] private HighScoreView highScoreView;
        [SerializeField] private LoadGameButtonView loadGameButtonView;
        [SerializeField] private Image backgroundImage;
        
        private Canvas _canvas;

        public void Initialize()
        {
            _canvas = GetComponent<Canvas>();
        }
    }
}