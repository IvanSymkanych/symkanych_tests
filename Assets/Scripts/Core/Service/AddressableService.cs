using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Core.Service
{
    public class AddressableService : IAddressableService
    {
        private readonly Dictionary<string, AsyncOperationHandle> _completed = new();
        private readonly Dictionary<string, List<AsyncOperationHandle>> _handles = new();

        public async UniTask<T> LoadAssetAsync<T>(string key) where T : class
        {
            if (_completed.TryGetValue(key, out var completedHandle))
                return completedHandle.Result as T;

            var assetHandle = GetHandle<T>(key);
            var result = await assetHandle.Task as T;
            return result;
        }
        
        private AsyncOperationHandle GetHandle<T>(string key) where T : class
        {
            var assetHandle = Addressables.LoadAssetAsync<T>(key);
            assetHandle.Completed += handle => OnAssetDownloadComplete(handle, key);
            AddHandle(key, assetHandle);
            return assetHandle;
        }

        private AsyncOperationHandle GetHandle<T>(IResourceLocation location) where T : class
        {
            var assetHandle = Addressables.LoadAssetAsync<T>(location);
            assetHandle.Completed += handle => OnAssetDownloadComplete(handle, location.PrimaryKey);
            AddHandle(location.PrimaryKey, assetHandle);
            return assetHandle;
        }
        
        private void OnAssetDownloadComplete<T>(AsyncOperationHandle<T> handle, string key) where T : class
        {
            if (handle.Result != null)
                _completed[key] = handle;
        }

        private void AddHandle<T>(string key, AsyncOperationHandle<T> handle) where T : class
        {
            if (!_handles.TryGetValue(key, out var resourceHandle))
            {
                resourceHandle = new List<AsyncOperationHandle>();
                _handles[key] = resourceHandle;
            }

            resourceHandle.Add(handle);
        }
    }
}