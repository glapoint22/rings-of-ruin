using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class RuneflareManager : MonoBehaviour
{
    [SerializeField] private LevelPrefabLibrary prefabLibrary;

    [Header("Runtime Settings")]
    private float minRadius;
    private float maxRadius;
    private LevelData currentLevelData;
    private Queue<RuneflareProjectile> runeflarePool;
    private List<RuneflareProjectile> activeRuneflares;

    private void Start()
    {
        LevelLoader.instance.OnLevelLoaded += OnLevelLoaded;
    }

    private void OnLevelLoaded(object sender, LevelData levelData)
    {
        Initialize(levelData);
    }

    // Called by LevelLoader when level is loaded
    public void Initialize(LevelData levelData)
    {
        if (!levelData.hasRuneflareHazard) return;

        currentLevelData = levelData;
        int ringCount = levelData.rings.Count;
        float outerRadius = RingConstants.BaseRadius + (ringCount - 1) * RingConstants.RingSpacing;
        minRadius = outerRadius + 2f;
        maxRadius = outerRadius + 2f;

        // Initialize object pool
        runeflarePool = new Queue<RuneflareProjectile>();
        activeRuneflares = new List<RuneflareProjectile>();
        
        // Create all runeflares upfront
        GameObject prefab = prefabLibrary.runeflarePrefab;
        if (prefab == null)
        {
            Debug.LogWarning("[RuneflareManager] No Runeflare prefab set!");
            return;
        }

        // Create the maximum number of runeflares we'll ever need
        for (int i = 0; i < levelData.maxConcurrentRuneflares; i++)
        {
            GameObject obj = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            obj.SetActive(false);
            RuneflareProjectile projectile = obj.GetComponent<RuneflareProjectile>();
            if (projectile != null)
            {
                projectile.OnRuneflareDestroyed += ReturnRuneflareToPool;
                runeflarePool.Enqueue(projectile);
                // Initialize spawn timer for this runeflare
                AssignNewSpawnTimer(projectile);
            }
        }
    }

    private void Update()
    {
        if (currentLevelData == null || !currentLevelData.hasRuneflareHazard) return;

        // Check all runeflares in the pool for spawning
        foreach (var runeflare in runeflarePool)
        {

            runeflare.DecreaseSpawnTimer(Time.deltaTime);
                

            if (runeflare.SpawnTimer <= 0 && activeRuneflares.Count < currentLevelData.maxConcurrentRuneflares)
            {
                SpawnRuneflare(runeflare);
            }
        }
    }

    private void AssignNewSpawnTimer(RuneflareProjectile runeflare)
    {
        float randomInterval = Random.Range(currentLevelData.runeflareIntervalRange.x, currentLevelData.runeflareIntervalRange.y);
        runeflare.UpdateSpawnTimer(randomInterval);
    }

    private void ReturnRuneflareToPool(object sender, System.EventArgs e)
    {
        RuneflareProjectile runeflare = sender as RuneflareProjectile;
        if (runeflare != null && activeRuneflares.Contains(runeflare))
        {
            activeRuneflares.Remove(runeflare);
            runeflare.gameObject.SetActive(false);
            runeflarePool.Enqueue(runeflare);
            // Assign new spawn timer when returned to pool
            AssignNewSpawnTimer(runeflare);
        }
    }

    private void SpawnRuneflare(RuneflareProjectile projectile)
    {
        if (runeflarePool.Contains(projectile))
        {
            runeflarePool = new Queue<RuneflareProjectile>(runeflarePool.Where(x => x != projectile));
            Vector3 spawn = GetRandomSpawnPosition();
            Vector3 target = GetRandomTargetPosition();

            projectile.gameObject.SetActive(true);
            projectile.transform.position = spawn;
            projectile.Launch(spawn, target);
            activeRuneflares.Add(projectile);
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float angleRad = Random.Range(0f, Mathf.PI * 2f);
        float radius = Random.Range(minRadius, maxRadius);
        Vector2 dir = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));

        return new Vector3(dir.x * radius, -2f, dir.y * radius);
    }

    private Vector3 GetRandomTargetPosition()
    {
        float angleRad = Random.Range(0f, Mathf.PI * 2f);
        float radius = Random.Range(RingConstants.BaseRadius, minRadius - 1f);

        float x = Mathf.Cos(angleRad) * radius;
        float z = Mathf.Sin(angleRad) * radius;
        return new Vector3(x, 0f, z);
    }
}