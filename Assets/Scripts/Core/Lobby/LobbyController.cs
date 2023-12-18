using UnityEngine;

namespace Core.Lobby
{
    public class LobbyController
    {
        public LobbyView LobbyView { get; private set; }
        public LoadGameButtonController LoadGameButtonController { get; private set; }
        public HighScoreController HighScoreController { get; private set; }
        
        public LobbyController(
            LobbyView lobbyView,
            LoadGameButtonController loadGameButtonController,
            HighScoreController highScoreController)
        {
            LobbyView = lobbyView;
            LoadGameButtonController = loadGameButtonController;
            HighScoreController = highScoreController;
        }

        public void Initialize()
        {
            LobbyView.Initialize();
            LoadGameButtonController.Initialize();
            HighScoreController.Initialize();
            Cursor.lockState = CursorLockMode.Confined;
        }

        public void CleanUp()
        {
            LoadGameButtonController.CleanUp();
        }
    }
}