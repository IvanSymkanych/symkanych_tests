using Enums;
using Modules.Enemy;

namespace Core.Game
{
    public interface IRewardService
    {
        int Score { get; }
        void EnemyDeath(IEnemy enemy, PlayerDamageType playerDamageType);
        void SaveScore();
    }
}