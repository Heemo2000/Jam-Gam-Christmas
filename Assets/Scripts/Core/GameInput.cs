using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Game.Input;

namespace Game.Core
{
    public class GameInput : MonoBehaviour
    {
        
        public Action OnPausePressed;
        private PlayerInputActions playerInputActions;
        
        public Vector2 GetMovementInputNormalized()
        {
            return GetMovementInput().normalized;
        }
        public Vector2 GetMovementInput()
        {
            return Vector2.zero;
            //return playerInputActions.PlayerActionMap.Movement.ReadValue<Vector2>();
        }

        
        private void Awake() 
        {
            playerInputActions = new PlayerInputActions();
        }

        // Start is called before the first frame update
        void Start()
        {
            
            
        }

        private void Update() 
        {
               
        }

        private void OnDestroy() 
        {

        }
    }
}
