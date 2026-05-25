using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetInstantiate : MonoBehaviour
{
    public Collider fieldCollider;
    public GameObject spawner;
    public float allTimeSpawn;
    float spawnTime;
    float interval;
    bool wait;
    float xSpawn;
    float ySpawn;
    LinkedList<Transform> spawners;
    int entityCount;
    int uncatched;
    // Start is called before the first frame update
    void Start()
    {
        uncatched = 0;
        spawnTime = 0.0f;
        interval = 1.0f;
        wait = false;
        spawners = new LinkedList<Transform>();
        ConfigurationGame.ConfigurationGameInstance.OnLevelChange += ConfigurationGameInstance_OnLevelChange;
        entityCount = (int)(allTimeSpawn / interval);
        PreSpawn();
    }

    private void PreSpawn()
    {
        for (int i = 0; i < entityCount; i++)
        {
            xSpawn = Random.Range(fieldCollider.bounds.min.x, fieldCollider.bounds.max.x);
            ySpawn = Random.Range(fieldCollider.bounds.min.y, fieldCollider.bounds.max.y);
            var newSpawner = Instantiate(spawner, new Vector3(xSpawn, ySpawn, fieldCollider.transform.position.z), Quaternion.identity);
            newSpawner.SetActive(false);
            spawners.AddLast(newSpawner.transform);
        }
        spawners.First.Value.gameObject.SetActive(true);
    }
    private void ConfigurationGameInstance_OnLevelChange()
    {
        interval += 0.2f;
        entityCount = (int)(allTimeSpawn / interval);
        uncatched = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (wait == false)
        {
            StartCoroutine("Spawn");
        }
    }

    IEnumerator Spawn()
    {
        if (spawnTime <= allTimeSpawn)
        {
            if (spawners.Count >= entityCount)
            {
                var spawner = spawners.First.Value;
                if(spawner != null)
                {
                    spawner.gameObject.SetActive(false);
                }
                spawners.RemoveFirst();
                xSpawn = Random.Range(fieldCollider.bounds.min.x, fieldCollider.bounds.max.x);
                ySpawn = Random.Range(fieldCollider.bounds.min.y, fieldCollider.bounds.max.y);
                spawner.position = new Vector3(xSpawn, ySpawn, fieldCollider.transform.position.z);
                spawner.gameObject.SetActive(true);
                spawners.AddLast(spawner);
                uncatched++;
            }
            ConfigurationGame.ConfigurationGameInstance.CheckGameOver((float)uncatched, (float)allTimeSpawn);
            wait = true;
            Debug.Log("Time: " + spawnTime);
            yield return new WaitForSeconds(interval);
            spawnTime += interval;
            wait = false;
        }
        else
        {
            spawnTime = 0.0f;
        }
    }

}
