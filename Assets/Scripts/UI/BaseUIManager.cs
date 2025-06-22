using UnityEngine;
using System.Collections.Generic;

public abstract class BaseUIManager : MonoBehaviour
{
    [SerializeField] protected UIPool uiPool;
    protected Dictionary<PickupType, UIIcon> activeIcons = new Dictionary<PickupType, UIIcon>();

    protected virtual void Awake()
    {
        RectTransform panel = GetComponent<RectTransform>();
        uiPool.Initialize(panel);
    }

    protected virtual void OnEnable()
    {
        SubscribeToEvents();
    }


    protected abstract void SubscribeToEvents();

    protected void AddIcon(PickupType pickupType)
    {
        if (activeIcons.ContainsKey(pickupType))
        {
            // Icon already active, maybe refresh or stack it
            return;
        }

        UIIcon icon = uiPool.GetWithIcon(pickupType);
        if (icon != null)
        {
            activeIcons.Add(pickupType, icon);
        }
    }

    protected void RemoveIcon(PickupType pickupType)
    {
        if (activeIcons.TryGetValue(pickupType, out UIIcon icon))
        {
            uiPool.Return(icon.gameObject);
            activeIcons.Remove(pickupType);
        }
    }
}