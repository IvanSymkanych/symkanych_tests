using Core.StateMachine;
using Zenject;

namespace Core.Installers
{
    public class BootLoader : IInitializable
    {
        private readonly ApplicationStateMachine _applicationStateMachine;

        public BootLoader(ApplicationStateMachine applicationStateMachine) => _applicationStateMachine = applicationStateMachine;
        public void Initialize() => _applicationStateMachine.Enter<StartupApplicationState>().Forget();
        
    }
}