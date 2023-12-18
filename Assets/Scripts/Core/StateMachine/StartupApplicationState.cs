using Core.Service;
using Core.Startup;
using Cysharp.Threading.Tasks;
using Helpers;

namespace Core.StateMachine
{
    public class StartupApplicationState : IApplicationState
    {
        private readonly ApplicationStateMachine _applicationStateMachine;
        private readonly ISceneLoadService _sceneLoadService;
        private readonly IStartupFactory _startupFactory;

        public StartupApplicationState(
            ApplicationStateMachine applicationStateMachine,
            ISceneLoadService sceneLoadService,
            IStartupFactory startupFactory)
        {
            _sceneLoadService = sceneLoadService;
            _startupFactory = startupFactory;
            _applicationStateMachine = applicationStateMachine;
        }

        public async UniTask Enter()
        {
            await _sceneLoadService.LoadSceneAsync(AddressableKeysHelper.StartupSceneKey);
            await _startupFactory.Initialize();
            _applicationStateMachine.Enter<LobbyApplicationState>().Forget();
        }

        public void Exit()
        {
        }
    }
}