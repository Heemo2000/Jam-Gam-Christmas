using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterMovement : MonoBehaviour
    {
        
        [Header("Ground Settings: ")]
        [SerializeField]private LayerMask groundMask;
        [SerializeField]private Transform groundCheck;
        [SerializeField]private float groundCheckRadius = 0.1f;
        [Header("Gravity Settings")]

        [Range(0.0f, 20.0f)]
        [SerializeField]private float gravity = 10.0f;

        [Min(0.0f)]
        [SerializeField]private float fallMultiplier = 1.0f;

        [SerializeField]private bool allowGravity = true;
        //[Header("Obstruction Settings: ")]
        //[SerializeField]private LayerMask obstructionLayerMask;
        [Header("Jump Settings: ")]
        [Min(1.0f)]
        [SerializeField]private float jumpHeight = 5.0f;

        private float velocityY = 0.0f;
        private CharacterController controller;

        private float initialJumpVelocity = 0.0f;

        public float JumpHeight { get => jumpHeight; set => jumpHeight = value; }

        public void Move(Vector2 moveInput, float lookInputX, float moveSpeed, float rotationSpeed)
        {
            Vector3 moveVector = transform.forward * moveInput.y + transform.right * moveInput.x;
            
            /*
            bool canMove = !Physics.CapsuleCast(transform.position, 
                                                transform.position + Vector3.up * controller.center.y, 
                                                controller.radius, 
                                                moveVector, 
                                                moveSpeed * Time.fixedDeltaTime, obstructionLayerMask.value);

            
            if(!canMove)
            {
                //Attempt only X movement
                Vector3 moveVectorX = transform.right * moveInput.x;
                moveVectorX.Normalize();
                canMove = moveVectorX.x != 0 && !Physics.CapsuleCast(transform.position, 
                                                transform.position + Vector3.up * controller.center.y, 
                                                controller.radius, 
                                                moveVectorX, 
                                                moveSpeed * Time.fixedDeltaTime, obstructionLayerMask.value);

                if(canMove)
                {
                    //Can move only on the X.
                    moveVector = moveVectorX;
                }
                else
                {
                    
                    Vector3 moveVectorZ = transform.forward * moveInput.y;
                    moveVectorZ.Normalize();
                    canMove = moveVectorZ.z != 0 && !Physics.CapsuleCast(transform.position, 
                                                transform.position + Vector3.up * controller.center.y, 
                                                controller.radius, 
                                                moveVectorZ, 
                                                moveSpeed * Time.fixedDeltaTime, obstructionLayerMask.value);

                    if(canMove)
                    {
                        //Can move only on the Z.
                        moveVector = moveVectorZ;
                    }
                    else
                    {
                        //Cannot move in any direction.
                        moveVector = Vector3.zero;
                    }
                }
            }
            */
             
            controller.Move((moveVector * moveSpeed + Vector3.up * velocityY) * Time.fixedDeltaTime);
            
            float targetRotationY = lookInputX * rotationSpeed;
            transform.Rotate(Vector3.up * targetRotationY);
        }

        public void Jump()
        {
            velocityY += initialJumpVelocity;
        }
        public void CalculateParameters()
        {
            if(!allowGravity)
            {
                return;
            }
            bool isFalling = velocityY < 0.0f;
    
            if(!IsGrounded())
            {
                float currentVelocityY = velocityY;
                float currentGravity = gravity;
                
                if(isFalling)
                {
                    currentGravity *= fallMultiplier;
                }
                float newVelocityY = currentVelocityY - (currentGravity * Time.fixedDeltaTime);
                float nextVelocityY = (currentVelocityY + newVelocityY) * 0.5f;
                velocityY = nextVelocityY;
            }
            else
            {
                velocityY = 0f;
            }

            initialJumpVelocity = Mathf.Sqrt(2.0f * gravity * jumpHeight);
        }
        public bool IsGrounded()
        {
            return Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask.value);
        }
        
        private void Awake() 
        {
            controller = GetComponent<CharacterController>();    
        }

        private void Start() {
            initialJumpVelocity = Mathf.Sqrt(2.0f * gravity * jumpHeight);
        }

        private void OnDrawGizmosSelected() 
        {
            if(groundCheck == null)
            {
                return;
            }

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);    
        }
    }
}
