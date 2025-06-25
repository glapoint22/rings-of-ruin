using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Rings of Ruin/UI Spell Pool")]
public class UISpellPool : MultiPrefabPool
{
    [SerializeField] private List<PrefabEnumMapping<PickupType>> stormboltSpells = new();
    [SerializeField] private List<PrefabEnumMapping<PickupType>> bloodrootSpells = new();
    [SerializeField] private List<PrefabEnumMapping<PickupType>> fireballSpells = new();
    

    protected override void ProcessAllMappings()
    {
        ProcessMappingList(stormboltSpells);
        ProcessMappingList(bloodrootSpells);
        ProcessMappingList(fireballSpells);
    }
}