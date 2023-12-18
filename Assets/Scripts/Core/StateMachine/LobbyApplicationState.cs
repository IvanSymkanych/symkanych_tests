using Core.Lobby;
using Core.Service;
using Cysharp.Threading.Tasks;
using Helpers;

namespace Core.StateMachine
{
    public class LobbyApplicationState : IApplicationState

    {
    private readonly ILobbyFactory _lobbyFactory;
    private readonly ISceneLoadService _sceneLoadService;

    public LobbyApplicationState(
        ILobbyFactory lobbyFactory,
        ISceneLoadService sceneLoadService)
    {
        _lobbyFactory = lobbyFactory;
        _sceneLoadService = sceneLoadService;
    }

    public async UniTask Enter()
    {
        await _sceneLoadService.LoadSceneAsync(AddressableKeysHelper.LobbySceneKey);
        await _lobbyFactory.Initialize();
    }

    public void Exit()
    {
    }
    }
}