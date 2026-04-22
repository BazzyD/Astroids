using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }
    [SerializeField] private List<PoolData> poolBlueprints;
    
    private Dictionary<string, PoolData> _poolDictionary = new();
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        foreach (var blueprint in poolBlueprints)
        {
            _poolDictionary.Add(blueprint.tag, blueprint);
            
            // Pre-fill the pools
            for (int i = 0; i < blueprint.size; i++)
            {
                CreateNewObjectInPool(blueprint);
            }
        }
    }
    private void CreateNewObjectInPool(PoolData pool)
    {
        GameObject obj = Instantiate(pool.prefab, transform);
        obj.SetActive(false);
        pool.inactiveObjects.Enqueue(obj);
    }

    public GameObject Spawn(string tag, Vector3 position, Quaternion rotation)
    {
        if (!_poolDictionary.TryGetValue(tag, out PoolData pool))
        {
            Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
            return null;
        }

        // Handle empty pool (Dynamic growth)
        if (pool.inactiveObjects.Count == 0)
        {
            CreateNewObjectInPool(pool);
        }

        GameObject spawnedObj = pool.inactiveObjects.Dequeue();

        spawnedObj.transform.SetPositionAndRotation(position, rotation);
        spawnedObj.SetActive(true);

        if (spawnedObj.TryGetComponent(out IPoolable poolable))
        {
            poolable.OnSpawn();
        }
        
        return spawnedObj;
    }
    public void Despawn(string tag, GameObject obj)
    {
        if (!_poolDictionary.ContainsKey(tag)) return;

        if (obj.TryGetComponent(out IPoolable poolable))
        {
            poolable.OnDespawn();
        }

        obj.SetActive(false);
        _poolDictionary[tag].inactiveObjects.Enqueue(obj);
    }
}
