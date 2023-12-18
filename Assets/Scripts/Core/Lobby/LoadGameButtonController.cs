using Core.StateMachine;
using Zenject;

namespace Core.Lobby
{
    public class LoadGameButtonController 
    {
        public LoadGameButtonView LoadGameButtonView { get; private set; }
        
        private ApplicationStateMachine _stateMachine;

        [Inject]
        public void Construct(ApplicationStateMachine stateMachine) => _stateMachine = stateMachine;
        
        public LoadGameButtonController(LoadGameButtonView loadGameButtonView) => LoadGameButtonView = loadGameButtonView;
   
        public void Initialize()
        { 
            LoadGameButtonView.Init();
           SubscribeEvent();
        }

        public void CleanUp()
        {
            LoadGameButtonView.CleanUp();
            UnsubscribeEvent();
        }

        private void PlayButtonClicked() => _stateMachine.Enter<GameApplicationState>().Forget();
        private void SubscribeEvent() => LoadGameButtonView.OnPlayButtonClick += PlayButtonClicked;
        private void UnsubscribeEvent() => LoadGameButtonView.OnPlayButtonClick -= PlayButtonClicked;
    }
}