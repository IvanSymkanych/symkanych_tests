using System;
using Core.Service;
using Enums;
using Helpers;
using Modules.Enemy;
using Modules.Player;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

namespace Core.Game
{
    public class RewardService : IRewardService
    {
        private const int EnergyCountForBlueEnemy = 50;
        private const int EnergyCountForRedEnemy = 15;

        public int Score { get; private set; }

        private readonly PlayerBehaviour _playerBehaviour;
        private readonly IPlayerPrefsService _playerPrefsService;
        private readonly IGameFactory _gameFactory;

        public RewardService(
            PlayerBehaviour playerBehaviour,
            IPlayerPrefsService playerPrefsService,
            IGameFactory gameFactory
        )
        {
            _playerPrefsService = playerPrefsService;
            _playerBehaviour = playerBehaviour;
            _gameFactory = gameFactory;
        }

        public void EnemyDeath(IEnemy enemy, PlayerDamageType playerDamageType)
        {
            Score++;
            _gameFactory.GameController.GameView.KillProgressView.SetScore(Score);

            if (playerDamageType.HasFlag(PlayerDamageType.Ultimate))
                return;

            switch (enemy.EnemyType)
            {
                case EnemyType.RedEnemy:
                    SetReward(EnergyCountForRedEnemy, playerDamageType.HasFlag(PlayerDamageType.Ricochet));
                    break;
                case EnemyType.BlueEnemy:
                default:
                    SetReward(EnergyCountForBlueEnemy, playerDamageType.HasFlag(PlayerDamageType.Ricochet));
                    break;
            }
        }

        public void SaveScore()
        {
            var highScore = _playerPrefsService.GetInt(PlayerPrefsKeyHelper.HighScorePrefsKey);

            if (Score <= highScore)
                return;

            _playerPrefsService.SetInt(PlayerPrefsKeyHelper.HighScorePrefsKey, Score);
            _playerPrefsService.Save();
        }


        private void SetReward(int energyCount, bool deathByRicochet)
        {
            const int energyBonusChance = 70;

            var energyReward = energyCount;
            var lifeReward = 0;

            var lifeBonus = Random.Range(0, 100);

            if (deathByRicochet)
            {
                if (energyBonusChance >= lifeBonus)
                    energyReward += 10;
                else
                    lifeReward += _playerBehaviour.HeathProgressBarController.MaxValue / 2;
            }

            _playerBehaviour.EnergyProgressBarController.AddValueAnimated(energyReward);
            _playerBehaviour.HeathProgressBarController.AddValueAnimated(lifeReward);
        }
    }
}