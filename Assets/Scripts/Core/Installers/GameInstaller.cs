using Core.Game;
using Core.Game.Pools;
using Core.Service;
using Modules.Player;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private PlayerBehaviour playerBehaviour;
        [SerializeField] private ArenaBehaviour arenaBehaviour;
        
        [Inject]
        public void Construct(IInjectService injectService, DiContainer diContainer)
        {
            injectService.Set(diContainer);
        }
        
        public override void InstallBindings()
        {
            Container.BindInstance(playerBehaviour).AsSingle();
            Container.BindInstance(arenaBehaviour).AsSingle();
            Container.Bind<IRewardService>().To<RewardService>().AsSingle();
            Container.Bind<IGamePoolService>().To<GamePoolService>().AsSingle();
        }
    }
}