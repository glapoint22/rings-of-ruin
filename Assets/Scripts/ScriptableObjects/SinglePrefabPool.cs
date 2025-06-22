using UnityEngine;
using System.Collections.Generic;

public abstract class SinglePrefabPool : BasePool
{
    [SerializeField] protected MonoBehaviour prefab;
    
    protected Queue<MonoBehaviour> pool = new Queue<MonoBehaviour>();

    public MonoBehaviour Get()
    {
        if (pool.Count > 0)
        {
            MonoBehaviour instance = pool.Dequeue();
            instance.gameObject.SetActive(true);
            return instance;
        }
        
        // Create new instance if pool is empty
        return CreateNewInstance();
    }

    public override void Return(MonoBehaviour instance)
    {
        instance.gameObject.SetActive(false);
        pool.Enqueue(instance);
    }

    public override void Clear()
    {
        while (pool.Count > 0)
        {
            MonoBehaviour instance = pool.Dequeue();
            if (instance != null)
            {
                Destroy(instance.gameObject);
            }
        }
    }

    protected virtual MonoBehaviour CreateNewInstance()
    {
        MonoBehaviour instance = Instantiate(prefab, poolParent);
        instance.gameObject.SetActive(false);
        return instance; // Don't add to pool - it will be returned when done
    }
}