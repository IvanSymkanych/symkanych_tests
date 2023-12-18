using System;
using System.Collections.Generic;
using System.Linq;
using Core.Game;
using Enums;
using Helpers;
using Modules.Enemy;
using UnityEngine;

namespace Modules.Player
{
    [RequireComponent(typeof(Collider), typeof(Rigidbody))]
    public class PlayerProjectile : MonoBehaviour
    {
        public event Action<PlayerProjectile> OnDespawn;

        private const int Damage = 50;
        private const int ForceMultiplier = 600;

        [SerializeField] private LayerMask enemyLayerMask;

        private Rigidbody _rigidbody;
        private Collider _collider;

        private bool _canRicochet;
        private PlayerDamageType _playerDamageType;
        
        public void Shot(Vector3 spawnPosition, Vector3 shotDirection, bool canRicochet)
        {
            gameObject.SetActive(true);

            _canRicochet = canRicochet;
            _playerDamageType = PlayerDamageType.Default;
            
            transform.position = spawnPosition;
            var force = shotDirection * ForceMultiplier;
            transform.LookAt(force);
            _rigidbody.AddForce(force);
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IEnemy>(out var enemy))
            {
                if (enemy.IsDeathHit(Damage))
                {
                    if (_canRicochet)
                        TryRicochet(enemy);

                    enemy.TakeDamage(Damage, _playerDamageType);
                    return;
                }

                enemy.TakeDamage(Damage, _playerDamageType);
            }

            if (!other.TryGetComponent<ArenaBehaviour>(out var arenaBehaviour)) 
                Despawn();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<ArenaBehaviour>(out var arenaBehaviour))
                Despawn();
        }

        private void TryRicochet(IEnemy hitEnemy)
        {
            const float radius = 2.5f;
            _canRicochet = false;
            _playerDamageType = PlayerDamageType.Ricochet;
            
            var hitColliders = Physics.OverlapSphere(transform.position, radius, enemyLayerMask);

            var targetsEnemies = new List<IEnemy>();

            foreach (var c in hitColliders)
            {
                if(c.TryGetComponent<IEnemy>(out var enemy))
                    targetsEnemies.Add(enemy);
            }

            if (targetsEnemies.Contains(hitEnemy))
                targetsEnemies.Remove(hitEnemy);

            if (targetsEnemies.Count <= 0)
                return;
            
            var positionList = targetsEnemies.Select(e => e.GameObject.transform.position).ToList();
            var nearestPosition = GameHelper.FindNearestPosition(positionList, hitEnemy.GameObject.transform.position);
            var direction = nearestPosition - transform.position;
            var force = direction.normalized * ForceMultiplier;
            transform.LookAt(force);
            _rigidbody.AddForce(force);
        }

        private void Despawn()
        {
            _rigidbody.velocity = Vector3.zero;
            _playerDamageType = PlayerDamageType.Default;
            OnDespawn?.Invoke(this);
        }
    }
}