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


    public void BuildLevel(LevelData levelData)
    {
        if (levelData == null)
        {
            return;
        }


        RingRoots.Clear();

        foreach (var ring in levelData.rings)
        {
            BuildRing(ring);
        }

        setPortals();
    }




    private void BuildRing(RingConfiguration ring)
    {
        float radius = RingConstants.BaseRadius + ring.ringIndex * RingConstants.RingSpacing;

        // 🔧 Create a parent object for this ring
        GameObject ringRoot = new GameObject($"Ring_{ring.ringIndex}");
        ringRoot.transform.SetParent(levelRoot);
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

            // Get from pool
            GameObject segmentGO = levelPool.Get(GetRingSegmentType(ring.ringIndex, segment.segmentType));
            if (segmentGO == null) continue;

            segmentGO.transform.SetPositionAndRotation(position, rotation);
            segmentGO.transform.SetParent(ringRoot.transform);
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
        // Each ring has 4 segment types (Normal, Gap, Crumbling, Spike)
        // ringIndex is 0-based, but RingSegmentType enum is 1-based (Ring_1, Ring_2, etc.)
        // Formula: ringIndex * 4 + (int)segmentType
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
        else if (config.interactableType != InteractableType.None)
        {
            GameObject interactable = levelPool.Get(config.interactableType);
            if (interactable != null)
            {
                interactable.transform.SetPositionAndRotation(ringSegment.SlotGround.position, ringSegment.SlotGround.rotation);
                interactable.transform.SetParent(ringSegment.SlotGround);
                interactable.name = $"{config.interactableType}";

                if (config.interactableType == InteractableType.PortalA)
                {
                    portalA = interactable;
                }
                else if (config.interactableType == InteractableType.PortalB)
                {
                    portalB = interactable;

                }
            }
        }
    }

    private void ConfigureSlotFloat(RingSegment ringSegment, SegmentConfiguration config)
    {
        if (ringSegment.SlotFloat == null)
            return;

        GameObject pickup = levelPool.Get(config.pickupType);
        if (pickup != null)
        {
            pickup.transform.SetPositionAndRotation(ringSegment.SlotFloat.position, ringSegment.SlotFloat.rotation);
            pickup.transform.SetParent(ringSegment.SlotFloat);
            pickup.name = $"Pickup_{config.pickupType}";
        }
    }
}