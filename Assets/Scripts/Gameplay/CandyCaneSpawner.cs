using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay
{
    public class CandyCaneSpawner : MonoBehaviour
    {
        [SerializeField]private CandyCane normalCandyCanePrefab;
        [SerializeField]private CandyCane movingCandyCanePrefab;
        [SerializeField]private CandyCane respawnCandyCanePrefab;
        
        [SerializeField]private Transform[] spawnPoints;
        
        [Min(0.1f)]
        [SerializeField]private float acceptSpawnPointProb = 0.5f;

        private CandyCane[] candyCanePrefabs;
        private void SpawnBasedOnWeight()
        {
            int totalWeight = 0;
            foreach(CandyCane candyCane in candyCanePrefabs)
            {
                totalWeight += candyCane.Data.weight;
            }

            int randomWeight = Random.Range(0, totalWeight);
            int cummulativeWeight = 0;

            foreach(Transform spawnPoint in spawnPoints)
            {
                float randomProb = Random.value;
                if(randomProb >= (1.0f - acceptSpawnPointProb))
                {
                    foreach(CandyCane candyCanePrefab in candyCanePrefabs)
                    {
                        var data = candyCanePrefab.Data;
                        cummulativeWeight += data.weight;
                        if(cummulativeWeight <= randomWeight)
                        {
                            CandyCane candyCane = Instantiate(candyCanePrefab, spawnPoint.position, Quaternion.identity);
                            break;
                        }
                    }
                }
            }
            
        }

        private void Awake() 
        {
            candyCanePrefabs = new CandyCane[3];
            
        }
        // Start is called before the first frame update
        void Start()
        {
            candyCanePrefabs[0] = normalCandyCanePrefab;
            candyCanePrefabs[1] = movingCandyCanePrefab;
            candyCanePrefabs[2] = respawnCandyCanePrefab;
            SpawnBasedOnWeight();
        }
    }
}
