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
    List<Transform> spawners;
    int entityCount;
    int uncatched;
    // Start is called before the first frame update
    void Start()
    {
        uncatched = 0;
        spawnTime = 0.0f;
        interval = 1.0f;
        wait = false;
        spawners = new List<Transform>();
        ConfigurationGame.ConfigurationGameInstance.OnLevelChange += ConfigurationGameInstance_OnLevelChange;
        entityCount = (int)(allTimeSpawn / interval);
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
                if(spawners[0] != null)
                {
                    Destroy(spawners[0].gameObject);
                }
                spawners.RemoveAt(0);
                uncatched++;
            }
            ConfigurationGame.ConfigurationGameInstance.CheckGameOver((float)uncatched, (float)allTimeSpawn);
            wait = true;
            Debug.Log("Time: " + spawnTime);
            yield return new WaitForSeconds(interval);
            spawnTime += interval;
            xSpawn = Random.Range(fieldCollider.bounds.min.x, fieldCollider.bounds.max.x);
            ySpawn = Random.Range(fieldCollider.bounds.min.y, fieldCollider.bounds.max.y);
            spawners.Add(Instantiate(spawner.transform, new Vector3(xSpawn, ySpawn, fieldCollider.transform.position.z),
            Quaternion.Euler(Vector3.zero)));
            wait = false;
        }
        else
        {
            spawnTime = 0.0f;
        }
    }

}
