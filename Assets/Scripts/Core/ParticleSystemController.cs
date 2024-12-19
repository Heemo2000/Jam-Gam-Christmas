using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Game.Core
{
    public class ParticleSystemController : MonoBehaviour
    {
        [SerializeField]private ParticleSystem[] particleSystems;
        
        private Coroutine playCoroutine;
        private ObjectPool<ParticleSystemController> particlePool;

        public void SetPool(ObjectPool<ParticleSystemController> pool)
        {
            particlePool = pool;
        }
        public void Play()
        {
            /*
            foreach(ParticleSystem particleSystem in particleSystems)
            {
                particleSystem.Play();
            }
            */

            if(playCoroutine == null)
            {
                playCoroutine = StartCoroutine(PlayParticle());
            }
        }

        public void Pause()
        {
            foreach(ParticleSystem particleSystem in particleSystems)
            {
                particleSystem.Pause();
            }
        }

        public void Stop()
        {
            foreach(ParticleSystem particleSystem in particleSystems)
            {
                particleSystem.Stop();
            }
        }

        private IEnumerator PlayParticle()
        {
            foreach(ParticleSystem particleSystem in particleSystems)
            {
                particleSystem.Play();
            }

            yield return new WaitUntil(()=> IsAllStopped());

            playCoroutine = null;
        }

        private bool IsAllStopped()
        {
            foreach(ParticleSystem particleSystem in particleSystems)
            {
                if(particleSystem.IsAlive())
                {
                    return false;
                }
            }

            return true;
        }

        private void HandleParticleState(GameState gameState)
        {
            switch(gameState)
            {
                case GameState.UnPaused:
                                        Play();
                                        break;
                case GameState.Paused:
                                        Pause();
                                        break;
            }
        }
        // Start is called before the first frame update
        private void Start()
        {
            GameStateManager.Instance.OnGameStateChanged.AddListener(HandleParticleState);
        }

        private void OnDestroy() 
        {
            GameStateManager.Instance.OnGameStateChanged.RemoveListener(HandleParticleState);
        }
    }
}
