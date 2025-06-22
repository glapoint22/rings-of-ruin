using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(menuName = "Rings of Ruin/Runeflare Pool")]
public class RuneflarePool : SinglePrefabPool
{
    public IEnumerable<RuneflareProjectile> GetInactiveRuneflares()
    {
        // Get the RuneflareProjectile component from each GameObject in the pool
        return pool.Select(gameObject => gameObject.GetComponent<RuneflareProjectile>());
    }

    public void PrePopulateRuneflarePool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject instance = CreateNewInstance();
            instance.SetActive(false);
            pool.Enqueue(instance);
        }
    }
}