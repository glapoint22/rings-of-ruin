using UnityEngine;

[CreateAssetMenu(menuName = "Rings of Ruin/UI Pool")]
public class UIPool : SinglePrefabPool          
{
    [SerializeField] private UIIconLibrary iconLibrary;

    public UIIcon GetWithIcon(PickupType pickupType)
    {
        UIIcon icon = Get() as UIIcon;
        if (icon != null)
        {
            icon.SetIcon(iconLibrary.GetIcon(pickupType));
        }
        return icon;
    }
}