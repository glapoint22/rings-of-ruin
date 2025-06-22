using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(menuName = "Rings of Ruin/Multi Prefab Pool")]
public class MultiPrefabPool : BasePool
{
    [SerializeField] private List<PrefabEnumMapping<RingSegmentType>> ring1Segments = new();
    [SerializeField] private List<PrefabEnumMapping<RingSegmentType>> ring2Segments = new();
    [SerializeField] private List<PrefabEnumMapping<RingSegmentType>> ring3Segments = new();
    [SerializeField] private List<PrefabEnumMapping<RingSegmentType>> ring4Segments = new();
    [SerializeField] private List<PrefabEnumMapping<CollectibleType>> collectibles = new();
    [SerializeField] private List<PrefabEnumMapping<PickupType>> pickups = new();
    [SerializeField] private List<PrefabEnumMapping<InteractableType>> interactables = new();
    [SerializeField] private List<PrefabEnumMapping<EnemyType>> enemies = new();
    
    

    private Dictionary<System.Enum, Queue<GameObject>> pools = new Dictionary<System.Enum, Queue<GameObject>>();
    private Dictionary<System.Enum, GameObject> enumToPrefab = new Dictionary<System.Enum, GameObject>();
    private Dictionary<System.Type, System.Enum> typeToEnum = new Dictionary<System.Type, System.Enum>();



    public override void Initialize(Transform poolParent)
    {
        base.Initialize(poolParent);
        BuildLookupDictionaries();
    }




    private void BuildLookupDictionaries()
    {
        enumToPrefab.Clear();
        typeToEnum.Clear();
        pools.Clear();

        // Use a generic helper method to process all mapping lists
        ProcessMappingList(collectibles);
        ProcessMappingList(pickups);
        ProcessMappingList(enemies);
        ProcessMappingList(ring1Segments);
        ProcessMappingList(ring2Segments);
        ProcessMappingList(ring3Segments);
        ProcessMappingList(ring4Segments);
        ProcessMappingList(interactables);
    }





    private void ProcessMappingList<T>(List<PrefabEnumMapping<T>> mappings) where T : System.Enum
    {
        foreach (var mapping in mappings)
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
public struct PrefabEnumMapping<T> where T : System.Enum
{
    public GameObject prefab;
    public T enumValue;
}