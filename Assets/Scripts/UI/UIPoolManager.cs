using System;
using UnityEngine;
using System.Collections.Generic;

public abstract class UIPoolManager : MonoBehaviour
{
    [SerializeField] protected MultiPrefabPool uiPool;
    private Dictionary<Enum, GameObject> activeIcons = new Dictionary<Enum, GameObject>();

    protected virtual void Awake()
    {
        RectTransform panel = GetComponent<RectTransform>();
        uiPool.Initialize(panel);
    }


    protected void AddIcon(Enum pickupType)
    {
        if (activeIcons.ContainsKey(pickupType)) return;


        GameObject icon = uiPool.Get(pickupType);
        if (icon != null)
        {
            activeIcons.Add(pickupType, icon);
        }
    }

    protected void RemoveIcon(Enum pickupType)
    {
        if (activeIcons.TryGetValue(pickupType, out GameObject icon))
        {
            uiPool.Return(icon);
            activeIcons.Remove(pickupType);
        }
    }
}