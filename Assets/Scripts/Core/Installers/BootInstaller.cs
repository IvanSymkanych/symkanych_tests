using Zenject;

namespace Core.Installers
{
    public class BootInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<BootLoader>().AsSingle().NonLazy();
        }
    }
}