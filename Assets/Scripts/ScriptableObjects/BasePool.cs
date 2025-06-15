using UnityEngine;
using System.Collections.Generic;

public abstract class BasePool<T> : ScriptableObject where T : MonoBehaviour
{
    [SerializeField] protected T prefab;
    [SerializeField] protected int initialPoolSize = 10;

    protected Queue<T> pool = new Queue<T>();

    private Transform poolParent;

    public virtual void Initialize(Transform poolParent)
    {
        this.poolParent = poolParent;
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateNewInstance();
        }
    }

    protected virtual T CreateNewInstance()
    {
        T instance = Instantiate(prefab, poolParent);
        instance.gameObject.SetActive(false);
        pool.Enqueue(instance);
        return instance;
    }

    public virtual T Get()
    {
        if (pool.Count > 0)
        {
            T instance = pool.Dequeue();
            instance.gameObject.SetActive(true);
            return instance;
        }
        
        return null;
    }

    public virtual void Return(T instance)
    {
        instance.gameObject.SetActive(false);
        pool.Enqueue(instance);
    }

    public virtual void Clear()
    {
        while (pool.Count > 0)
        {
            T instance = pool.Dequeue();
            if (instance != null)
            {
                Destroy(instance.gameObject);
            }
        }
    }
}