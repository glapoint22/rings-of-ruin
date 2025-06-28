using UnityEngine;
using System.Collections.Generic;

public class UIPoolManager : MonoBehaviour
{
    [SerializeField] protected MultiPrefabPool uiPool;
    private Dictionary<PickupType, GameObject> activeIcons = new Dictionary<PickupType, GameObject>();

    protected virtual void Awake()
    {
        RectTransform panel = GetComponent<RectTransform>();
        uiPool.Initialize(panel);
    }

    protected virtual void OnEnable()
    {
        GameEvents.OnPickupUpdate += AddIcon;
        GameEvents.OnBuffExpired += RemoveIcon;
    }



    protected void AddIcon(PickupType pickupType)
    {
        if (activeIcons.ContainsKey(pickupType)) return;


        GameObject icon = uiPool.Get(pickupType);
        if (icon != null)
        {
            activeIcons.Add(pickupType, icon);
        }
    }

    protected void RemoveIcon(PickupType pickupType)
    {
        if (activeIcons.TryGetValue(pickupType, out GameObject icon))
        {
            uiPool.Return(icon);
            activeIcons.Remove(pickupType);
        }
    }
}