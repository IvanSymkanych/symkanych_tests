using Core.Game;
using Core.Lobby;
using Core.Service;
using Core.Startup;
using Core.StateMachine;
using Zenject;

namespace Core.Installers
{
    public class ApplicationInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ApplicationStateMachine>().AsSingle().NonLazy();

            InstallService();
            InstallFactories();
        }

        private void InstallService()
        {
            Container.Bind<IAddressableService>().To<AddressableService>().AsSingle();
            Container.Bind<ISceneLoadService>().To<SceneLoadService>().AsSingle();
            Container.Bind<IInjectService>().To<InjectedService>().AsSingle();
            Container.Bind<IPlayerPrefsService>().To<PlayerPrefsService>().AsSingle();
            Container.Bind<ILoadingScreenService>().To<LoadingScreenService>().AsSingle();
        }

        private void InstallFactories()
        {
            Container.Bind<IStartupFactory>().To<StartupFactory>().AsSingle();
            Container.Bind<ILobbyFactory>().To<LobbyFactory>().AsSingle();
            Container.Bind<IGameFactory>().To<GameFactory>().AsSingle();
        }
    }
}