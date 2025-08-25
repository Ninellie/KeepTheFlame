using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerMovement
{
    public class PlayerMovementInputHandler : MonoBehaviour
    {
        public event Action<Vector2> OnRun;
        public event Action OnIdle;
        
        public void OnMove(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<Vector2>().normalized;
            if (value != Vector2.zero) OnRun?.Invoke(value);
            else OnIdle?.Invoke();
        }

        private void Start()
        {
            var playerInput = gameObject.GetComponent<PlayerInput>();
            playerInput.SwitchCurrentActionMap("Player");
        }
    }
}