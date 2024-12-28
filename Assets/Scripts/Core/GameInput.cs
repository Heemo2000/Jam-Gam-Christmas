using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Game.Input;
using System.Collections;

namespace Game.Core
{
    public class GameInput : MonoBehaviour
    {
        
        [SerializeField]private Vector2 lookSpeed = Vector2.one;
        [SerializeField]private Vector2 sensitivity = Vector2.zero;

        [Min(0.01f)]
        [SerializeField]private float jumpPressedTime = 0.1f;
        [SerializeField]private Camera lookCamera;

        public Action OnPausePressed;
        public Action OnInteractPressed;
        
        private PlayerInputActions playerInputActions;
        private bool sprintPressed = false;
        private bool jumpPressed = false;
        private Coroutine jumpPressedCoroutine = null;
        private Vector2 currentLookDelta = Vector2.zero;

        public bool SprintPressed { get => sprintPressed; }
        public bool JumpPressed { get => jumpPressed; }
        public Camera LookCamera { get => lookCamera; }

        public Vector2 GetSmoothLookDelta()
        {
            Vector2 targetLookDelta = GetLookDelta();
            currentLookDelta.x = Mathf.Lerp(currentLookDelta.x, targetLookDelta.x, lookSpeed.x * Time.deltaTime);
            currentLookDelta.y = Mathf.Lerp(currentLookDelta.y, targetLookDelta.y, lookSpeed.y * Time.deltaTime);
            return currentLookDelta;
        }
        public Vector2 GetLookDelta()
        {
            Vector2 lookDelta = playerInputActions.PlayerActionMap.Look.ReadValue<Vector2>() * sensitivity;
            return lookDelta;
        }
        public Vector2 GetMovementInputNormalized()
        {
            return GetMovementInput().normalized;
        }
        public Vector2 GetMovementInput()
        {
            return playerInputActions.PlayerActionMap.Movement.ReadValue<Vector2>();
        }

        private IEnumerator PressJump()
        {
            jumpPressed = true;
            yield return new WaitForSeconds(jumpPressedTime);
            jumpPressed = false;
            jumpPressedCoroutine = null;
        }

        private void OnJumpPressed(InputAction.CallbackContext context)
        {
            if(jumpPressedCoroutine == null)
            {
                jumpPressedCoroutine = StartCoroutine(PressJump());
            }
        }

        private void OnPausePress(InputAction.CallbackContext context)
        {
            OnPausePressed?.Invoke();   
        }

        private void OnSprintPressed(InputAction.CallbackContext context)
        {
            sprintPressed = true;
        }

        private void OnSprintReleased(InputAction.CallbackContext context)
        {
            sprintPressed = false;
        }

        private void OnInteraction(InputAction.CallbackContext context)
        {
            OnInteractPressed?.Invoke();
        }
        
        private void Awake() 
        {
            playerInputActions = new PlayerInputActions();
        }

        // Start is called before the first frame update
        void Start()
        {
            lookCamera = FindObjectOfType<Camera>();
            playerInputActions.Enable();
            playerInputActions.PlayerActionMap.Jump.Enable();
            playerInputActions.PlayerActionMap.Jump.started += OnJumpPressed;
            playerInputActions.PlayerActionMap.Pause.started += OnPausePress;
            playerInputActions.PlayerActionMap.Sprint.started += OnSprintPressed;
            playerInputActions.PlayerActionMap.Sprint.canceled += OnSprintReleased;

            playerInputActions.PlayerActionMap.Interact.Enable();
            playerInputActions.PlayerActionMap.Interact.started += OnInteraction;
        }

        private void OnDestroy() 
        {
            playerInputActions.Disable();
            playerInputActions.PlayerActionMap.Jump.Disable();
            playerInputActions.PlayerActionMap.Jump.started -= OnJumpPressed;
            playerInputActions.PlayerActionMap.Pause.started -= OnPausePress;
            playerInputActions.PlayerActionMap.Sprint.started -= OnSprintPressed;
            playerInputActions.PlayerActionMap.Sprint.canceled -= OnSprintReleased;

            playerInputActions.PlayerActionMap.Interact.Disable();
            playerInputActions.PlayerActionMap.Interact.started -= OnInteraction;
        }
    }
}
