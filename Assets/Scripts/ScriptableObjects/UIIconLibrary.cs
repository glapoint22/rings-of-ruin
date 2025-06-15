using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Rings of Ruin/UI Icon Library")]
public class UIIconLibrary : ScriptableObject
{
    [SerializeField] private UIIconImage[] UIIcons;
    private Dictionary<PickupType, Sprite> iconDictionary;

    private void OnEnable()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        iconDictionary = new Dictionary<PickupType, Sprite>();
        foreach (var entry in UIIcons)
        {
            iconDictionary[entry.pickupType] = entry.icon;
        }
    }

    public Sprite GetIcon(PickupType type)
    {
        if (iconDictionary == null)
        {
            InitializeDictionary();
        }
        
        return iconDictionary.TryGetValue(type, out Sprite icon) ? icon : null;
    }

    public int GetIconCount()
    {
        return UIIcons.Length;
    }
}