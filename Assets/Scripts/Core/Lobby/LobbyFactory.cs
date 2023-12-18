using Core.Service;
using Cysharp.Threading.Tasks;
using Helpers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Lobby
{
    public class LobbyFactory : ILobbyFactory
    {
        public LobbyController LobbyController { get; private set; }

        private readonly IInjectService _injectService;
        private readonly IAddressableService _addressableService;
        private readonly ILoadingScreenService _loadingScreenService;
        private readonly ISceneLoadService _sceneLoadService;

        public LobbyFactory(
            IInjectService injectService,
            IAddressableService assetService,
            ILoadingScreenService loadingScreenService,
            ISceneLoadService sceneLoadService)
        {
            _injectService = injectService;
            _addressableService = assetService;
            _loadingScreenService = loadingScreenService;
            _sceneLoadService = sceneLoadService;
        }

        public async UniTask Initialize()
        {
            await CreateLobbyController();
            _loadingScreenService.Hide();
        }

        private async UniTask CreateLobbyController()
        {
            var lobbyView = await InstantiateLobbyView();
            var playButtonController = CreatePlayButtonController(lobbyView.LoadGameButtonView);
            var highScoreController = CreateHighScoreController(lobbyView.HighScoreView);

            LobbyController = new LobbyController(
                lobbyView,
                playButtonController,
                highScoreController);

            _injectService.Inject(LobbyController);
            LobbyController.Initialize();
        }

        private async UniTask<LobbyView> InstantiateLobbyView()
        {
            var lobbyViewPrefab = await _addressableService.LoadAssetAsync<GameObject>(AddressableKeysHelper.LobbyViewKey);
            var instance = Object.Instantiate(lobbyViewPrefab);
            var view = instance.GetComponent<LobbyView>();
            return view;
        }

        private LoadGameButtonController CreatePlayButtonController(LoadGameButtonView view)
        {
            var instance = new LoadGameButtonController(view);
            _injectService.Inject(instance);
            return instance;
        }

        private HighScoreController CreateHighScoreController(HighScoreView view)
        {
            var instance = new HighScoreController(view);
            _injectService.Inject(instance);
            return instance;
        }
    }
}