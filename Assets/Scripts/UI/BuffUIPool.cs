using System.Collections.Generic;
using UnityEngine;

public class BuffUIPool : MonoBehaviour
{
    [SerializeField] private UIIcon buffUIPrefab;
    [SerializeField] private Transform poolParent; // Should be BuffsPanel
    [SerializeField] private int initialPoolSize = 10;

    private Queue<UIIcon> pool = new Queue<UIIcon>();

    private void Awake()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            UIIcon buffUI = Instantiate(buffUIPrefab, poolParent);
            buffUI.gameObject.SetActive(false);
            pool.Enqueue(buffUI);
        }
    }

    public UIIcon Get()
    {
        if (pool.Count > 0)
        {
            UIIcon buffUI = pool.Dequeue();
            buffUI.gameObject.SetActive(true);
            return buffUI;
        }
        else
        {
            // Pool is empty and we don't want to instantiate more
            return null;
        }
    }

    public void Return(UIIcon buffUI)
    {
        buffUI.gameObject.SetActive(false);
        pool.Enqueue(buffUI);
    }
} 