using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [Header("오브젝트 풀 설정")]
    public List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> objectPools;

    protected override void Awake()
    {
        base.Awake();

        objectPools = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectQueue = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.transform.SetParent(this.transform); 
                obj.SetActive(false);
                objectQueue.Enqueue(obj);
            }
            objectPools.Add(pool.tag, objectQueue);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!objectPools.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        if (objectPools[tag].Count == 0)
        {
            Pool pool = pools.Find(p => p.tag == tag);
            if (pool != null)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.transform.SetParent(this.transform);
            }
        }

        GameObject objectToSpawn = objectPools[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        return objectToSpawn;
    }

    public void ReturnToPool(string tag, GameObject objectToReturn)
    {
        if (!objectPools.ContainsKey(tag))
        {
            Destroy(objectToReturn); 
            return;
        }

        objectToReturn.SetActive(false);
        objectPools[tag].Enqueue(objectToReturn);
    }
}