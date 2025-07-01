using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Rings of Ruin/UI Spell Pool")]
public class UISpellPool : MultiPrefabPool
{
    [SerializeField] private List<PrefabEnumMapping<SpellType>> stormboltSpells = new();
    [SerializeField] private List<PrefabEnumMapping<SpellType>> bloodrootSpells = new();
    [SerializeField] private List<PrefabEnumMapping<SpellType>> fireballSpells = new();
    [SerializeField] private List<PrefabEnumMapping<SpellType>> ashbindSpells = new();
    [SerializeField] private List<PrefabEnumMapping<SpellType>> shieldSpells = new();
    [SerializeField] private List<PrefabEnumMapping<SpellType>> cloakSpells = new();
    [SerializeField] private List<PrefabEnumMapping<SpellType>> timeDilationSpells = new();
    

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