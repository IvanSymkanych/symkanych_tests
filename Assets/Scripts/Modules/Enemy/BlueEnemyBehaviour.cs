using System;
using System.Collections;
using System.Threading;
using Core.Game;
using Core.Game.Pools;
using Enums;
using Helpers;
using Modules.Player;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Modules.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class BlueEnemyBehaviour : BaseEnemy
    {
        private BlueEnemyProjectilePool _pool;
        private CancellationTokenSource _cancellationTokenSource;
        private Coroutine _lifeCycleCoroutine;
        private NavMeshAgent _navMeshAgent;
        private Transform _playerTransform;
        private ArenaBehaviour _arenaBehaviour;
        
        [Inject]
        public void Construct(
            IGamePoolService gamePoolService, 
            PlayerBehaviour playerBehaviour, 
            ArenaBehaviour arenaBehaviour)
        {
            _pool = gamePoolService.BlueEnemyProjectilePool;
            _playerTransform = playerBehaviour.transform;
            _arenaBehaviour = arenaBehaviour;
        }

        public override void Initialize()
        {
            CurrentLife = maxLife;
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.enabled = false;
        }

        public override void Spawn(Vector3 spawnPosition)
        {
            gameObject.SetActive(true);
            CurrentLife = maxLife;
            _navMeshAgent.enabled = true;
            _navMeshAgent.Warp(spawnPosition);
            _lifeCycleCoroutine = StartCoroutine(LifeCycle());
        }
        
        private IEnumerator LifeCycle()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);

                var movePosition = _arenaBehaviour.GetRandomPositionInArena();
                _navMeshAgent.SetDestination(movePosition);
                yield return new WaitUntil(_navMeshAgent.ReachedDestinationOrGaveUp);

                for (var i = 0; i < 3; i++)
                {
                    yield return new WaitForSeconds(1f);

                    if (!PlayerInAttackRange()) 
                        continue;
                    
                    Attack();
                }
            }
        }
        
        protected override void Death(PlayerDamageType playerDamageType)
        {
            base.Death(playerDamageType);
            StopCoroutine(_lifeCycleCoroutine);
        }

        private void Attack()
        {
            transform.LookAt(_playerTransform);
            _pool.Pool.Get().Shot(transform.position, transform.forward);
        }
        
        private bool PlayerInAttackRange()
        {
            const float attackRange = 5f;
            var distance = Vector3.Distance(transform.position, PlayerBehaviour.transform.position);
            return distance <= attackRange;
        }
    }
}