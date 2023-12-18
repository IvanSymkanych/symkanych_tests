using Modules.UI;

namespace Core.Service
{
    public class LoadingScreenService : ILoadingScreenService
    {
        private LoadingScreenBehaviour _loadingScreenBehaviour;

        public void SetBehaviourInstance(LoadingScreenBehaviour instance)
        {
            _loadingScreenBehaviour = instance;
            _loadingScreenBehaviour.Initialize();
        }
        
        public void Show()
        {
            _loadingScreenBehaviour.Show();
        }

        public void Hide()
        {
            _loadingScreenBehaviour.Hide();
        }
    }
}