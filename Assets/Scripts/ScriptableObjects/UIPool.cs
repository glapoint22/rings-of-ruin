using UnityEngine;

[CreateAssetMenu(menuName = "Rings of Ruin/UI Pool")]
public class UIPool : SinglePrefabPool          
{
    [SerializeField] private UIIconLibrary iconLibrary;

    public UIIcon GetWithIcon(PickupType pickupType)
    {
        GameObject icon = Get();
        if (icon != null)
        {
            icon.GetComponent<UIIcon>().SetIcon(iconLibrary.GetIcon(pickupType));
        }
        return icon.GetComponent<UIIcon>();
    }
}