using Cysharp.Threading.Tasks;
using Modules.Player;

namespace Core.Game.Pools
{
    public interface IGamePoolService
    {
        PlayerProjectilePool PlayerProjectilePool { get; }
        BlueEnemyProjectilePool BlueEnemyProjectilePool { get; }
        EnemyPool RedEnemyPool { get;}
        EnemyPool BlueEnemyPool { get;}

        UniTask Initialize();
    }
}