using System.Collections.Generic;
using System.Linq;
using Core.Service;
using Cysharp.Threading.Tasks;
using Helpers;
using Modules.DebugTools;
using Modules.UI;
using UnityEngine;

namespace Core.Startup
{
    public class StartupFactory : IStartupFactory
    {
        private readonly IAddressableService _addressableService;
        private readonly ISceneLoadService _sceneLoadService;
        private readonly ILoadingScreenService _loadingScreenService;
        private readonly IInjectService _injectService;

        public StartupFactory(
            IAddressableService addressableService,
            ISceneLoadService sceneLoadService,
            ILoadingScreenService loadingScreenService,
            IInjectService injectService)
        {
            _addressableService = addressableService;
            _sceneLoadService = sceneLoadService;
            _loadingScreenService = loadingScreenService;
            _injectService = injectService;
        }
        
        public async UniTask Initialize()
        {
            Application.targetFrameRate = 60;
            
            RegisterLoadingScreen();
            
            _loadingScreenService.Show();
            
            // var taskList = new List<UniTask>()
            // {
            //     _addressableService.PreloadAssetsWithLabel<GameObject>(AddressableKeysHelper.Label.LobbyLabel),
            //     _sceneLoadService.PrepareSceneAsync(AddressableKeysHelper.LobbySceneKey),
            // };
            //
            // await UniTask.WhenAll(taskList);
        }
        
        private void RegisterLoadingScreen()
        {
            var instance = Object.FindObjectOfType<LoadingScreenBehaviour>();
            Object.DontDestroyOnLoad(instance);
            _loadingScreenService.SetBehaviourInstance(instance);
        }
    }
}