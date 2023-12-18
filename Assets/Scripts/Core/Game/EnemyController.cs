using System;
using System.Collections.Generic;
using Core.Game.Pools;
using Cysharp.Threading.Tasks;
using Enums;
using Modules.Enemy;
using UnityEngine;
using Zenject;

namespace Core.Game
{
    public class EnemyController
    {
        private const int InitialSpawnInterval = 10;
        private const int MinSpawnInterval = 6;
        private const int MaxEnemiesOnMap = 30;
        private const int MaxRedEnemiesPerCycleSpawn = 4;
        private const int SpawnDecreaseStep = 2;

        private IGamePoolService _gamePoolService;
        private IRewardService _rewardService;
        private ArenaBehaviour _arenaBehaviour;

        private List<IEnemy> _enemies;

        private int _currentSpawnInterval;

        private bool _canSpawn;
        private bool _isBlueEnemySpawn;

        [Inject]
        public void Construct(
            IGamePoolService gamePoolService,
            IRewardService rewardService,
            ArenaBehaviour arenaBehaviour)
        {
            _gamePoolService = gamePoolService;
            _rewardService = rewardService;
            _arenaBehaviour = arenaBehaviour;
            _enemies = new List<IEnemy>();
        }

        public void StartSpawn()
        {
            _canSpawn = true;
            Spawner().Forget();
        }

        public void StopSpawn()
        {
            _canSpawn = false;
        }

        public void KillAllEnemyByUltimate()
        {
            var targets = new List<IEnemy>();
            targets.AddRange(_enemies);

            foreach (var enemy in targets)
            {
                enemy.TakeDamage(enemy.MaxLife, PlayerDamageType.Ultimate);
            }
        }

        private void SpawnRedEnemy()
        {
            var spawnPosition = _arenaBehaviour.GetRandomPositionInArenaWithoutPlayer();
            var instance = _gamePoolService.RedEnemyPool.Pool.Get();
            instance.Spawn(spawnPosition);
            instance.OnDeath += EnemyDeath;
            _enemies.Add(instance);
        }

        private void SpawnBlueEnemy()
        {
            var spawnPosition = _arenaBehaviour.GetRandomPositionInArenaWithoutPlayer();
            var instance = _gamePoolService.BlueEnemyPool.Pool.Get();
            instance.Spawn(spawnPosition);
            instance.OnDeath += EnemyDeath;
            _enemies.Add(instance);
        }

        private void EnemyDeath(IEnemy enemy, PlayerDamageType playerDamageType)
        {
            enemy.OnDeath -= EnemyDeath;
            _enemies.Remove(enemy);
            _rewardService.EnemyDeath(enemy, playerDamageType);
        }


        private async UniTask Spawner()
        {
            _isBlueEnemySpawn = true;
            _currentSpawnInterval = InitialSpawnInterval;
            var redEnemiesPerCycle = 1;

            while (_canSpawn)
            {
                if (_isBlueEnemySpawn)
                {
                    SpawnBlueEnemy();
                }
                else
                {
                    for (var i = 0; i < redEnemiesPerCycle; i++)
                    {
                        SpawnRedEnemy();
                    }
                }

                _isBlueEnemySpawn = !_isBlueEnemySpawn;
                _currentSpawnInterval = Mathf.Max(_currentSpawnInterval - SpawnDecreaseStep, MinSpawnInterval);
                redEnemiesPerCycle++;
                redEnemiesPerCycle = Mathf.Min(redEnemiesPerCycle, MaxRedEnemiesPerCycleSpawn);

                await UniTask.Delay(TimeSpan.FromSeconds(_currentSpawnInterval));
                await UniTask.WaitUntil((() => _enemies.Count <= MaxEnemiesOnMap));
            }
        }
    }
}