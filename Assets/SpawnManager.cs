using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnManager : MonoBehaviour
{
    public Collider fieldCollider;
    public List<SpawnPool> spawnPools = new List<SpawnPool>();
    public float timeLineSpawn = 10.0f;
    
    private LineDrawer lineDrawer;
    private int uncatched = 0;
    private float allTimeSpawn;
    // Start is called before the first frame update
    void Start()
    {
        lineDrawer = GetComponent<LineDrawer>();
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

        StartCoroutine(AddLinesToDots());
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

    IEnumerator AddLinesToDots()
    {
        while (true)
        {
            ConfigurationGame.ConfigurationGameInstance.ClearCombo();
            var dots = spawnPools[0].instances.Where(s => s.gameObject.activeSelf);
            if (dots.Count() >= 2)
            {
                var comboDots = dots.Take(2).ToArray();
                lineDrawer.AddPoints(comboDots.Select(s => s.transform.position).ToArray());
                ConfigurationGame.ConfigurationGameInstance.AddDotsToCombo(comboDots);
                lineDrawer.DrawLines();

            }
            yield return new WaitForSeconds(timeLineSpawn);
            lineDrawer.ClearLines();
        }
    }
}
