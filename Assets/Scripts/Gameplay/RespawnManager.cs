using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Game.Core;
namespace Game.Gameplay
{
    public class RespawnManager : MonoSingelton<RespawnManager>
    {
        public UnityEvent<int> OnRespawnCountSet;
        private int spawnCount = 0;
        private Respawner[] spawners;

        public int SpawnCount { get => spawnCount; }

        public void IncreaseSpawnCount()
        {
            spawnCount++;
            OnRespawnCountSet?.Invoke(spawnCount);
        }

        public void DecreaseSpawnCount()
        {
            spawnCount--;
            OnRespawnCountSet?.Invoke(spawnCount);
        }

        public Respawner FindClosestSpawner(Vector3 position)
        {
            if(spawners == null || spawners.Length == 0)
            {
                return null;
            }

            float minSqrDistance = float.MaxValue;
            Respawner closest = null;
            for(int i = 0; i < spawners.Length; i++)
            {
                Respawner spawner = spawners[i];
                float sqrDistance = Vector3.SqrMagnitude(position - spawner.transform.position);
                if(sqrDistance < minSqrDistance)
                {
                    closest = spawner;
                    minSqrDistance = sqrDistance;
                }
            }

            return closest;
        }

        protected override void InternalInit()
        {
            spawnCount = 0;
        }

        protected override void InternalOnStart()
        {
            spawners = FindObjectsByType<Respawner>(FindObjectsSortMode.InstanceID);
            for(int i = 0; i < spawners.Length; i++)
            {
                spawners[i].gameObject.SetActive(true);
                spawners[i].enabled = true;
            }
            OnRespawnCountSet?.Invoke(spawnCount);
        }

        protected override void InternalOnDestroy()
        {
            
        }
    }
}
