using UnityEngine;
using System.Collections.Generic;

public class RuneflareManager : MonoBehaviour
{
    [SerializeField] private RuneflarePool runeflarePool;

    private float minRadius;
    private float maxRadius;
    private LevelDataOld currentLevelData;
    private Transform poolParent = null;
    private List<RuneflareProjectile> activeRuneflares = new();


    private void Start()
    {
        poolParent = GetComponent<Transform>();
        runeflarePool.Initialize(poolParent);
    }

    private void OnEnable()
    {
        GameEvents.OnLevelLoaded += OnLevelLoaded;
    }



    private void OnLevelLoaded(LevelDataOld levelData)
    {
        if (!levelData.hasRuneflareHazard) return;

        currentLevelData = levelData;
        int ringCount = levelData.rings.Count;
        float outerRadius = RingConstants.BaseRadius + (ringCount - 1) * RingConstants.RingRadiusOffset;
        minRadius = outerRadius + 2f;
        maxRadius = outerRadius + 2f;

        // Pre-populate the pool with the initial amount of runeflares
        runeflarePool.PrePopulateRuneflarePool(currentLevelData.maxConcurrentRuneflares);

        // Assign event handler and spawn timer to each pooled runeflare
        foreach (var runeflare in runeflarePool.GetInactiveRuneflares())
        {
            runeflare.OnRuneflareDestroyed += ReturnRuneflareToPool;
            AssignNewSpawnTimer(runeflare);
        }
    }




    private void Update()
    {
        if (currentLevelData == null || !currentLevelData.hasRuneflareHazard || activeRuneflares.Count >= currentLevelData.maxConcurrentRuneflares) return;

        // Check all inactive runeflares in the pool for spawning
        foreach (var runeflare in runeflarePool.GetInactiveRuneflares())
        {
            runeflare.DecreaseSpawnTimer(Time.deltaTime);

            if (runeflare.SpawnTimer <= 0)
            {
                SpawnRuneflare();
                break;
            }
        }
    }



    private void SpawnRuneflare()
    {
        GameObject projectile = runeflarePool.Get();
        if (projectile == null) return;

        Vector3 spawn = GetRandomPosition(true);
        Vector3 target = GetRandomPosition(false);

        projectile.SetActive(true);
        projectile.transform.position = spawn;
        projectile.GetComponent<RuneflareProjectile>().Launch(spawn, target);

        activeRuneflares.Add(projectile.GetComponent<RuneflareProjectile>()); // Track it as active
    }



    private void AssignNewSpawnTimer(RuneflareProjectile runeflare)
    {
        float randomInterval = Random.Range(currentLevelData.runeflareIntervalRange.x, currentLevelData.runeflareIntervalRange.y);
        runeflare.UpdateSpawnTimer(randomInterval);
    }



    private Vector3 GetRandomPosition(bool isSpawn)
    {
        float angleRad = Random.Range(0f, Mathf.PI * 2f);
        float radius = isSpawn ? Random.Range(minRadius, maxRadius) : Random.Range(RingConstants.BaseRadius, minRadius - 1f);
        float y = isSpawn ? -2f : 0f;

        return new Vector3(Mathf.Cos(angleRad) * radius, y, Mathf.Sin(angleRad) * radius);
    }



    private void ReturnRuneflareToPool(RuneflareProjectile runeflare)
    {
        if (activeRuneflares.Contains(runeflare))
        {
            activeRuneflares.Remove(runeflare);
            runeflarePool.Return(runeflare.gameObject);
            AssignNewSpawnTimer(runeflare); // Reset timer for reuse
        }
    }
}