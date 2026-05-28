using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class SpawnPool
{
    public Spawner prefab;
    public int poolSize = 10;
    public float interval = 1f;
    public bool preSpawn = true;

    [HideInInspector]
    public List<Spawner> instances = new List<Spawner>();

    private Collider fieldCollider;
    private MonoBehaviour coroutineOwner;
    private Transform parentTransform;

    public void Initialize(MonoBehaviour owner, Collider spawnArea, Transform parent = null)
    {
        coroutineOwner = owner;
        fieldCollider = spawnArea;
        parentTransform = parent;
        CreatePool();
        if (preSpawn) PreSpawn();
    }

    private void CreatePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            Vector3 spawnPos = GetRandomPosition();
            var instance = Object.Instantiate(prefab, spawnPos, Quaternion.identity, parentTransform);
            instance.gameObject.SetActive(false);
            instances.Add(instance);
        }
    }

    public void PreSpawn()
    {
        foreach (var instance in instances)
        {
            instance.transform.position = GetRandomPosition();
            instance.gameObject.SetActive(false);
        }
    }

    public IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            SpawnNext();
        }
    }

    private void SpawnNext()
    {
        var next = instances.FirstOrDefault(s => !s.gameObject.activeSelf);
        if (next == null) return;
        
        next.transform.position = GetRandomPosition();
        next.gameObject.SetActive(true);
    }

    private Vector3 GetRandomPosition()
    {
        return new Vector3(
            Random.Range(fieldCollider.bounds.min.x, fieldCollider.bounds.max.x),
            Random.Range(fieldCollider.bounds.min.y, fieldCollider.bounds.max.y),
            fieldCollider.transform.position.z
        );
    }
}
