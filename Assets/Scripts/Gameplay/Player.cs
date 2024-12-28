using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.CameraManagement;
using Game.SoundManagement;

namespace Game.Gameplay
{
    public class Player : MonoBehaviour
    {
        [Header("Movement Settings: ")]
        [Min(1.0f)]
        [SerializeField]private float walkingSpeed = 5.0f;
        
        [Min(5.0f)]
        [SerializeField]private float sprintingSpeed = 5.0f;

        [Min(0.1f)]
        [SerializeField]private float acceleration = 1.0f;

        [Min(0.1f)]
        [SerializeField]private float rotatingSpeed = 5.0f;

        [Header("Look Settings: ")]
        [Min(1.0f)]
        [SerializeField]private float lookSpeed = 10.0f;
        
        [SerializeField]private FPSCameraController cameraController;
        
        [Header("Collectable Settings: ")]
        [SerializeField]private LayerMask collectableMask;
        [SerializeField]private SoundData muchingSoundData;

        [Header("Game Over Settings: ")]
        [SerializeField]private LayerMask deadZoneMask;

        [Header("Game Passed Settings: ")]
        [SerializeField]private LayerMask gamePassedMask;

        [Header("Interact Settings: ")]
        [SerializeField]private LayerMask interactMask;
        [Min(1.0f)]
        [SerializeField]private float maxCheckDistance = 4.0f;
        [SerializeField]private float maxCheckRadius = 2.0f;
        [SerializeField]private AnimationCurve tossAnimCurve;
        [SerializeField]private GameObject[] gifts;
        [SerializeField]private SoundData tossGiftSoundData;
        [Min(1.0f)]
        [SerializeField]private float tossSpeed = 5.0f;
        [SerializeField]private float maxTossHeight = 2.0f;
        [Header("Feature Flags: ")]
        [SerializeField]private bool touchCandies = false;
        [SerializeField]private bool interactWithPillars = false;
        [SerializeField]private bool allowGameOver = false;
        [SerializeField]private bool allowGamePassed = false;

        private GameInput gameInput;
        private CharacterMovement movement;
        private Vector2 moveInput;
        private Vector2 normalLookDelta;
        private Vector2 smoothLookDelta;
        private float currentSpeed = 0.0f;

        private bool shouldMove = true;

        private Coroutine giftCoroutine;


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

        private IEnumerator IncreaseSpeedForLimitedTime(float speedIncrease, float time)
        {
            sprintingSpeed += speedIncrease;
            yield return new WaitForSeconds(time);
            sprintingSpeed -= speedIncrease;
        }

        private void OnStartRespawning()
        {
            shouldMove = false;
            movement.enabled = false;
        }

        private void OnDuringRespawning(Vector3 position)
        {
            transform.position = position;
            cameraController.HandleCameraFeel(Vector2.zero, walkingSpeed, walkingSpeed, 0.0f, 0.0f);
        }
        private void OnEndRespawning()
        {
            shouldMove = true;
        }

        private void HandleInteractions()
        {
            if(!interactWithPillars)
            {
                return;
            }
            
            //Debug.Log("Trying to find gift receivers");

            //Debug.DrawLine(cameraController.transform.position, cameraController.transform.position + cameraController.transform.forward * maxCheckDistance, Color.green, 2.0f);
            
            if(Physics.SphereCast(cameraController.transform.position, maxCheckRadius, 
                                  cameraController.transform.forward, out RaycastHit hit, 
                                  maxCheckDistance, interactMask.value))
            {
                if(!hit.transform.TryGetComponent<GiftReceiver>(out GiftReceiver giftReceiver))
                {
                    //Debug.Log("Can't find gift receiver");
                    return;
                }
                //Debug.Log("Found Gift receiver");
                if(giftReceiver.ShouldReceiveGifts && giftCoroutine == null)
                {
                    giftCoroutine = StartCoroutine(GiftCoroutine(giftReceiver));
                }    
            }
            else
            {
                //Debug.Log("Can't find anything");
            }   
        }

        private IEnumerator GiftCoroutine(GiftReceiver giftReceiver)
        {
            giftReceiver.ShouldReceiveGifts = false;
            Vector3 start = transform.position;
            Vector3 end = giftReceiver.ReceivePosition;
            GameObject gift = Instantiate(gifts[Random.Range(0, gifts.Length)], start, Quaternion.identity);
            float delta = 0.0f;
            while(delta <= 1.0f)
            {
                //Debug.Log("Tossing");
                Vector3 lerpedPosition = Vector3.Lerp(start, end, delta);
                lerpedPosition.y += tossAnimCurve.Evaluate(delta) * maxTossHeight;
                gift.transform.position = lerpedPosition;
                delta += tossSpeed * Time.fixedDeltaTime;
                yield return null;
            }

            start = giftReceiver.ReceivePosition;
            end = giftReceiver.transform.position;

            
            SoundManager.Instance.Play(tossGiftSoundData, start);
            //Vector3 startScale = Vector3.one;
            //Vector3 endScale = Vector3.one * 0.5f;
            delta = 0.0f;
            while(delta <= 1.0f)
            {
                //Debug.Log("Going");
                Vector3 lerpedPosition = Vector3.Lerp(start, end, delta);
                //Vector3 lerpedScale = Vector3.Lerp(startScale, endScale, delta);

                gift.transform.position = lerpedPosition;
                
                //Debug.Log("Lerped Position: " + lerpedPosition);
                //gift.transform.localScale = lerpedScale;
                
                delta += tossSpeed * Time.fixedDeltaTime;
            }
            
            giftCoroutine = null;
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
            gameInput.OnInteractPressed += HandleInteractions;
        }

        private void Update() 
        {
            if(GameStateManager.Instance != null && (GameStateManager.Instance.IsGamePaused() == true || GameStateManager.Instance.IsGameOver() == true))
            {
                shouldMove = false;
            }
            if(!shouldMove)
            {
                return;
            }
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
            if(GameStateManager.Instance != null && 
              (GameStateManager.Instance.IsGamePaused() || GameStateManager.Instance.IsGameOver() || GameStateManager.Instance.IsGamePassed()))
            {
                shouldMove = false;
            }

            if(!shouldMove)
            {
                return;
            }
            Vector2 moveInput = gameInput.GetMovementInputNormalized();
            Vector2 lookDelta = gameInput.GetSmoothLookDelta();
            //Debug.Log("Look delta: " + lookDelta);
            
            movement.Move(moveInput, lookDelta.x, currentSpeed, rotatingSpeed);
        }

        private void OnControllerColliderHit(ControllerColliderHit hit) 
        {
            //Debug.Log("Colliding with " + hit.gameObject.name);
            int layerMask = 1 << hit.gameObject.layer;
            
            if(allowGameOver && (deadZoneMask.value & layerMask) != 0)
            {
                if(RespawnManager.Instance.SpawnCount > 0)
                {
                    RespawnManager.Instance.DecreaseSpawnCount();
                    Respawner respawner = RespawnManager.Instance.FindClosestSpawner(transform.position);
                    respawner.Reach(transform, OnStartRespawning, OnDuringRespawning, OnEndRespawning);
                }
                else
                {
                    GameStateManager.Instance.EndGame();
                }
                
            }
            else if(allowGamePassed && (gamePassedMask.value & layerMask) != 0)
            {
                GameStateManager.Instance.PassGame();
            }
            else if(!touchCandies || (collectableMask.value & layerMask) == 0)
            {
                return;
            }

            if(!hit.transform.TryGetComponent<CandyCane>(out var candyCane))
            {
                Debug.LogError("Cannot find candy cane, please check collectableMask in Player!");
                return;
            }

            SoundManager.Instance.Play(muchingSoundData, hit.transform.position, true);   
            var data = candyCane.Data;

            switch(data.type)
            {
                case CandyCaneType.NormalCandyCane:
                                                    sprintingSpeed += data.speedIncrease;
                                                    break;

                case CandyCaneType.MovingCandyCane: 
                                                    StartCoroutine(IncreaseSpeedForLimitedTime(data.speedIncrease, data.effectTime));
                                                    break;
                
                case CandyCaneType.RespawnCandyCane:
                                                    RespawnManager.Instance.IncreaseSpawnCount();
                                                    break;
            }

            Destroy(candyCane.gameObject);
            
        }
    }
}
