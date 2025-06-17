using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Rings of Ruin/Runeflare Pool")]
public class RuneflarePool : BasePool<RuneflareProjectile>
{

    public void InitializePool(Transform poolParent, int poolSize)
    {
        initialPoolSize = poolSize;
        Initialize(poolParent);
    }



    public IEnumerable<RuneflareProjectile> GetInactiveRuneflares()
    {
        return pool; // 'pool' is the Queue<T> in BasePool
    }
}