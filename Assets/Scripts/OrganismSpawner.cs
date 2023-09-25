using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class OrganismSpawner : MonoBehaviour
{
    [SerializeField] private float spawnRadius;
    [SerializeField] private int spawnCount;
    [SerializeField] private Organism[] organisms;

    [Serializable]
    private struct Organism
    {
        public GameObject prefab;
        public int weight;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 position = Random.insideUnitCircle * spawnRadius;
            Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
            Instantiate(SelectEnemyPrefabFromLootTable(organisms), position, rotation);
        }
    }
    
    GameObject SelectEnemyPrefabFromLootTable(Organism[] table) {
        if (table.Length == 0) return null;
        
        Organism v;
        int totalWeight = 0;
        for (int i = 0; i < table.Length; i++) {
            v = table[i];
            totalWeight += v.weight;
        }

        int choice = 0;
        int randomNumber = (int)Math.Floor(Random.value * totalWeight + 1);
        int weight = 0;
        for(int i = 0; i < table.Length; i++) {
            v = table[i];

            weight += v.weight;
            if (randomNumber <= weight) {
                choice = i;
                break;
            }
        }

        var chosenItem = table[choice];

        return chosenItem.prefab;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
