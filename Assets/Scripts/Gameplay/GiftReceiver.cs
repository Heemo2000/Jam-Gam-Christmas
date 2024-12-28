using System.Collections;
using System.Collections.Generic;
using Game.Core;
using UnityEngine;

namespace Game.Gameplay
{
    public class GiftReceiver : MonoBehaviour
    {
        [SerializeField]private Vector3 offset = Vector3.zero;
        [SerializeField]private bool shouldReceiveGifts = false;
        [SerializeField]private GameObject arrowPrefab;
        [Min(0.0f)]
        [SerializeField]private float arrowRotationSpeed = 10.0f;

        public Vector3 ReceivePosition { get => transform.position + offset; }
        public bool ShouldReceiveGifts { get => shouldReceiveGifts; set => shouldReceiveGifts = value; }
        private CandyCaneSpawner candyCaneSpawner;
        private GameObject arrow;

        private void Start() 
        {
            candyCaneSpawner = GetComponentInParent<CandyCaneSpawner>();
            shouldReceiveGifts = candyCaneSpawner.GenerateCandies;
            if(shouldReceiveGifts)
            {
                arrow = Instantiate(arrowPrefab, ReceivePosition, Quaternion.identity);
                arrow.transform.parent = transform;
            }
        }

        private void FixedUpdate() 
        {
            if(arrow == null)
            {
                return;
            }
            arrow.SetActive(shouldReceiveGifts);
            if(shouldReceiveGifts)
            {
                arrow.transform.Rotate(Vector3.up * arrowRotationSpeed * Time.fixedDeltaTime);
            }
        }

        private void OnDrawGizmos() 
        {
            if(!shouldReceiveGifts)
            {
                return;
            }    

            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(ReceivePosition, Constants.DEFAULT_RADIUS);
        }
    }
}
