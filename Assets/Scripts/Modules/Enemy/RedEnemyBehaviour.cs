using System;
using DG.Tweening;
using Modules.Player;
using UnityEngine;

namespace Modules.Enemy
{
    public class RedEnemyBehaviour : BaseEnemy
    {
        private Collider _collider;
        private bool _canFollow;

        public override void Initialize()
        {
            _collider = GetComponent<Collider>();
            CurrentLife = maxLife;
            _canFollow = false;
            _collider.enabled = false;
        }

        public override void Spawn(Vector3 spawnPosition)
        {
            gameObject.SetActive(true);
            CurrentLife = maxLife;
            transform.position = spawnPosition;
            _collider.enabled = true;
            PlaySpawnAnimation();
        }

        private void PlaySpawnAnimation()
        {
            DOTween.Sequence()
                .Append(transform.DOMoveY(transform.position.y + 2, 2))
                .AppendInterval(2)
                .OnComplete((() => _canFollow = true));
        }

        private void AttackPlayer(PlayerCollisionHandler playerCollisionHandler)
        {
            const int lifeDamage = 15;
            playerCollisionHandler.TakeLifeHit(lifeDamage);
            _canFollow = false;
            _collider.enabled = false;
            Despawn();
            gameObject.SetActive(false);
        }

        private void LateUpdate()
        {
            if (!_canFollow)
                return;

            transform.position =
                Vector3.Lerp(transform.position, PlayerBehaviour.transform.position, 4 * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlayerCollisionHandler>(out var collisionHandler))
                AttackPlayer(collisionHandler);
        }
    }
}