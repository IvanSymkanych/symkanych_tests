using Core.Service;
using Cysharp.Threading.Tasks;
using Enums;
using Modules.Enemy;
using Modules.Player;
using UnityEngine;
using UnityEngine.Pool;

namespace Core.Game.Pools
{
    public class EnemyPool
    {
        public ObjectPool<IEnemy> Pool { get; private set; }

        private readonly IAddressableService _addressableService;
        private readonly IInjectService _injectService;

        private IEnemy _prefab;

        public EnemyPool(
            IAddressableService addressableService,
            IInjectService injectService)
        {
            _addressableService = addressableService;
            _injectService = injectService;
        }

        public async UniTask Initialize(string prefabAddressableKey)
        {
            var asset = await _addressableService.LoadAssetAsync<GameObject>(prefabAddressableKey);
            _prefab = asset.GetComponent<IEnemy>();
            Pool = new ObjectPool<IEnemy>(
                InstantiateEnemy,
                OnGetPool,
                OnReleasePool,
                OnDestroyPool,
                true,
                5,
                20);
        }

        private IEnemy InstantiateEnemy()
        {
            var prefab = _injectService.InstantiatePrefab(_prefab.GameObject);
            var instance = prefab.GetComponent<IEnemy>();
            instance.Initialize();
            instance.OnDespawn += Pool.Release;
            instance.GameObject.SetActive(false);
            return instance;
        }

        private void OnGetPool(IEnemy instance)
        {
        }

        private void OnReleasePool(IEnemy instance)
        {
        }

        private void OnDestroyPool(IEnemy instance) =>
            Object.Destroy(instance.GameObject);
    }
}