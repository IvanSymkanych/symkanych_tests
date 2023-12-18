using Core.Service;
using Cysharp.Threading.Tasks;
using Helpers;
using Modules.Enemy;
using UnityEngine;
using UnityEngine.Pool;

namespace Core.Game.Pools
{
    public class BlueEnemyProjectilePool
    {
        public ObjectPool<BlueEnemyProjectile> Pool { get; private set; }

        private readonly IAddressableService _addressableService;

        private BlueEnemyProjectile _prefab;
        
        public BlueEnemyProjectilePool(IAddressableService addressableService) =>
            _addressableService = addressableService;

        public async UniTask Initialize()
        {
            var asset = await _addressableService.LoadAssetAsync<GameObject>(AddressableKeysHelper.BlueEnemyProjectileKey);
            _prefab = asset.GetComponent<BlueEnemyProjectile>();
            Pool = new ObjectPool<BlueEnemyProjectile>(
                InstantiateProjectile, 
                OnGetPool, 
                OnReleasePool, 
                OnDestroyPool, 
                false, 
                10, 
                50);
        }
        private BlueEnemyProjectile InstantiateProjectile()
        {
            var instance = Object.Instantiate(_prefab, Vector3.zero, Quaternion.identity);
            instance.OnDespawn += ReturnToPool;
            instance.Initialize();
            return instance;
        }

        private void OnGetPool(BlueEnemyProjectile instance)
        {
        }

        private void OnReleasePool(BlueEnemyProjectile instance) =>
            instance.gameObject.SetActive(false);

        private void OnDestroyPool(BlueEnemyProjectile instance) =>
            Object.Destroy(instance.gameObject);

        private void ReturnToPool(BlueEnemyProjectile instance) =>
            Pool.Release(instance);
    }
}