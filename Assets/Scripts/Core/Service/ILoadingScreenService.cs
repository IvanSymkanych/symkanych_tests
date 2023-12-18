using Modules.UI;

namespace Core.Service
{
    public interface ILoadingScreenService
    {
        void SetBehaviourInstance(LoadingScreenBehaviour instance);
        void Show();
        void Hide();
    }
}