using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game.Gameplay
{
    public class PowerupVisual : MonoBehaviour
    {
        [SerializeField]private Material powerupMaterial;

        public bool GetVisualActiveStatus()
        {
            return powerupMaterial.GetInt(Constants.SHOW_EFFECT_PROPERTY) == 1;
        }
        
        public void SetColor(Color color)
        {
            if(powerupMaterial == null)
            {
                Debug.LogError("Powerup Material is null!");
                return;    
            }
            powerupMaterial.SetColor(Constants.COLOR_PROPERTY, color);
        }
        
        public void SetVisualActiveStatus(bool status)
        {
            if(powerupMaterial == null)
            {
                Debug.LogError("Powerup Material is null!");
                return;    
            }
            powerupMaterial.SetInt(Constants.SHOW_EFFECT_PROPERTY, status == true ? 1: 0);
        }
        private void Start() {
            SetVisualActiveStatus(false);
        }
    }
}
