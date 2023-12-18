using Cysharp.Threading.Tasks;

namespace Core.Lobby
{
    public interface ILobbyFactory
    { 
        LobbyController LobbyController { get; }
        UniTask Initialize();
    }
}