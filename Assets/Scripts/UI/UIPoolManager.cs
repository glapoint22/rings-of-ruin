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


    protected void AddIcon(Enum iconType)
    {
        // Prevent duplicate icons from being added to the ui
        if (activeIcons.ContainsKey(iconType)) return;


        GameObject icon = uiPool.Get(iconType);
        if (icon != null)
        {
            activeIcons.Add(iconType, icon);
        }
    }

    protected void RemoveIcon(Enum iconType)
    {
        if (activeIcons.TryGetValue(iconType, out GameObject icon))
        {
            uiPool.Return(icon);
            activeIcons.Remove(iconType);
        }
    }
}