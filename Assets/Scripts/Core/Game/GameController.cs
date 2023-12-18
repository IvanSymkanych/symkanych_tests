using Core.Game.Pools;
using Cysharp.Threading.Tasks;
using Modules.Player;
using Modules.Tools;
using UnityEngine;
using Zenject;

namespace Core.Game
{
    public class GameController
    {
        public GameView GameView { get; private set; }
        public EnemyController EnemyController { get; private set; }
        
        private PlayerBehaviour _playerBehaviour;
        private IGamePoolService _gamePoolService;
        
        public GameController(
            GameView gameView,
            EnemyController enemyController
        )
        {
            GameView = gameView;
            EnemyController = enemyController;
        }

        [Inject]
        public void Construct(
            IGamePoolService gamePoolService, 
            PlayerBehaviour playerBehaviour)
        {
            _gamePoolService = gamePoolService;
            _playerBehaviour = playerBehaviour;
        }
        
        public async UniTask Initialize()
        {
            GameView.Initialize();
            await _gamePoolService.Initialize();

            InitializePlayerBehaviour();
            EnemyController.StartSpawn();
            SubscribeEvents();
        }
        
        private void InitializePlayerBehaviour()
        {
            var heathProgressBarController = new ProgressBarController(GameView.HealthBarView, 100, 100);
            var energyProgressBarController = new ProgressBarController(GameView.EnergyBarView, 100, 50);

            _playerBehaviour.Initialize(heathProgressBarController, energyProgressBarController);
        }

        private void GameOver()
        {
            GameView.GameOver();
            EnemyController.StopSpawn();
        }

        private void SubscribeEvents()
        {
            _playerBehaviour.OnDeath += GameOver;
        }

        private void UnSubscribeEvents()
        {
            _playerBehaviour.OnDeath -= GameOver;
        }
    }
}