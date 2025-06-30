using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Rings of Ruin/Level Pool")]
public class LevelPool : MultiPrefabPool
{
    [SerializeField] private List<PrefabEnumMapping<RingSegmentType>> ring1Segments = new();
    [SerializeField] private List<PrefabEnumMapping<RingSegmentType>> ring2Segments = new();
    [SerializeField] private List<PrefabEnumMapping<RingSegmentType>> ring3Segments = new();
    [SerializeField] private List<PrefabEnumMapping<RingSegmentType>> ring4Segments = new();
    [SerializeField] private List<PrefabEnumMapping<CollectibleType>> collectibles = new();
    [SerializeField] private List<PrefabEnumMapping<PickupType>> pickups = new();
    [SerializeField] private List<PrefabEnumMapping<PortalType>> interactables = new();
    [SerializeField] private List<PrefabEnumMapping<EnemyType>> enemies = new();

    protected override void ProcessAllMappings()
    {
        ProcessMappingList(collectibles);
        ProcessMappingList(pickups);
        ProcessMappingList(enemies);
        ProcessMappingList(ring1Segments);
        ProcessMappingList(ring2Segments);
        ProcessMappingList(ring3Segments);
        ProcessMappingList(ring4Segments);
        ProcessMappingList(interactables);
    }
}