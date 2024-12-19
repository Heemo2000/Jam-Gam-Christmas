using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Game.Core;

namespace Game.SoundManagement
{
    public class SoundManager : MonoSingelton<SoundManager>
    {
        [SerializeField]private SoundEmitter soundEmitterPrefab;
        [SerializeField]private bool collectionCheck = true;

        [Min(10)]
        [SerializeField]private int defaultCapacity = 10;
        
        [Min(10)]
        [SerializeField]private int maxPoolSize = 100;
        
        [Min(1)]
        [SerializeField]private int maxSoundInstances = 30;
        IObjectPool<SoundEmitter> soundEmitterPool;
        private readonly List<SoundEmitter> activeSoundEmitters = new();
        public readonly LinkedList<SoundEmitter> frequentSoundEmitters = new();

        
        public void Play(SoundData soundData, Vector3 position, bool randomPitch)
        {
            if(randomPitch)
            {
                CreateSoundBuilder().WithPosition(position).Play(soundData);
            }
            else
            {
                CreateSoundBuilder().WithPosition(position).WithRandomPitch().Play(soundData);
            }
        }

        

        public void Stop(SoundData soundData)
        {
            for(int i = 0; i < activeSoundEmitters.Count; i++)
            {
                SoundEmitter soundEmitter = activeSoundEmitters[i];
                if(soundEmitter.Data == soundData)
                {
                    soundEmitter.Stop();
                }
            }
        }

        public SoundBuilder CreateSoundBuilder()
        {
            return new SoundBuilder(this);
        }

        public bool CanPlaySound(SoundData data)
        {
            if(!data.frequentSound)
            {
                return true;
            }

            if(frequentSoundEmitters.Count >= maxSoundInstances)
            {
                try
                {
                    frequentSoundEmitters.First.Value.Stop();
                    return true;
                }
                catch
                {
                    Debug.LogWarning("SoundEmitter is already released");
                }

                return false;
            }

            return true;
        }

        public SoundEmitter Get()
        {
            return soundEmitterPool.Get();
        }

        public void ReturnToPool(SoundEmitter soundEmitter)
        {
            soundEmitterPool.Release(soundEmitter);
        }

        public void StopAll()
        {
            foreach(var soundEmitter in activeSoundEmitters)
            {
                soundEmitter.Stop();
            }

            frequentSoundEmitters.Clear();
        }
        protected override void InternalInit()
        {
            
        }

        protected override void InternalOnStart()
        {
            InitializePool();
        }

        protected override void InternalOnDestroy()
        {
            
        }

        private void InitializePool()
        {
            soundEmitterPool = new ObjectPool<SoundEmitter>(
                CreateSoundEmitter,
                OnTakeFromPool,
                OnReturnedToPool,
                OnDestroyPoolObject,
                collectionCheck,
                defaultCapacity,
                maxPoolSize
            );
        }

        private SoundEmitter CreateSoundEmitter()
        {
            var soundEmitter = Instantiate(soundEmitterPrefab);
            soundEmitter.gameObject.SetActive(false);
            return soundEmitter;
        }

        private void OnTakeFromPool(SoundEmitter soundEmitter)
        {
            soundEmitter.gameObject.SetActive(true);
            activeSoundEmitters.Add(soundEmitter);
        }

        private void OnReturnedToPool(SoundEmitter soundEmitter)
        {
            if(soundEmitter.Node != null)
            {
                frequentSoundEmitters.Remove(soundEmitter.Node);
                soundEmitter.Node = null;
            }
            soundEmitter.gameObject.SetActive(false);
            activeSoundEmitters.Remove(soundEmitter);
        }

        private void OnDestroyPoolObject(SoundEmitter soundEmitter)
        {
            Destroy(soundEmitter.gameObject);
        }
    }
}