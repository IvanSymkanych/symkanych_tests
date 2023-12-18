using System.Linq;
using Core.Game.Pools;
using Core.Service;
using Cysharp.Threading.Tasks;
using Helpers;
using Modules.Enemy;
using Modules.Player;
using UnityEngine;

namespace Core.Game
{
    public class GameFactory : IGameFactory
    {
        public GameController GameController { get; private set; }

        private readonly IInjectService _injectService;
        private readonly IAddressableService _addressableService;
        private readonly ILoadingScreenService _loadingScreenService;

        public GameFactory(
            IInjectService injectService,
            IAddressableService addressableService,
            ILoadingScreenService loadingScreenService)
        {
            _injectService = injectService;
            _addressableService = addressableService;
            _loadingScreenService = loadingScreenService;
        }

        public async UniTask Initialize()
        {
            _loadingScreenService.Show();

            await InstantiateGameController();

            _loadingScreenService.Hide();
        }

        private async UniTask InstantiateGameController()
        {
            var gameView = await InstantiateGameView();
            var enemyController = InstatiateEnemyController();

            GameController = new GameController(
                gameView,
                enemyController);

            _injectService.Inject(GameController);
            await GameController.Initialize();
        }

        private EnemyController InstatiateEnemyController()
        {
            var instance = new EnemyController();
            _injectService.Inject(instance);
            return instance;
        }

        private async UniTask<GameView> InstantiateGameView()
        {
            var prefab = await _addressableService.LoadAssetAsync<GameObject>(AddressableKeysHelper.GameViewPrefabKey);
            var instance = _injectService.InstantiatePrefab(prefab);
            return instance.GetComponent<GameView>();
        }
    }
}