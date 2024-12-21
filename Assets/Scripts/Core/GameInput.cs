using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Game.Input;

namespace Game.Core
{
    public class GameInput : MonoBehaviour
    {
        
        public Action OnPausePressed;
        public Action OnJump;
        private PlayerInputActions playerInputActions;
        
        public Vector2 GetMovementInputNormalized()
        {
            return GetMovementInput().normalized;
        }
        public Vector2 GetMovementInput()
        {
            return playerInputActions.PlayerActionMap.Movement.ReadValue<Vector2>();
        }

        private void OnJumpPressed(InputAction.CallbackContext context)
        {
            OnJump?.Invoke();
        }

        private void OnPausePress(InputAction.CallbackContext context)
        {
            OnPausePressed?.Invoke();   
        }
        
        private void Awake() 
        {
            playerInputActions = new PlayerInputActions();
        }

        // Start is called before the first frame update
        void Start()
        {
            playerInputActions.Enable();
            playerInputActions.PlayerActionMap.Jump.started += OnJumpPressed;
            playerInputActions.PlayerActionMap.Pause.started += OnPausePress;   
        }
    }
}
