using UnityEngine;

[CreateAssetMenu(menuName = "Rings of Ruin/UI Icon Library")]
public class UIIconLibrary : ScriptableObject
{
    public UIIconImage[] UIIcons;

    public Sprite GetIcon(PickupType type)
    {
        foreach (var entry in UIIcons)
        {
            if (entry.pickupType == type)
                return entry.icon;
        }
        return null;
    }
}