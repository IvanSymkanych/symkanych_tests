using Cysharp.Threading.Tasks;

namespace Core.Game
{
    public interface IGameFactory
    {
        GameController GameController { get; }
        UniTask Initialize();
    }
}