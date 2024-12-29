using System.Collections;
using System.Collections.Generic;
using Game.Core;
using UnityEngine;
using UnityEngine.UI;


namespace Game.Gameplay
{
    public class GamePassedRegionIndicator : MonoBehaviour
    {
        [SerializeField]private Transform gamePassedRegion;
        [SerializeField]private Canvas arrowCanvas;
        [SerializeField]private Transform arrowIndicator;
        [SerializeField]private bool allowXRotation = false;
        [SerializeField]private bool allowYRotation = false;
        

        private GameInput gameInput;
        private bool show;
        public void SetGamePassedRegionActive(bool status)
        {
            show = status;
        }

        private void Start() 
        {
            gameInput = FindObjectOfType<GameInput>();
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if(gamePassedRegion != null)
            {
                gamePassedRegion.gameObject.SetActive(show);
            }
            
            if(arrowCanvas == null || arrowIndicator == null)
            {
                return;
            }
            arrowCanvas.gameObject.SetActive(show);
            arrowIndicator.gameObject.SetActive(show);
            if(!show)
            {
                return;
            }

            Vector3 direction = (gamePassedRegion.position - transform.position).normalized;

            float xAngle = (allowXRotation) ? Vector3.SignedAngle(transform.forward, direction, Vector3.right) : 0.0f;
            float yAngle = (allowYRotation) ? Vector3.SignedAngle(transform.forward, direction, Vector3.up) : 0.0f;

            arrowIndicator.eulerAngles = new Vector3(xAngle, yAngle, 0.0f);
        }
    }
}
