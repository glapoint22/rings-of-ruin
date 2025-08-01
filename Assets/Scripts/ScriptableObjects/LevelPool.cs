using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Rings of Ruin/Level Pool")]
public class LevelPool : MultiPrefabPool
{
    [SerializeField] private List<PrefabEnumMapping<RingSegmentType>> ring0Segments = new();
    [SerializeField] private List<PrefabEnumMapping<RingSegmentType>> ring1Segments = new();
    [SerializeField] private List<PrefabEnumMapping<RingSegmentType>> ring2Segments = new();
    [SerializeField] private List<PrefabEnumMapping<RingSegmentType>> ring3Segments = new();
    [SerializeField] private List<PrefabEnumMapping<SpawnType>> spawnTypes = new();
    [SerializeField] private List<PrefabEnumMapping<EnemySpawnType>> enemies = new();
    [SerializeField] private List<PrefabEnumMapping<UtilityItem>> utilityItems = new();

    protected override void ProcessAllMappings()
    {
        ProcessMappingList(spawnTypes);
        ProcessMappingList(enemies);
        ProcessMappingList(ring0Segments);
        ProcessMappingList(ring1Segments);
        ProcessMappingList(ring2Segments);
        ProcessMappingList(ring3Segments);
        ProcessMappingList(utilityItems);
    }
}