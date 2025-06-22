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
    [SerializeField] protected Interactables portals = new Interactables();

    protected Dictionary<System.Enum, Queue<GameObject>> pools = new Dictionary<System.Enum, Queue<GameObject>>();

    // Optimized dictionaries for fast lookups
    protected Dictionary<System.Enum, GameObject> enumToPrefab = new Dictionary<System.Enum, GameObject>();
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

        // Build from Portals
        foreach (var mapping in portals.interactableMappings)
        {
            enumToPrefab[mapping.enumValue] = mapping.prefab;
            typeToEnum[mapping.prefab.GetType()] = mapping.enumValue;
        }

    }

    public GameObject Get(System.Enum enumType)
    {
        if (!pools.ContainsKey(enumType))
        {
            pools[enumType] = new Queue<GameObject>();
        }

        if (pools[enumType].Count > 0)
        {
            GameObject instance = pools[enumType].Dequeue();
            instance.SetActive(true);
            return instance;
        }

        // Create new instance of the requested type
        return CreateNewInstance(enumType);
    }

    public override void Return(GameObject instance)
    {
        // Find which enum type this instance belongs to
        System.Enum enumType = GetEnumTypeForInstance(instance);

        if (enumType != null)
        {
            instance.SetActive(false);
            pools[enumType].Enqueue(instance);
        }
    }

    public override void Clear()
    {
        foreach (var pool in pools.Values)
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
        pools.Clear();
    }

    protected virtual GameObject CreateNewInstance(System.Enum enumType)
    {
        GameObject prefab = GetPrefabForEnum(enumType);
        if (prefab != null)
        {
            GameObject instance = Instantiate(prefab, poolParent);
            instance.SetActive(true);
            return instance;
        }

        return null;
    }

    protected virtual GameObject GetPrefabForEnum(System.Enum enumType)
    {
        enumToPrefab.TryGetValue(enumType, out GameObject prefab);
        return prefab;
    }

    protected virtual System.Enum GetEnumTypeForInstance(GameObject instance)
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
    public List<PrefabEnumMapping<RingSegmentType>> ring1SegmentMappings = new List<PrefabEnumMapping<RingSegmentType>>();
}

[System.Serializable]
public class Ring2Segments
{
    public List<PrefabEnumMapping<RingSegmentType>> ring2SegmentMappings = new List<PrefabEnumMapping<RingSegmentType>>();
}


[System.Serializable]
public class Ring3Segments
{
    public List<PrefabEnumMapping<RingSegmentType>> ring3SegmentMappings = new List<PrefabEnumMapping<RingSegmentType>>();
}


[System.Serializable]
public class Ring4Segments
{
    public List<PrefabEnumMapping<RingSegmentType>> ring4SegmentMappings = new List<PrefabEnumMapping<RingSegmentType>>();
}


[System.Serializable]
public class Interactables
{
    public List<PrefabEnumMapping<InteractableType>> interactableMappings = new List<PrefabEnumMapping<InteractableType>>();
}




[System.Serializable]
public class PrefabEnumMapping<T> where T : System.Enum
{
    public GameObject prefab;
    public T enumValue;
}


public enum RingSegmentType
{
    Ring_1_Normal,
    Ring_1_Gap,
    Ring_1_Crumbling,
    Ring_1_Spike,
    Ring_2_Normal,
    Ring_2_Gap,
    Ring_2_Crumbling,
    Ring_2_Spike,
    Ring_3_Normal,
    Ring_3_Gap,
    Ring_3_Crumbling,
    Ring_3_Spike,
    Ring_4_Normal,
    Ring_4_Gap,
    Ring_4_Crumbling,
    Ring_4_Spike
}