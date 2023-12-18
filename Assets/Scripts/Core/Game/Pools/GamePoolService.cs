using System.Collections.Generic;
using Core.Service;
using Cysharp.Threading.Tasks;
using Helpers;

namespace Core.Game.Pools
{
    public class GamePoolService : IGamePoolService
    {
        public PlayerProjectilePool PlayerProjectilePool { get; private set; }
        public BlueEnemyProjectilePool BlueEnemyProjectilePool { get; private set; }
        public EnemyPool RedEnemyPool { get; private set; }
        public EnemyPool BlueEnemyPool { get; private set; }
        
        private readonly IAddressableService _addressableService;
        private readonly IInjectService _injectService;
        
        public GamePoolService(
            IAddressableService addressableService,
            IInjectService injectService)
        {
            _addressableService = addressableService;
            _injectService = injectService;
        }

        public async UniTask Initialize()
        {
            var taskList = new List<UniTask>()
            {
                InitializePlayerProjectilePool(),
                InitializeBlueEnemyProjectilePool(),
                InitializeRedEnemyPool(),
                InitializeBlueEnemyPool()
            };

            await UniTask.WhenAll(taskList);
        }

        private async UniTask InitializePlayerProjectilePool()
        {
            PlayerProjectilePool = new PlayerProjectilePool(_addressableService);
            await PlayerProjectilePool.Initialize();
        }
        
        private async UniTask InitializeBlueEnemyProjectilePool()
        {
            BlueEnemyProjectilePool = new BlueEnemyProjectilePool(_addressableService);
            await BlueEnemyProjectilePool.Initialize();
        }
        
        private async UniTask InitializeRedEnemyPool()
        {
            RedEnemyPool = new EnemyPool(_addressableService,_injectService);
            await RedEnemyPool.Initialize(AddressableKeysHelper.RedEnemyPrefabKey);
        }
        
        private async UniTask InitializeBlueEnemyPool()
        {
            BlueEnemyPool = new EnemyPool(_addressableService, _injectService);
            await BlueEnemyPool.Initialize(AddressableKeysHelper.BlueEnemyPrefabKey);
        }
    }
}