using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay
{
    public class Respawner : MonoBehaviour
    {
        [Min(0.1f)]
        [SerializeField]private float reachingSpeed = 10.0f;
        
        private Coroutine reachCoroutine = null;
        public void Reach(Transform transformToMove, Action OnStarted, Action<Vector3> OnReaching, Action OnReached)
        {
            if(reachCoroutine == null)
            {
                reachCoroutine = StartCoroutine(ReachCoroutine(transformToMove, OnStarted, OnReaching, OnReached));
            }
        }
        private IEnumerator ReachCoroutine(Transform transformToMove, Action OnStarted, Action<Vector3> OnReaching, Action OnReached)
        {
            OnStarted?.Invoke();

            float delta = 0.0f;
            Vector3 startPosition = transformToMove.position;
            Vector3 endPosition = transform.position;
            while(delta < 1.0f)
            {
                Vector3 lerpedPosition = Vector3.Lerp(startPosition, endPosition, delta);
                OnReaching?.Invoke(lerpedPosition);
                delta += reachingSpeed * Time.deltaTime;
                yield return null;
            }

            yield return null;
            OnReached?.Invoke();
            reachCoroutine = null;
        }
    }
}
