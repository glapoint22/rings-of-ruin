using UnityEngine;
using System.Collections;

public class RuneflareManager : MonoBehaviour
{
    [SerializeField] private LevelPrefabLibrary prefabLibrary;

    [Header("Runtime Settings")]
    private float spawnInterval = 6f;
    private float minRadius = 15f;
    private float maxRadius = 20f;
    private bool isActive = false;

    public void InitializeFromLevel(LevelData levelData)
    {
        if (!levelData.hasRuneflareHazard) return;

        isActive = true;
        spawnInterval = levelData.runeflareFrequency;

        int ringCount = levelData.rings.Count;
        float outerRadius = RingConstants.BaseRadius + (ringCount - 1) * RingConstants.RingSpacing;
        minRadius = outerRadius + 1f;
        maxRadius = outerRadius + 2f;

        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (isActive)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnFireball();
        }
    }

    private void SpawnFireball()
    {
        GameObject prefab = prefabLibrary.runeflarePrefab;
        if (prefab == null)
        {
            Debug.LogWarning("[RuneflareManager] No Runeflare prefab set!");
            return;
        }

        Vector3 spawn = GetRandomSpawnPosition();
        Vector3 target = GetRandomTargetPosition();

        GameObject obj = Instantiate(prefab, spawn, Quaternion.identity);
        RuneflareProjectile projectile = obj.GetComponent<RuneflareProjectile>();
        projectile?.Launch(spawn, target);
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
        // Strike zone: any point roughly within the rings
        float angleRad = Random.Range(0f, Mathf.PI * 2f);
        float radius = Random.Range(RingConstants.BaseRadius, minRadius - 1f);

        float x = Mathf.Cos(angleRad) * radius;
        float z = Mathf.Sin(angleRad) * radius;
        return new Vector3(x, 0f, z);
    }

}
