using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Rings of Ruin/UI Buff Pool")]
public class UIBuffPool : MultiPrefabPool
{
    [SerializeField] private List<PrefabEnumMapping<BuffType>> shieldBuffs = new();
    [SerializeField] private List<PrefabEnumMapping<BuffType>> cloakBuffs = new();
    [SerializeField] private List<PrefabEnumMapping<BuffType>> timeDilationBuffs = new();
    [SerializeField] private List<PrefabEnumMapping<BuffType>> ashbindBuffs = new();

    

    protected override void ProcessAllMappings()
    {
        ProcessMappingList(shieldBuffs);
        ProcessMappingList(cloakBuffs);
        ProcessMappingList(timeDilationBuffs);
        ProcessMappingList(ashbindBuffs);
    }
}