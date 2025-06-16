using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    //private const int SEGMENT_COUNT = 24;
    //private const float BASE_RADIUS = 5f;
    //private const float RING_SPACING = 2.5f;


    [Header("Prefab Library Reference")]
    [SerializeField]
    private LevelPrefabLibrary prefabLibrary;


    private GameObject portalA;
    private GameObject portalB;


    public static Dictionary<int, Transform> RingRoots = new Dictionary<int, Transform>();


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


        RingRoots.Clear();

        foreach (var ring in levelData.rings)
        {
            BuildRing(ring, parentRoot);
        }

        setPortals();
    }




    private void BuildRing(RingConfiguration ring, Transform parentRoot)
    {
        float radius = RingConstants.BaseRadius + ring.ringIndex * RingConstants.RingSpacing;

        // 🔧 Create a parent object for this ring
        GameObject ringRoot = new GameObject($"Ring_{ring.ringIndex}");
        ringRoot.transform.SetParent(parentRoot);
        ringRoot.transform.localPosition = Vector3.zero;
        ringRoot.transform.localRotation = Quaternion.identity;


        RingRoots[ring.ringIndex] = ringRoot.transform;


        // Attach RotatingRing if needed
        if (ring.rotation != RingRotationDirection.None)
        {
            var rotator = ringRoot.AddComponent<RotatingRing>();
            rotator.SetRotationDirection(ring.rotation); // injects rotation from data
        }

        for (int i = 0; i < ring.segments.Count; i++)
        {
            SegmentConfiguration segment = ring.segments[i];

            float angle = -i * Mathf.PI * 2f / RingConstants.SegmentCount + Mathf.PI / 2f;
            Vector3 position = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            Quaternion rotation = Quaternion.Euler(0, -angle * Mathf.Rad2Deg, 0);

            GameObject prefab = GetSegmentPrefab(ring.ringIndex, segment.segmentType);
            if (prefab == null) continue;

            // 💡 Instantiate under the new ringRoot
            GameObject segmentGO = Instantiate(prefab, position, rotation, ringRoot.transform);
            segmentGO.name = $"Ring{ring.ringIndex}_Seg{i}";

            RingSegment ringSegment = segmentGO.GetComponent<RingSegment>();
            if (ringSegment != null)
            {
                ConfigureSegment(ringSegment, segment);
                ringSegment.SetSegment(ring.ringIndex, i);
            }
        }
    }





    private void setPortals()
    {
        if (portalA == null || portalB == null) return;
        portalA.GetComponent<Portal>().linkedPortal = portalB.GetComponent<Portal>();
        portalB.GetComponent<Portal>().linkedPortal = portalA.GetComponent<Portal>();
    }






    private GameObject GetSegmentPrefab(int ringIndex, SegmentType segmentType)
    {
        switch (segmentType)
        {
            case SegmentType.Gap:
                if (ringIndex >= 0 && ringIndex < prefabLibrary.gapSegmentPrefabs.Length)
                    return prefabLibrary.gapSegmentPrefabs[ringIndex];
                break;

            case SegmentType.Crumbling:
                if (ringIndex >= 0 && ringIndex < prefabLibrary.crumblingPlatformPrefabs.Length)
                    return prefabLibrary.crumblingPlatformPrefabs[ringIndex];
                break;

            case SegmentType.Spike:
                if (ringIndex >= 0 && ringIndex < prefabLibrary.spikePlatformPrefabs.Length)
                    return prefabLibrary.spikePlatformPrefabs[ringIndex];
                break;

            case SegmentType.Normal:
            default:
                if (ringIndex >= 0 && ringIndex < prefabLibrary.normalSegmentPrefabs.Length)
                    return prefabLibrary.normalSegmentPrefabs[ringIndex];
                break;
        }

        return null;
    }




    private void ConfigureSegment(RingSegment ringSegment, SegmentConfiguration config)
    {
        // First check for float slot (pickup)
        if (config.pickupType != PickupType.None)
        {
            ConfigureSlotFloat(ringSegment, config);
            return; // If we have a pickup, we don't check for ground elements
        }

        // If no pickup, check for ground elements
        ConfigureSlotGround(ringSegment, config);
    }

    private void ConfigureSlotGround(RingSegment ringSegment, SegmentConfiguration config)
    {
        if (ringSegment.SlotGround == null)
            return;

        if (config.collectibleType != CollectibleType.None)
        {
            GameObject prefab = prefabLibrary.GetCollectiblePrefab(config.collectibleType);
            if (prefab != null)
            {
                GameObject collectible = Instantiate(prefab, ringSegment.SlotGround.position, ringSegment.SlotGround.rotation, ringSegment.SlotGround);
                collectible.name = $"Collectible_{config.collectibleType}";

                // Set coin count for treasure chests
                if (config.collectibleType == CollectibleType.TreasureChest)
                {
                    //var treasureChest = collectible.GetComponent<TreasureChestCollect>();
                    //if (treasureChest != null)
                    //{
                    //    treasureChest.SetCoinCount(config.treasureChestCoinCount);
                    //}
                }
            }
        }
        else if (config.enemyType != EnemyType.None)
        {
            GameObject prefab = prefabLibrary.GetEnemyPrefab(config.enemyType);
            if (prefab != null)
            {
                Instantiate(prefab, ringSegment.SlotGround.position, ringSegment.SlotGround.rotation, ringSegment.SlotGround)
                    .name = $"Enemy_{config.enemyType}";
            }
        }
        else if (config.portalType != PortalType.None)
        {
            GameObject prefab = config.portalType == PortalType.PortalA ?
                prefabLibrary.portalAPrefab :
                prefabLibrary.portalBPrefab;

            if (prefab != null)
            {
                GameObject portal = Instantiate(prefab, ringSegment.SlotGround.position, ringSegment.SlotGround.rotation, ringSegment.SlotGround);
                portal.name = $"Portal_{config.portalType}";

                if (config.portalType == PortalType.PortalA)
                {
                    portalA = portal;
                }
                else
                {
                    portalB = portal;
                }
            }
        }
        else if (config.hasCheckpoint)
        {
            GameObject prefab = prefabLibrary.checkpointPrefab;
            if (prefab != null)
            {
                Instantiate(prefab, ringSegment.SlotGround.position, ringSegment.SlotGround.rotation, ringSegment.SlotGround)
                    .name = "Checkpoint";
            }
        }
    }

    private void ConfigureSlotFloat(RingSegment ringSegment, SegmentConfiguration config)
    {
        if (ringSegment.SlotFloat == null)
            return;

        GameObject prefab = prefabLibrary.GetPickupPrefab(config.pickupType);
        if (prefab != null)
        {
            Instantiate(prefab, ringSegment.SlotFloat.position, ringSegment.SlotFloat.rotation, ringSegment.SlotFloat)
                .name = $"Pickup_{config.pickupType}";
        }
    }
}