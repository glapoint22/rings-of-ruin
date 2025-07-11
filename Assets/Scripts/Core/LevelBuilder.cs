using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    [Header("Pool Reference")]
    [SerializeField]
    private LevelPool levelPool;

    [SerializeField] private Transform levelRoot;

    private GameObject portalA;
    private GameObject portalB;

    public static Dictionary<int, Transform> RingRoots = new Dictionary<int, Transform>();

    private void OnEnable()
    {
        levelPool.Initialize(levelRoot);
        GameEvents.OnInteracted += OnInteracted;
    }

    private void OnInteracted(GameObject interactable)
    {
        levelPool.Return(interactable);
    }

    public void ClearLevel()
    {
        // Loop through each ring (assuming 4 rings: 0, 1, 2, 3)
        for (int ringIndex = 0; ringIndex < 4; ringIndex++)
        {
            Transform ringRoot = levelRoot.Find($"Ring_{ringIndex}");
            if (ringRoot == null) continue;

            // Loop through each segment in the ring
            for (int i = 0; i < ringRoot.childCount; i++)
            {
                Transform segmentTransform = ringRoot.GetChild(i);
                RingSegment ringSegment = segmentTransform.GetComponent<RingSegment>();

                if (ringSegment != null)
                {
                    // Return object from ground slot if it exists
                    if (ringSegment.SlotGround != null && ringSegment.SlotGround.childCount > 0)
                    {
                        GameObject childObject = ringSegment.SlotGround.GetChild(0).gameObject;
                        levelPool.Return(childObject);
                    }

                    // Return object from float slot if it exists
                    if (ringSegment.SlotFloat != null && ringSegment.SlotFloat.childCount > 0)
                    {
                        GameObject childObject = ringSegment.SlotFloat.GetChild(0).gameObject;
                        levelPool.Return(childObject);
                    }
                }

                levelPool.Return(segmentTransform.gameObject);
            }
        }

        // Reset portal references
        portalA = null;
        portalB = null;
    }

    public void BuildLevel(LevelData levelData)
    {
        if (levelData == null)
        {
            return;
        }

        foreach (var ring in levelData.rings)
        {
            BuildRing(ring);
        }

        setPortals();
    }

    private void BuildRing(RingConfiguration ring)
    {
        Transform ringRoot = levelRoot.Find($"Ring_{ring.ringIndex}");
        if (ringRoot == null) return;

        float ninetyDegreeOffset = Mathf.PI / 2f;
        float anglePerSegment = (Mathf.PI * 2f) / RingConstants.SegmentCount;
        float radius = RingConstants.BaseRadius + (ring.ringIndex * RingConstants.RingRadiusOffset);

        for (int i = 0; i < ring.segments.Count; i++)
        {
            float angle = -i * anglePerSegment + ninetyDegreeOffset;
            Vector3 position = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            Quaternion rotation = Quaternion.Euler(0, -angle * Mathf.Rad2Deg, 0);

            // Get from pool
            SegmentConfiguration segment = ring.segments[i];
            GameObject segmentGO = levelPool.Get(GetRingSegmentType(ring.ringIndex, segment.segmentType));
            if (segmentGO == null) continue;

            segmentGO.transform.SetPositionAndRotation(position, rotation);
            segmentGO.transform.SetParent(ringRoot);
            segmentGO.name = $"Ring{ring.ringIndex}_Seg{i}";

            RingSegment ringSegment = segmentGO.GetComponent<RingSegment>();
            if (ringSegment != null)
            {
                ConfigureSegment(ringSegment, segment);
                ringSegment.SetSegment(ring.ringIndex, i);
            }
        }
    }

    private RingSegmentType GetRingSegmentType(int ringIndex, SegmentType segmentType)
    {
        // Converts SegmentType enum to RingSegmentType enum for levelPool compatibility
        return (RingSegmentType)(ringIndex * 4 + (int)segmentType);
    }

    private void setPortals()
    {
        if (portalA == null || portalB == null) return;
        portalA.GetComponent<Portal>().linkedPortal = portalB.GetComponent<Portal>();
        portalB.GetComponent<Portal>().linkedPortal = portalA.GetComponent<Portal>();
    }



    private void ConfigureSegment(RingSegment ringSegment, SegmentConfiguration config)
    {
        // First check for float slot (spell)
        if (config.spellType != SpellType.None)
        {
            ConfigureSlotFloat(ringSegment, config.spellType);
            return;
        }

        // Check for Health and Key in float slot
        if (config.hasHealth)
        {
            // HERE is where we'd use the computed property:
            ConfigureSlotFloat(ringSegment, UtilityItem.Health); // This calls the computed property
            return;
        }

        if (config.hasKey)
        {
            // AND HERE:
            ConfigureSlotFloat(ringSegment, UtilityItem.Key); // This calls the computed property
            return;
        }

        // If no spell, check for ground elements
        ConfigureSlotGround(ringSegment, config);
    }



    private void ConfigureSlotGround(RingSegment ringSegment, SegmentConfiguration config)
    {
        if (ringSegment.SlotGround == null)
            return;

        if (config.collectibleType != CollectibleType.None)
        {
            GameObject collectible = levelPool.Get(config.collectibleType);
            if (collectible != null)
            {
                collectible.transform.SetPositionAndRotation(ringSegment.SlotGround.position, ringSegment.SlotGround.rotation);
                collectible.transform.SetParent(ringSegment.SlotGround);
                collectible.name = $"Collectible_{config.collectibleType}";

                // Set coin count for treasure chests
                if (config.collectibleType == CollectibleType.TreasureChest)
                {
                    var treasureChest = collectible.GetComponent<TreasureChestCollect>();
                    if (treasureChest != null)
                    {
                        treasureChest.SetCoinCount(config.treasureChestCoinCount);
                    }
                }
            }
        }
        else if (config.enemyType != EnemyType.None)
        {
            GameObject enemy = levelPool.Get(config.enemyType);
            if (enemy != null)
            {
                enemy.transform.SetPositionAndRotation(ringSegment.SlotGround.position, ringSegment.SlotGround.rotation);
                enemy.transform.SetParent(ringSegment.SlotGround);
                enemy.name = $"Enemy_{config.enemyType}";
            }
        }
        else if (config.portalType != PortalType.None)
        {
            GameObject portal = levelPool.Get(config.portalType);
            if (portal != null)
            {
                portal.transform.SetPositionAndRotation(ringSegment.SlotGround.position, ringSegment.SlotGround.rotation);
                portal.transform.SetParent(ringSegment.SlotGround);
                portal.name = $"{config.portalType}";

                if (config.portalType == PortalType.PortalA)
                {
                    portalA = portal;
                }
                else if (config.portalType == PortalType.PortalB)
                {
                    portalB = portal;
                }
            }
        }
    }

    private void ConfigureSlotFloat(RingSegment ringSegment, System.Enum enumType)
    {
        if (ringSegment.SlotFloat == null)
            return;

        GameObject floatObject = levelPool.Get(enumType);
        if (floatObject != null)
        {
            floatObject.transform.SetPositionAndRotation(ringSegment.SlotFloat.position, ringSegment.SlotFloat.rotation);
            floatObject.transform.SetParent(ringSegment.SlotFloat);
            floatObject.name = $"Float_{enumType}";
        }
    }
}