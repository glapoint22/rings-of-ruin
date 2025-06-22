using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(menuName = "Rings of Ruin/Multi Prefab Pool")]
public class MultiPrefabPool : BasePool
{
    [SerializeField] protected Collectibles collectibles = new Collectibles();
    [SerializeField] protected Pickups pickups = new Pickups();
    [SerializeField] protected Enemies enemies = new Enemies();
    [SerializeField] protected Ring1Segments ring1Segments = new Ring1Segments();
    [SerializeField] protected Ring2Segments ring2Segments = new Ring2Segments();
    [SerializeField] protected Ring3Segments ring3Segments = new Ring3Segments();
    [SerializeField] protected Ring4Segments ring4Segments = new Ring4Segments();
    
    protected Dictionary<System.Enum, Queue<MonoBehaviour>> pools = new Dictionary<System.Enum, Queue<MonoBehaviour>>();
    
    // Optimized dictionaries for fast lookups
    protected Dictionary<System.Enum, MonoBehaviour> enumToPrefab = new Dictionary<System.Enum, MonoBehaviour>();
    protected Dictionary<System.Type, System.Enum> typeToEnum = new Dictionary<System.Type, System.Enum>();

    public override void Initialize(Transform poolParent)
    {
        base.Initialize(poolParent);
        BuildLookupDictionaries();
    }

    private void BuildLookupDictionaries()
    {
        enumToPrefab.Clear();
        typeToEnum.Clear();
        
        // Build from CollectibleGroup
        foreach (var mapping in collectibles.collectibleMappings)
        {
            enumToPrefab[mapping.enumValue] = mapping.prefab;
            typeToEnum[mapping.prefab.GetType()] = mapping.enumValue;
        }
        
        // Build from PickupGroup
        foreach (var mapping in pickups.pickupMappings)
        {
            enumToPrefab[mapping.enumValue] = mapping.prefab;
            typeToEnum[mapping.prefab.GetType()] = mapping.enumValue;
        }

        // Build from EnemyGroup
        foreach (var mapping in enemies.enemyMappings)
        {
            enumToPrefab[mapping.enumValue] = mapping.prefab;
            typeToEnum[mapping.prefab.GetType()] = mapping.enumValue;
        }

        // Build from Ring1Segments
        foreach (var mapping in ring1Segments.ring1SegmentMappings)
        {
            enumToPrefab[mapping.enumValue] = mapping.prefab;
            typeToEnum[mapping.prefab.GetType()] = mapping.enumValue;
        }

        // Build from Ring2Segments
        foreach (var mapping in ring2Segments.ring2SegmentMappings)
        {
            enumToPrefab[mapping.enumValue] = mapping.prefab;
            typeToEnum[mapping.prefab.GetType()] = mapping.enumValue;
        }

        // Build from Ring3Segments
        foreach (var mapping in ring3Segments.ring3SegmentMappings)
        {
            enumToPrefab[mapping.enumValue] = mapping.prefab;
            typeToEnum[mapping.prefab.GetType()] = mapping.enumValue;
        }

        // Build from Ring4Segments
        foreach (var mapping in ring4Segments.ring4SegmentMappings)
        {
            enumToPrefab[mapping.enumValue] = mapping.prefab;
            typeToEnum[mapping.prefab.GetType()] = mapping.enumValue;
        }

    }

    public MonoBehaviour Get(System.Enum enumType)
    {
        if (!pools.ContainsKey(enumType))
        {
            pools[enumType] = new Queue<MonoBehaviour>();
        }

        if (pools[enumType].Count > 0)
        {
            MonoBehaviour instance = pools[enumType].Dequeue();
            instance.gameObject.SetActive(true);
            return instance;
        }

        // Create new instance of the requested type
        return CreateNewInstance(enumType);
    }

    public override void Return(MonoBehaviour instance)
    {
        // Find which enum type this instance belongs to
        System.Enum enumType = GetEnumTypeForInstance(instance);

        if (enumType != null)
        {
            instance.gameObject.SetActive(false);
            pools[enumType].Enqueue(instance);
        }
    }

    public override void Clear()
    {
        foreach (var pool in pools.Values)
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
        pools.Clear();
    }

    protected virtual MonoBehaviour CreateNewInstance(System.Enum enumType)
    {
        MonoBehaviour prefab = GetPrefabForEnum(enumType);
        if (prefab != null)
        {
            MonoBehaviour instance = Instantiate(prefab, poolParent);
            instance.gameObject.SetActive(true);
            return instance;
        }

        return null;
    }

    protected virtual MonoBehaviour GetPrefabForEnum(System.Enum enumType)
    {
        enumToPrefab.TryGetValue(enumType, out MonoBehaviour prefab);
        return prefab;
    }

    protected virtual System.Enum GetEnumTypeForInstance(MonoBehaviour instance)
    {
        typeToEnum.TryGetValue(instance.GetType(), out System.Enum enumType);
        return enumType;
    }
}

[System.Serializable]
public class Collectibles
{
    public List<PrefabEnumMapping<CollectibleType>> collectibleMappings = new List<PrefabEnumMapping<CollectibleType>>();
}

[System.Serializable]
public class Pickups
{
    public List<PrefabEnumMapping<PickupType>> pickupMappings = new List<PrefabEnumMapping<PickupType>>();
}

[System.Serializable]
public class Enemies
{
    public List<PrefabEnumMapping<EnemyType>> enemyMappings = new List<PrefabEnumMapping<EnemyType>>();
}

[System.Serializable]
public class Ring1Segments
{
    public List<PrefabEnumMapping<SegmentType>> ring1SegmentMappings = new List<PrefabEnumMapping<SegmentType>>();
}

[System.Serializable]
public class Ring2Segments
{
    public List<PrefabEnumMapping<SegmentType>> ring2SegmentMappings = new List<PrefabEnumMapping<SegmentType>>();
}


[System.Serializable]
public class Ring3Segments
{
    public List<PrefabEnumMapping<SegmentType>> ring3SegmentMappings = new List<PrefabEnumMapping<SegmentType>>();
}


[System.Serializable]
public class Ring4Segments
{
    public List<PrefabEnumMapping<SegmentType>> ring4SegmentMappings = new List<PrefabEnumMapping<SegmentType>>();
}



[System.Serializable]
public class PrefabEnumMapping<T> where T : System.Enum
{
    public MonoBehaviour prefab;
    public T enumValue;
}