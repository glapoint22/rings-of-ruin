using UnityEngine;

[CreateAssetMenu(menuName = "Rings of Ruin/Hud Icon Library")]
public class HudIconLibrary : ScriptableObject
{
    public HudIconEntry[] hudIcons;

    public Sprite GetIcon(PickupType type)
    {
        foreach (var entry in hudIcons)
        {
            if (entry.buffType == type)
                return entry.icon;
        }
        return null;
    }
}