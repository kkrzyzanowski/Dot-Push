using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnManager : MonoBehaviour
{
    public Collider fieldCollider;
    public List<SpawnPool> spawnPools = new List<SpawnPool>();
    
    private int uncatched = 0;
    private float allTimeSpawn;

    // Start is called before the first frame update
    void Start()
    {
        uncatched = 0;
        
        foreach (var pool in spawnPools)
        {
            pool.Initialize(this, fieldCollider, transform);
            StartCoroutine(pool.SpawnRoutine());
        }
        
        ConfigurationGame.ConfigurationGameInstance.OnLevelChange += ConfigurationGameInstance_OnLevelChange;
        
        if (spawnPools.Count > 0)
        {
            allTimeSpawn = spawnPools.Sum(p => p.poolSize * p.interval);
        }
    }

    private void ConfigurationGameInstance_OnLevelChange()
    {
        foreach (var pool in spawnPools)
        {
            pool.interval += 0.2f;
        }
        uncatched = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float activeCount = 0;
        foreach (var pool in spawnPools)
        {
            activeCount += pool.instances.Count(s => s.gameObject.activeSelf);
        }
        
        ConfigurationGame.ConfigurationGameInstance.CheckGameOver(activeCount, (float)allTimeSpawn);
    }
}
