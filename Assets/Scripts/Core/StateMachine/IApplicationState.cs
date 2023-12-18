using Cysharp.Threading.Tasks;

namespace Core.StateMachine
{
    public interface IApplicationState
    {
        UniTask Enter();
        void Exit();
    }
}