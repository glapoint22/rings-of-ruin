using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    //private const int SEGMENT_COUNT = 24;
    //private const float BASE_RADIUS = 5f;
    //private const float RING_SPACING = 2.5f;


    [Header("Prefab Library Reference")]
    [SerializeField]
    private LevelPrefabLibrary prefabLibrary;

    /// <summary>
    /// Entry point for building a level from data.
    /// </summary>
    /// <param name="levelData">The level definition to build from.</param>
    /// <param name="parentRoot">The parent transform to build the level under.</param>
    public void BuildLevel(LevelData levelData, Transform parentRoot)
    {
        if (levelData == null || prefabLibrary == null || parentRoot == null)
        {
            return;
        }

       


        foreach (var ring in levelData.rings)
        {
            BuildRing(ring, parentRoot);
        }
    }



    private void BuildRing(RingConfiguration ring, Transform parentRoot)
    {
        float radius = RingConstants.BaseRadius + ring.ringIndex * RingConstants.RingSpacing;

        for (int i = 0; i < ring.segments.Count; i++)
        {
            SegmentConfiguration segment = ring.segments[i];

            // Calculate angle around the circle
            float angle = -i * Mathf.PI * 2f / RingConstants.SegmentCount + Mathf.PI / 2f;
            Vector3 position = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            Quaternion rotation = Quaternion.Euler(0, -angle * Mathf.Rad2Deg, 0);

            // Choose prefab based on segment type
            GameObject prefab = GetSegmentPrefab(ring.ringIndex, segment.segmentType);
            if (prefab == null)
            {
                continue;
            }

            GameObject segmentGO = Instantiate(prefab, position, rotation, parentRoot);
            segmentGO.name = $"Ring{ring.ringIndex}_Seg{i}";

            RingSegment ringSegment = segmentGO.GetComponent<RingSegment>();
            if (ringSegment != null)
            {
                ConfigureSegment(ringSegment, segment);
            }

        }
    }


    private GameObject GetSegmentPrefab(int ringIndex, SegmentType segmentType)
    {
        if (segmentType == SegmentType.Gap)
        {
            if (ringIndex >= 0 && ringIndex < prefabLibrary.gapSegmentPrefabs.Length)
            {
                return prefabLibrary.gapSegmentPrefabs[ringIndex];
            }
        }
        else
        {
            if (ringIndex >= 0 && ringIndex < prefabLibrary.normalSegmentPrefabs.Length)
            {
                return prefabLibrary.normalSegmentPrefabs[ringIndex];
            }
        }

        return null;
    }



    private void ConfigureSegment(RingSegment ringSegment, SegmentConfiguration config)
    {
        ConfigureCollectible(ringSegment, config);
        ConfigurePickup(ringSegment, config);
        ConfigureHazard(ringSegment, config);
        ConfigureEnemy(ringSegment, config);
        ConfigurePortal(ringSegment, config);
        ConfigureCheckpoint(ringSegment, config);
    }



    private void ConfigureCollectible(RingSegment ringSegment, SegmentConfiguration config)
    {
        if (config.collectibleType == CollectibleType.None || ringSegment.SlotCollectible == null)
            return;

        GameObject prefab = prefabLibrary.GetCollectiblePrefab(config.collectibleType);
        if (prefab != null)
        {
            Instantiate(prefab, ringSegment.SlotCollectible.position, ringSegment.SlotCollectible.rotation, ringSegment.SlotCollectible)
                .name = $"Collectible_{config.collectibleType}";
        }
    }

    private void ConfigurePickup(RingSegment ringSegment, SegmentConfiguration config)
    {
        if (config.pickupType == PickupType.None || ringSegment.SlotPickup == null)
            return;

        GameObject prefab = prefabLibrary.GetPickupPrefab(config.pickupType);
        if (prefab != null)
        {
            Instantiate(prefab, ringSegment.SlotPickup.position, ringSegment.SlotPickup.rotation, ringSegment.SlotPickup)
                .name = $"Pickup_{config.pickupType}";
        }
    }

    private void ConfigureHazard(RingSegment ringSegment, SegmentConfiguration config)
    {
        if (config.hazardType == HazardType.None || ringSegment.SlotHazard == null)
            return;

        GameObject prefab = prefabLibrary.GetHazardPrefab(config.hazardType);
        if (prefab != null)
        {
            Instantiate(prefab, ringSegment.SlotHazard.position, ringSegment.SlotHazard.rotation, ringSegment.SlotHazard)
                .name = $"Hazard_{config.hazardType}";
        }
    }

    private void ConfigureEnemy(RingSegment ringSegment, SegmentConfiguration config)
    {
        if (config.enemyType == EnemyType.None || ringSegment.SlotEnemy == null)
            return;

        GameObject prefab = prefabLibrary.GetEnemyPrefab(config.enemyType);
        if (prefab != null)
        {
            Instantiate(prefab, ringSegment.SlotEnemy.position, ringSegment.SlotEnemy.rotation, ringSegment.SlotEnemy)
                .name = $"Enemy_{config.enemyType}";
        }
    }

    private void ConfigurePortal(RingSegment ringSegment, SegmentConfiguration config)
    {
        if (!config.hasPortal || ringSegment.SlotPortal == null)
            return;

        GameObject prefab = prefabLibrary.portalPrefab;
        if (prefab != null)
        {
            Instantiate(prefab, ringSegment.SlotPortal.position, ringSegment.SlotPortal.rotation, ringSegment.SlotPortal)
                .name = $"Portal";
        }
    }

    private void ConfigureCheckpoint(RingSegment ringSegment, SegmentConfiguration config)
    {
        if (!config.hasCheckpoint || ringSegment.SlotCheckpoint == null)
            return;

        GameObject prefab = prefabLibrary.checkpointPrefab;
        if (prefab != null)
        {
            Instantiate(prefab, ringSegment.SlotCheckpoint.position, ringSegment.SlotCheckpoint.rotation, ringSegment.SlotCheckpoint)
                .name = $"Checkpoint";
        }
    }

}
