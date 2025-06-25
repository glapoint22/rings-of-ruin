using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Rings of Ruin/UI Buff Pool")]
public class UIBuffPool : MultiPrefabPool
{
    [SerializeField] private List<PrefabEnumMapping<PickupType>> shieldBuffs = new();
    [SerializeField] private List<PrefabEnumMapping<PickupType>> cloakBuffs = new();
    [SerializeField] private List<PrefabEnumMapping<PickupType>> timeDilationBuffs = new();
    [SerializeField] private List<PrefabEnumMapping<PickupType>> keyBuffs = new();
    

    protected override void ProcessAllMappings()
    {
        ProcessMappingList(shieldBuffs);
        ProcessMappingList(cloakBuffs);
        ProcessMappingList(timeDilationBuffs);
        ProcessMappingList(keyBuffs);
    }
}