using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.CameraManagement;
namespace Game.Gameplay
{
    public class Player : MonoBehaviour
    {
        [Min(1.0f)]
        [SerializeField]private float walkingSpeed = 5.0f;
        
        [Min(5.0f)]
        [SerializeField]private float sprintingSpeed = 5.0f;

        [Min(0.1f)]
        [SerializeField]private float acceleration = 1.0f;

        [Min(0.1f)]
        [SerializeField]private float rotatingSpeed = 5.0f;

        [Min(1.0f)]
        [SerializeField]private float lookSpeed = 10.0f;
        [SerializeField]private FPSCameraController cameraController;

        private GameInput gameInput;
        private CharacterMovement movement;
        private Vector2 moveInput;
        private Vector2 normalLookDelta;
        private Vector2 smoothLookDelta;
        private float currentSpeed = 0.0f;

        private void HandleJump()
        {
            
            if(movement.IsGrounded())
            {
                //Debug.Log("Jumping");
                movement.Jump();
            }
            else
            {
                //Debug.Log("Ground not detected");
            }
            
        }

        private void Awake() 
        {
            movement = GetComponent<CharacterMovement>();
        }
        // Start is called before the first frame update
        void Start()
        {
            gameInput = FindObjectOfType<GameInput>(true);
            gameInput.gameObject.SetActive(true);
            gameInput.enabled = true;
        }

        private void Update() 
        {
            movement.CalculateParameters();
            if(gameInput.JumpPressed)
            {
                HandleJump();
            }
            
            moveInput = gameInput.GetMovementInputNormalized();
            normalLookDelta = gameInput.GetLookDelta();
            smoothLookDelta = gameInput.GetSmoothLookDelta();

            float targetSpeed = (gameInput.SprintPressed && moveInput.y > 0.0f) ? sprintingSpeed : walkingSpeed;
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, acceleration * Time.fixedDeltaTime);

            cameraController.HandleCameraFeel(moveInput, walkingSpeed, currentSpeed, normalLookDelta.y, lookSpeed);
        }

        private void FixedUpdate() 
        {
            Vector2 moveInput = gameInput.GetMovementInputNormalized();
            Vector2 lookDelta = gameInput.GetSmoothLookDelta();
            //Debug.Log("Look delta: " + lookDelta);
            
            movement.Move(moveInput, lookDelta.x, currentSpeed, rotatingSpeed);
        }
    }
}
