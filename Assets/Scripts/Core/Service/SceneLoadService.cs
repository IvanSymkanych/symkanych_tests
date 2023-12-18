using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Core.Service
{ 
    public class SceneLoadService : ISceneLoadService
    {
        private readonly Dictionary<string, AsyncOperationHandle<SceneInstance>> _sceneCache = new ();
        
        public async UniTask LoadSceneAsync(string name)
        {
            var isSceneCached = _sceneCache.TryGetValue(name, out var cachedSceneHandler);
            var sceneHandler = isSceneCached ? cachedSceneHandler : Addressables.LoadSceneAsync(name);
            
            if (isSceneCached)
                _sceneCache.Remove(name);

            await sceneHandler.Task;
            await sceneHandler.Result.ActivateAsync();
        }
    }
}