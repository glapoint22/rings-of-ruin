using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Rings of Ruin/Prefab Library")]
public class LevelPrefabLibrary : ScriptableObject
{
    [Header("Segments")]
    public GameObject[] normalSegmentPrefabs = new GameObject[4];
    public GameObject[] gapSegmentPrefabs = new GameObject[4];

    [Header("Collectibles")]
    public GameObject gemPrefab;
    public GameObject coinPrefab;

    [Header("Pickups")]
    public GameObject shieldPrefab;
    public GameObject cloakPrefab;
    public GameObject timeDilationPrefab;
    public GameObject healthPrefab;
    // public GameObject pathmakerPrefab;
    // public GameObject fireballPrefab;
    // public GameObject stormboltPrefab;
    // public GameObject bloodrootPrefab;
    public GameObject keyPrefab;

    [Header("Hazards")]
    public GameObject SpikePrefab;
    public GameObject runeflarePrefab;
    public GameObject[] crumblingPlatformPrefabs = new GameObject[4];

    [Header("Enemies")]
    public GameObject ruinwalkerPrefab;
    public GameObject gravecallerPrefab;
    public GameObject bloodseekerPrefab;

    [Header("Checkpoint")]
    public GameObject checkpointPrefab;

    [Header("Portal")]
    public GameObject portalAPrefab;
    public GameObject portalBPrefab;


    private Dictionary<CollectibleType, GameObject> collectiblePrefabs;
    private Dictionary<PickupType, GameObject> pickupPrefabs;
    private Dictionary<HazardType, GameObject> hazardPrefabs;
    private Dictionary<EnemyType, GameObject> enemyPrefabs;

    private void OnEnable()
    {
        InitializeDictionaries();
    }

    private void InitializeDictionaries()
    {
        collectiblePrefabs = new Dictionary<CollectibleType, GameObject>
        {
            { CollectibleType.Gem, gemPrefab },
            { CollectibleType.Coin, coinPrefab }
        };

        pickupPrefabs = new Dictionary<PickupType, GameObject>
        {
            { PickupType.Shield, shieldPrefab },
            { PickupType.Cloak, cloakPrefab },
            { PickupType.TimeDilation, timeDilationPrefab },
            { PickupType.Health, healthPrefab },
            { PickupType.Key, keyPrefab }
        };

        hazardPrefabs = new Dictionary<HazardType, GameObject>
        {
            { HazardType.Spike, SpikePrefab }
        };

        enemyPrefabs = new Dictionary<EnemyType, GameObject>
        {
            { EnemyType.Ruinwalker, ruinwalkerPrefab },
            { EnemyType.Gravecaller, gravecallerPrefab },
            { EnemyType.Bloodseeker, bloodseekerPrefab }
        };
    }

    public GameObject GetCollectiblePrefab(CollectibleType collectibleType)
    {
        if (collectiblePrefabs.TryGetValue(collectibleType, out GameObject prefab))
        {
            return prefab;
        }
        Debug.LogWarning($"No prefab found for collectible type: {collectibleType}");
        return null;
    }

    public GameObject GetPickupPrefab(PickupType pickupType)
    {
        if (pickupPrefabs.TryGetValue(pickupType, out GameObject prefab))
        {
            return prefab;
        }
        Debug.LogWarning($"No prefab found for pickup type: {pickupType}");
        return null;
    }

    public GameObject GetHazardPrefab(HazardType hazardType)
    {
        if (hazardPrefabs.TryGetValue(hazardType, out GameObject prefab))
        {
            return prefab;
        }
        Debug.LogWarning($"No prefab found for hazard type: {hazardType}");
        return null;
    }

    public GameObject GetEnemyPrefab(EnemyType enemyType)
    {
        if (enemyPrefabs.TryGetValue(enemyType, out GameObject prefab))
        {
            return prefab;
        }
        Debug.LogWarning($"No prefab found for enemy type: {enemyType}");
        return null;
    }
}