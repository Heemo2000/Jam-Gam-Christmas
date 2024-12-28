using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game.Gameplay
{
    public class CandyCane : MonoBehaviour
    {
        [SerializeField]private CandyCaneDataSO candyCaneDataSO;

        public CandyCaneDataSO Data { get => candyCaneDataSO; }

        private Coroutine moveCoroutine;

        private IEnumerator MoveUpAndDown()
        {
            Vector3 startPosition = transform.position;
            Vector3 endPosition = transform.position + Vector3.up * candyCaneDataSO.maxDisplacement;
            float delta = 0.0f;
            Random.InitState((int)System.DateTime.Now.Ticks);
            float randomSpeed = Random.Range(candyCaneDataSO.minMoveSpeed, candyCaneDataSO.maxMoveSpeed + 0.1f);
            while(GameStateManager.Instance != null && GameStateManager.Instance.IsGameOver() == false)
            {
                if(delta <= 1.0f)
                {
                    transform.position = Vector3.Lerp(startPosition, endPosition, delta);
                    delta += randomSpeed * Time.fixedDeltaTime;
                    yield return null;
                }
                else
                {
                    yield return new WaitForSeconds(candyCaneDataSO.waitTime);
                    Vector3 temp = startPosition;
                    startPosition = endPosition;
                    endPosition = temp;
                    delta = 0.0f;
                }
            }

            moveCoroutine = null;
        }

        // Start is called before the first frame update
        void Start()
        {
            if(candyCaneDataSO == null)
            {
                return;
            }
            else if(candyCaneDataSO.type == CandyCaneType.MovingCandyCane && moveCoroutine == null)
            {
                moveCoroutine = StartCoroutine(MoveUpAndDown());
            }
        }

        
        void FixedUpdate()
        {
            bool check = GameStateManager.Instance != null && 
              (GameStateManager.Instance.IsGamePaused() == false || GameStateManager.Instance.IsGameOver() == false);
            if(!check)
            {
                return;
            }
            else if(candyCaneDataSO == null)
            {
                return;
            }

            transform.Rotate(Vector3.up * candyCaneDataSO.rotateSpeed * Time.fixedDeltaTime);
        }

        private void OnDestroy() 
        {
            if(moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
                moveCoroutine = null;
            }    
        }
    }
}
