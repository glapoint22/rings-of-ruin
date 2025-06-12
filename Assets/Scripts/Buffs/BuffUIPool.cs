using System.Collections.Generic;
using UnityEngine;

public class BuffUIPool : MonoBehaviour
{
    [SerializeField] private BuffUI buffUIPrefab;
    [SerializeField] private Transform poolParent; // Should be BuffsPanel
    [SerializeField] private int initialPoolSize = 10;

    private Queue<BuffUI> pool = new Queue<BuffUI>();

    private void Awake()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            BuffUI buffUI = Instantiate(buffUIPrefab, poolParent);
            buffUI.gameObject.SetActive(false);
            pool.Enqueue(buffUI);
        }
    }

    public BuffUI Get()
    {
        if (pool.Count > 0)
        {
            BuffUI buffUI = pool.Dequeue();
            buffUI.gameObject.SetActive(true);
            return buffUI;
        }
        else
        {
            // Pool is empty and we don't want to instantiate more
            return null;
        }
    }

    public void Return(BuffUI buffUI)
    {
        buffUI.gameObject.SetActive(false);
        pool.Enqueue(buffUI);
    }
} 