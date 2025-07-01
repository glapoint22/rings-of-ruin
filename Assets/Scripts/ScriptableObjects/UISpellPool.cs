using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Rings of Ruin/UI Spell Pool")]
public class UISpellPool : MultiPrefabPool
{
    [SerializeField] private List<PrefabEnumMapping<PickupType>> stormboltSpells = new();
    [SerializeField] private List<PrefabEnumMapping<PickupType>> bloodrootSpells = new();
    [SerializeField] private List<PrefabEnumMapping<PickupType>> fireballSpells = new();
    [SerializeField] private List<PrefabEnumMapping<PickupType>> ashbindSpells = new();
    [SerializeField] private List<PrefabEnumMapping<PickupType>> shieldSpells = new();
    [SerializeField] private List<PrefabEnumMapping<PickupType>> cloakSpells = new();
    [SerializeField] private List<PrefabEnumMapping<PickupType>> timeDilationSpells = new();
    

    protected override void ProcessAllMappings()
    {
        ProcessMappingList(stormboltSpells);
        ProcessMappingList(bloodrootSpells);
        ProcessMappingList(fireballSpells);
        ProcessMappingList(ashbindSpells);
        ProcessMappingList(shieldSpells);
        ProcessMappingList(cloakSpells);
        ProcessMappingList(timeDilationSpells);
    }
}