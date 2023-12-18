using System;
using Enums;
using Modules.Player;
using UnityEngine;
using Zenject;

namespace Modules.Enemy
{
    public abstract class BaseEnemy : MonoBehaviour, IEnemy
    {
        public event Action<IEnemy, PlayerDamageType> OnDeath;
        public event Action<IEnemy> OnDespawn;
        public GameObject GameObject => gameObject;
        public EnemyType EnemyType => enemyType;
        public int MaxLife => maxLife;

        [SerializeField] protected EnemyType enemyType;
        [SerializeField] protected int maxLife;
        
        [Inject] protected PlayerBehaviour PlayerBehaviour;
        
        protected int CurrentLife;

        public abstract void Initialize();
        public abstract void Spawn(Vector3 spawnPosition);
        
        public bool IsDeathHit(int damage) => 
            CurrentLife - damage > 0;
        
        public void TakeDamage(int damage, PlayerDamageType playerDamageType)
        {
            CurrentLife -= damage;

            if (IsAlive())
                return;

            Death(playerDamageType);
        }
        
        protected bool IsAlive() => CurrentLife > 0;

        protected virtual void Death(PlayerDamageType playerDamageType)
        {
            OnDeath?.Invoke(this, playerDamageType);
            Despawn();
            gameObject.SetActive(false);
        }

        protected void Despawn() => OnDespawn?.Invoke(this);
    }
}