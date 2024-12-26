using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay
{
    [CreateAssetMenu(menuName = "Candy Cane/Candy Cane Data", fileName = "Candy Cane Data")]
    [System.Serializable]
    public class CandyCaneDataSO: ScriptableObject
    {
        [Min(1.0f)]
        public float rotateSpeed = 20.0f;

        [Min(1)]
        public int weight = 2;
        public CandyCaneType type = CandyCaneType.NormalCandyCane;
        
        [Min(1.0f)]
        public float speedIncrease = 5.0f;
        
        [Min(1.0f)]
        public float increasedJumpHeight = 5.0f;

        [Min(1.0f)]
        public float effectTime = 5.0f;

        [Min(1.0f)]
        public float maxDisplacement = 5.0f;

        [Min(0.1f)]
        public float moveSpeed = 5.0f;

        [Min(0.5f)]
        public float waitTime = 0.5f;
    }
}
