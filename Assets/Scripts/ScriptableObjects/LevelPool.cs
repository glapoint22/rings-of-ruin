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
    [SerializeField] private List<PrefabEnumMapping<SpellType>> spells = new();
    [SerializeField] private List<PrefabEnumMapping<PortalType>> portals = new();
    [SerializeField] private List<PrefabEnumMapping<EnemyType>> enemies = new();
    [SerializeField] private List<PrefabEnumMapping<UtilityItem>> utilityItems = new();

    protected override void ProcessAllMappings()
    {
        ProcessMappingList(collectibles);
        ProcessMappingList(spells);
        ProcessMappingList(enemies);
        ProcessMappingList(ring1Segments);
        ProcessMappingList(ring2Segments);
        ProcessMappingList(ring3Segments);
        ProcessMappingList(ring4Segments);
        ProcessMappingList(portals);
        ProcessMappingList(utilityItems);
    }
}