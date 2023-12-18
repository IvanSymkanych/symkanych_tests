using System;
using System.Collections;
using Core.Game;
using Modules.Player;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Modules.Enemy
{
    public class BlueEnemyProjectile : MonoBehaviour
    {
        public event Action<BlueEnemyProjectile> OnDespawn;

        private Rigidbody _rigidbody;
        private Collider _collider;

        public void Initialize()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
            gameObject.SetActive(false);
        }

        public void Shot(Vector3 spawnPosition, Vector3 shotDirection)
        {
            const int forceMultiplier = 600;

            gameObject.SetActive(true);
            transform.position = spawnPosition;
            var force = shotDirection * forceMultiplier;
            transform.LookAt(force);
            _rigidbody.AddForce(force);
        }

        private void AttackPlayer(PlayerCollisionHandler playerCollisionHandler)
        {
            const int damage = 25;
            playerCollisionHandler.TakeEnergyHit(damage);
            _collider.enabled = false;
            Despawn();
        }

        private void Despawn()
        {
            _rigidbody.velocity = Vector3.zero;
            gameObject.SetActive(false);
            OnDespawn?.Invoke(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlayerCollisionHandler>(out var collisionHandler))
            {
                AttackPlayer(collisionHandler);
                return;
            }

            Despawn();
        }
    }
}