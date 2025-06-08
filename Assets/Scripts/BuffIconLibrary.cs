using UnityEngine;

[CreateAssetMenu(menuName = "Rings of Ruin/Buff Icon Library")]
public class BuffIconLibrary : ScriptableObject
{
    public BuffIconEntry[] buffIcons;

    public Sprite GetIcon(PickupType type)
    {
        foreach (var entry in buffIcons)
        {
            if (entry.buffType == type)
                return entry.icon;
        }
        return null;
    }
}