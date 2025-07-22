using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.AI.Navigation;

public class LevelBuilderOld : MonoBehaviour
{
    [SerializeField] private Transform levelRoot;
    [SerializeField] private LevelPool levelPool;
    [SerializeField] private NavMeshSurface navMeshSurface;

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
                Slot ringSegment = segmentTransform.GetComponent<Slot>();

                if (ringSegment != null)
                {
                    // Return object from ground slot if it exists
                    if (ringSegment.SpawnPoint != null && ringSegment.SpawnPoint.childCount > 0)
                    {
                        GameObject childObject = ringSegment.SpawnPoint.GetChild(0).gameObject;
                        levelPool.Return(childObject);
                    }

                    // // Return object from float slot if it exists
                    // if (ringSegment.SlotFloat != null && ringSegment.SlotFloat.childCount > 0)
                    // {
                    //     GameObject childObject = ringSegment.SlotFloat.GetChild(0).gameObject;
                    //     levelPool.Return(childObject);
                    // }
                }

                levelPool.Return(segmentTransform.gameObject);
            }
        }
    }

    public void BuildLevel(LevelDataOld levelData)
    {
        if (levelData == null)
        {
            return;
        }

        foreach (var ring in levelData.rings)
        {
            BuildRing(ring);
        }

        navMeshSurface.BuildNavMesh();


        // NEW: Spawn enemies at waypoints after all segments are built
        SpawnEnemiesAtWaypoints(levelData);
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

            Slot ringSegment = segmentGO.GetComponent<Slot>();
            if (ringSegment != null)
            {
                ConfigureSegment(ringSegment, segment, ring.ringIndex);
                // ringSegment.SetSegment(ring.ringIndex, i);
            }
        }
    }

    private RingSegmentType GetRingSegmentType(int ringIndex, SegmentType segmentType)
    {
        // Converts SegmentType enum to RingSegmentType enum for levelPool compatibility
        return (RingSegmentType)(ringIndex * 4 + (int)segmentType);
    }




    private void ConfigureSegment(Slot ringSegment, SegmentConfiguration config, int ringIndex)
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

        // Check for Bridge
        if (config.hasBridge)
        {
            GameObject bridge = levelPool.Get(UtilityItem.Bridge);
            if (bridge != null)
            {
                // Calculate bridge position using dynamic math
                float currentRingRadius = RingConstants.BaseRadius + (ringIndex * RingConstants.RingRadiusOffset);
                float gapBetweenRings = RingConstants.RingRadiusOffset - RingConstants.SegmentThickness;
                float bridgeRadius = currentRingRadius - (RingConstants.SegmentThickness / 2f) - (gapBetweenRings / 2f);
                
                // Calculate angle (same as segment angle)
                float ninetyDegreeOffset = Mathf.PI / 2f;
                float anglePerSegment = (Mathf.PI * 2f) / RingConstants.SegmentCount;
                float angle = -config.segmentIndex * anglePerSegment + ninetyDegreeOffset;
                
                // Position and rotate the bridge
                Vector3 bridgePosition = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * bridgeRadius;
                Quaternion bridgeRotation = Quaternion.Euler(0, -angle * Mathf.Rad2Deg + 90f, 0);
                
                bridge.transform.SetPositionAndRotation(bridgePosition, bridgeRotation);
                bridge.transform.SetParent(levelRoot);
                bridge.name = $"Bridge_Ring{ringIndex}_Seg{config.segmentIndex}";
            }
        }

        // If no spell, check for ground elements
        ConfigureSlotGround(ringSegment, config);
    }



    private void ConfigureSlotGround(Slot ringSegment, SegmentConfiguration config)
    {
        if (ringSegment.SpawnPoint == null)
            return;

        if (config.isPlayerStart)
        {
            GameObject player = levelPool.Get(UtilityItem.Player);
            if (player != null)
            {
                player.transform.SetPositionAndRotation(ringSegment.SpawnPoint.position, ringSegment.SpawnPoint.rotation);
                player.transform.SetParent(ringSegment.SpawnPoint);
                player.name = "Player";
            }
        }

        if (config.collectibleType != CollectibleType.None)
        {
            GameObject collectible = levelPool.Get(config.collectibleType);
            if (collectible != null)
            {
                collectible.transform.SetPositionAndRotation(ringSegment.SpawnPoint.position, ringSegment.SpawnPoint.rotation);
                collectible.transform.SetParent(ringSegment.SpawnPoint);
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
    }

    private void ConfigureSlotFloat(Slot ringSegment, System.Enum enumType)
    {
        // if (ringSegment.SlotFloat == null)
        //     return;

        // GameObject floatObject = levelPool.Get(enumType);
        // if (floatObject != null)
        // {
        //     floatObject.transform.SetPositionAndRotation(ringSegment.SlotFloat.position, ringSegment.SlotFloat.rotation);
        //     floatObject.transform.SetParent(ringSegment.SlotFloat);
        //     floatObject.name = $"Float_{enumType}";
        // }
    }

    // NEW: Method to spawn enemies at waypoints
    private void SpawnEnemiesAtWaypoints(LevelDataOld levelData)
    {
        var waypointGroups = levelData.GetEnemyWaypointGroups();

        foreach (var kvp in waypointGroups)
        {
            EnemyType enemyType = kvp.Key;
            List<WaypointLocation> waypoints = kvp.Value;

            // Skip if no waypoints for this enemy type
            if (waypoints.Count == 0) continue;

            List<Slot> ringSegmentsWithWaypoints = new List<Slot>();
            foreach (var waypoint in waypoints)
            {
                Transform ringRoot = levelRoot.Find($"Ring_{waypoint.ringIndex}");
                if (ringRoot == null) continue;

                Transform segmentTransform = ringRoot.Find($"Ring{waypoint.ringIndex}_Seg{waypoint.segmentIndex}");
                if (segmentTransform == null) continue;

                Slot ringSegment = segmentTransform.GetComponent<Slot>();
                ringSegmentsWithWaypoints.Add(ringSegment);
            }

            // Randomly select one waypoint for this enemy type
            int randomIndex = Random.Range(0, ringSegmentsWithWaypoints.Count);
            Slot selectedRingSegmentWithWaypoint = ringSegmentsWithWaypoints[randomIndex];


            // Spawn the enemy at the waypoint
            GameObject enemy = levelPool.Get(enemyType);

            enemy.GetComponent<EnemyStateMachine>().SetWaypoints(ringSegmentsWithWaypoints.Select(rs => rs.Waypoint.position).ToList(), selectedRingSegmentWithWaypoint.Waypoint.position);
            if (enemy != null)
            {
                enemy.transform.SetPositionAndRotation(selectedRingSegmentWithWaypoint.Waypoint.position, selectedRingSegmentWithWaypoint.Waypoint.rotation);
                enemy.transform.SetParent(selectedRingSegmentWithWaypoint.Waypoint);
                enemy.name = $"Enemy_{enemyType}";
            }
        }
    }
}