using Core.Service;
using Helpers;
using Zenject;

namespace Core.Lobby
{
    public class HighScoreController
    {
        public HighScoreView HighScoreView { get; private set; }

        
        private IPlayerPrefsService _playerPrefsService;

        [Inject]
        public void Construct(IPlayerPrefsService playerPrefsService) => _playerPrefsService = playerPrefsService;

        public HighScoreController(HighScoreView highScoreView) => HighScoreView = highScoreView;

        public void Initialize() => SetScore();
  
        private void SetScore()
        {
            var score = _playerPrefsService.GetInt(PlayerPrefsKeyHelper.HighScorePrefsKey);
            HighScoreView.SetScore(score.ToString());
        }
    }
}