using Core.Service;
using Cysharp.Threading.Tasks;
using Helpers;
using Modules.Player;
using UnityEngine;
using UnityEngine.Pool;

namespace Core.Game.Pools
{
    public class PlayerProjectilePool
    {
        public ObjectPool<PlayerProjectile> Pool { get; private set; }

        private readonly IAddressableService _addressableService;

        private PlayerProjectile _prefab;
        
        public PlayerProjectilePool(IAddressableService addressableService) =>
            _addressableService = addressableService;

        public async UniTask Initialize()
        {
            var asset = await _addressableService.LoadAssetAsync<GameObject>(AddressableKeysHelper.PlayerProjectileKey);
            _prefab = asset.GetComponent<PlayerProjectile>();
            Pool = new ObjectPool<PlayerProjectile>(
                InstantiateProjectile, 
                OnGetPool, 
                OnReleasePool, 
                OnDestroyPool, 
                false, 
                10, 
                50);
        }
        private PlayerProjectile InstantiateProjectile()
        {
            var instance = Object.Instantiate(_prefab, Vector3.zero, Quaternion.identity);
            instance.OnDespawn += ReturnToPool;
            instance.gameObject.SetActive(false);
            return instance;
        }

        private void OnGetPool(PlayerProjectile instance)
        {
        }

        private void OnReleasePool(PlayerProjectile instance) =>
            instance.gameObject.SetActive(false);

        private void OnDestroyPool(PlayerProjectile instance) =>
            Object.Destroy(instance.gameObject);

        private void ReturnToPool(PlayerProjectile instance) =>
            Pool.Release(instance);
    }
}