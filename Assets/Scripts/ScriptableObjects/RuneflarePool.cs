using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(menuName = "Rings of Ruin/Runeflare Pool")]
public class RuneflarePool : SinglePrefabPool
{
    public void InitializePool(Transform poolParent, int poolSize)
    {
        // Note: poolSize parameter is no longer used since we create on-demand
        Initialize(poolParent);
    }

    public IEnumerable<RuneflareProjectile> GetInactiveRuneflares()
    {
        // Cast the pool items to RuneflareProjectile
        return pool.Cast<RuneflareProjectile>();
    }
}