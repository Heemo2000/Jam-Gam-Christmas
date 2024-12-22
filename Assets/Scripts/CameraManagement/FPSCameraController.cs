using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Game.CameraManagement
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class FPSCameraController : MonoBehaviour
    {
        [Min(45.0f)]
        [SerializeField]private float defaultFOV = 45.0f;

        [Min(45.0f)]
        [SerializeField]private float maxFOV = 90.0f;

        [Min(1.0f)]
        [SerializeField]private float fovIncreaseRate = 10.0f;
        [Header("Idle Bobbling settings:")]
        [SerializeField]private NoiseSettings noiseProfile;
        [SerializeField]private Vector3 pivotOffset = Vector3.zero;
        
        [Min(0.0f)]
        [SerializeField]private float amplitudeGain = 0.1f;
        [Min(0.0f)]
        [SerializeField]private float frequencyGain = 0.1f;
        
        private CinemachineVirtualCamera virtualCamera;
        private CinemachineBasicMultiChannelPerlin multiChannelPerlin;
        private float verticalRotation = 0.0f;
        private float fov = 60.0f;
        public void HandleCameraFeel(Vector2 moveInput, float normalSpeed, float currentSpeed, float lookInputY, float lookSpeed)
        {
            //Debug.Log("Current Local X Angle: " + transform.localEulerAngles.x);
            
            float targetAngleInput = lookInputY * lookSpeed;
            verticalRotation = Mathf.Clamp(verticalRotation - targetAngleInput, -90.0f, 90.0f);
            
            //Debug.Log("Vertical Rotation: " + verticalRotation);
            transform.localRotation = Quaternion.Euler(verticalRotation, 0.0f, 0.0f);

            float ratio = currentSpeed / normalSpeed;
            
            float inverseRatio = Mathf.Abs(ratio - 1.0f);
            //Debug.Log("Inverse Ratio: " + inverseRatio);

            float targetFOV = defaultFOV + fovIncreaseRate * inverseRatio;

            targetFOV = Mathf.Clamp(targetFOV, defaultFOV, maxFOV);

            virtualCamera.m_Lens.FieldOfView = targetFOV;

            if(multiChannelPerlin == null)
            {
                Debug.LogError("Multi Channel Perlin is null");
                return;
            }

            if(moveInput.sqrMagnitude == 0.0f)
            {
                multiChannelPerlin.m_PivotOffset = pivotOffset;
            }
            else
            {
                multiChannelPerlin.m_PivotOffset = Vector3.zero;
            }
        }

        private void Awake() 
        {
            virtualCamera = GetComponent<CinemachineVirtualCamera>();
        }

        private void Start() 
        {
            fov = defaultFOV;
            virtualCamera.m_Lens.FieldOfView = fov; 
            multiChannelPerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if(multiChannelPerlin == null)
            {
                multiChannelPerlin = virtualCamera.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            }
            multiChannelPerlin.m_NoiseProfile = noiseProfile;
            multiChannelPerlin.m_PivotOffset = pivotOffset;
            multiChannelPerlin.m_AmplitudeGain = amplitudeGain;
            multiChannelPerlin.m_FrequencyGain = frequencyGain;
        }        
    }
}
