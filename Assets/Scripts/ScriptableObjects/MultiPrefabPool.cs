using UnityEngine;
using System.Collections.Generic;

public abstract class MultiPrefabPool : BasePool
{
    private Dictionary<System.Enum, Queue<GameObject>> pools = new Dictionary<System.Enum, Queue<GameObject>>();
    private Dictionary<System.Enum, GameObject> enumToPrefab = new Dictionary<System.Enum, GameObject>();
    private Dictionary<GameObject, System.Enum> prefabToEnum = new Dictionary<GameObject, System.Enum>();



    protected abstract void ProcessAllMappings();




    public override void Initialize(Transform poolParent)
    {
        base.Initialize(poolParent);
        BuildLookupDictionaries();
    }




    private void BuildLookupDictionaries()
    {
        pools.Clear();
        enumToPrefab.Clear();
        prefabToEnum.Clear();

        // Let derived classes provide their mappings
        ProcessAllMappings();
    }




    // Helper method for derived classes to use
    protected void ProcessMappingList<T>(List<PrefabEnumMapping<T>> mappings) where T : System.Enum
    {
        foreach (var mapping in mappings)
        {
            enumToPrefab[mapping.enumValue] = mapping.prefab;
        }
    }




    public GameObject Get(System.Enum enumType)
    {
        // Ensure a pool exists for this enum type, if not add it
        pools.TryAdd(enumType, new Queue<GameObject>());

        // If there are prefabs in the pool, return the first one
        if (pools[enumType].Count > 0)
        {
            GameObject prefab = pools[enumType].Dequeue();
            prefab.SetActive(true);
            return prefab;
        }

        // If there are no prefabs in the pool, create a new one
        return CreatePrefabInstance(enumType);
    }



    protected virtual GameObject CreatePrefabInstance(System.Enum enumType)
    {
        GameObject prefab = GetPrefabFromEnum(enumType);
        if (prefab == null) return null;

        GameObject prefabInstance = Instantiate(prefab, poolParent);
        prefabInstance.SetActive(true);
        prefabToEnum.TryAdd(prefabInstance, enumType);
        return prefabInstance;
    }




    public override void Return(GameObject prefab)
    {
        // Find which enum type this prefab belongs to
        System.Enum enumType = GetEnumTypeFromPrefab(prefab);
        if (enumType == null) return;

        prefab.SetActive(false);
        pools[enumType].Enqueue(prefab);
    }




    protected virtual GameObject GetPrefabFromEnum(System.Enum enumType)
    {
        enumToPrefab.TryGetValue(enumType, out GameObject prefab);
        return prefab;
    }




    protected virtual System.Enum GetEnumTypeFromPrefab(GameObject prefab)
    {
        prefabToEnum.TryGetValue(prefab, out System.Enum enumType);
        return enumType;
    }



    public override void Clear()
    {
        foreach (var pool in pools.Values)
        {
            while (pool.Count > 0)
            {
                GameObject prefab = pool.Dequeue();
                if (prefab != null)
                {
                    Destroy(prefab);
                }
            }
        }
        pools.Clear();
    }
}




[System.Serializable]
public struct PrefabEnumMapping<T> where T : System.Enum
{
    public GameObject prefab;
    public T enumValue;
}