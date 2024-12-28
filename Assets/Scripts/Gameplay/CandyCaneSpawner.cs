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
        
        [Range(0.1f, 1.0f)]
        [SerializeField]private float minAcceptSpawnPointProb = 0.5f;
        
        [SerializeField]private bool generateCandies = false;
        private CandyCane[] candyCanePrefabs;

        public bool GenerateCandies { get => generateCandies; }

        private void SpawnRandomCandies()
        {
            if(spawnPoints == null || spawnPoints.Length == 0)
            {
                return;
            }
            foreach(Transform spawnPoint in spawnPoints)
            {
                float randomProb = Random.value;
                if(randomProb >= (1.0f - minAcceptSpawnPointProb) )
                {
                    int randomIndex = Random.Range(0, candyCanePrefabs.Length);
                    CandyCane candyCane = Instantiate(candyCanePrefabs[randomIndex], spawnPoint.position, Quaternion.identity);   
                    candyCane.transform.parent = transform;
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
            if(normalCandyCanePrefab == null || movingCandyCanePrefab == null || respawnCandyCanePrefab == null)
            {
                return;
            }
            candyCanePrefabs[0] = normalCandyCanePrefab;
            candyCanePrefabs[1] = movingCandyCanePrefab;
            candyCanePrefabs[2] = respawnCandyCanePrefab;

            if(generateCandies)
            {
                SpawnRandomCandies();
            }
        }
    }
}
