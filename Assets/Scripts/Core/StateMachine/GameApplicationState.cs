using Core.Game;
using Core.Service;
using Cysharp.Threading.Tasks;
using Helpers;

namespace Core.StateMachine
{
    public class GameApplicationState : IApplicationState
    {
        private readonly IGameFactory _gameFactory;
        private readonly ISceneLoadService _sceneLoadService;

        public GameApplicationState(
            IGameFactory gameFactory,
            ISceneLoadService sceneLoadService)
        {
            _gameFactory = gameFactory;
            _sceneLoadService = sceneLoadService;
        }

        public async UniTask Enter()
        {
            await _sceneLoadService.LoadSceneAsync(AddressableKeysHelper.GameSceneKey);
            await _gameFactory.Initialize();
        }

        public void Exit()
        {
        }
    }
}