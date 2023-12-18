using System;
using Enums;
using Modules.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Modules.Enemy
{
    public interface IEnemy
    {
        event Action<IEnemy, PlayerDamageType> OnDeath;
        event Action<IEnemy> OnDespawn;
        GameObject GameObject { get; }
        EnemyType EnemyType { get; }
        int MaxLife { get; }
        void Initialize();
        void Spawn(Vector3 spawnPosition);
        bool IsDeathHit(int damage);
        void TakeDamage(int damage, PlayerDamageType playerDamageType);
    }
}