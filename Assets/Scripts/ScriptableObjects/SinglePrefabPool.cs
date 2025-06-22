using UnityEngine;
using System.Collections.Generic;

public abstract class SinglePrefabPool : BasePool
{
    [SerializeField] protected GameObject prefab;
    
    protected Queue<GameObject> pool = new Queue<GameObject>();

    public GameObject Get()
    {
        if (pool.Count > 0)
        {
            GameObject instance = pool.Dequeue();
            instance.SetActive(true);
            return instance;
        }
        
        // Create new instance if pool is empty
        return CreateNewInstance();
    }

    public override void Return(GameObject instance)
    {
        instance.SetActive(false);
        pool.Enqueue(instance);
    }

    public override void Clear()
    {
        while (pool.Count > 0)
        {
            GameObject instance = pool.Dequeue();
            if (instance != null)
            {
                Destroy(instance);
            }
        }
    }

    protected virtual GameObject CreateNewInstance()
    {
        GameObject instance = Instantiate(prefab, poolParent);
        // instance.SetActive(true);
        return instance; // Don't add to pool - it will be returned when done
    }
}