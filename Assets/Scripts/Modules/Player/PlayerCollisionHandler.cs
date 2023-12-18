using System;
using Core.Game;
using UnityEngine;

namespace Modules.Player
{
    [RequireComponent(typeof(Collider))]
    public class PlayerCollisionHandler : MonoBehaviour
    {
        public event Action OnArenaExit;
        public event Action<int> OnTakeLifeHit;
        public event Action<int> OnTakeEnergyHit;
        
        public void TakeLifeHit(int damage) => OnTakeLifeHit?.Invoke(damage);
        public void TakeEnergyHit(int damage) => OnTakeEnergyHit?.Invoke(damage);

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out ArenaBehaviour arena))
                OnArenaExit?.Invoke();
        }
    }
}