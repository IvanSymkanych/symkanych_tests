using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public event Action OnFirePressed;
        public event Action OnUltimatePressed;


        public bool IsEnable;
        public Vector2 MoveDirection { get; private set; }
        public Vector2 LookDirection { get; private set; }
        
        public void OnMove(InputValue value)
        {
            MoveDirection = value.Get<Vector2>();
        }

        public void OnLook(InputValue value)
        {
            LookDirection = value.Get<Vector2>();
        }

        public void OnFire(InputValue value)
        {
            if (value.isPressed)
                OnFirePressed?.Invoke();
        }

        public void OnUltimate(InputValue value)
        {
            if (value.isPressed)
                OnUltimatePressed?.Invoke();
        }
    }
}