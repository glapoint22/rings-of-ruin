using UnityEngine;

[CreateAssetMenu(menuName = "Rings of Ruin/UI Pool")]
public class UIPool : BasePool<UIIcon>
{
    [SerializeField] private UIIconLibrary iconLibrary;

    public override void Initialize(Transform poolParent)
    {
        if (iconLibrary != null)
        {
            initialPoolSize = iconLibrary.GetIconCount();
        }

        base.Initialize(poolParent);
    }

    public UIIcon GetWithIcon(PickupType pickupType)
    {
        UIIcon icon = Get();
        if (icon != null)
        {
            icon.SetIcon(iconLibrary.GetIcon(pickupType));
        }
        return icon;
    }
}