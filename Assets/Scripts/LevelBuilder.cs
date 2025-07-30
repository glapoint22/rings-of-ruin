using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using System.Linq;
using UnityEngine.AI;

public class LevelBuilder
{
    private readonly Dictionary<int, List<Slot>> slots = new();
    private const float NinetyDegreeOffset = Mathf.PI / 2f;
    private const float AnglePerSegment = Mathf.PI * 2f / RingConstants.SegmentCount;
    private readonly LevelPool levelPool;
    private readonly Transform levelRoot;
    private readonly NavMeshSurface navMeshSurface;
    private Slot playerSpawnSlot;
    private List<EnemySpawn> enemySpawns = new();
    private List<Waypoint> waypoints = new();
    private Dictionary<WaypointType, List<Waypoint>> groupedWaypoints;
    private Player player;


    public LevelBuilder(LevelPool levelPool, Transform levelRoot, NavMeshSurface navMeshSurface = null)
    {
        this.levelPool = levelPool;
        this.levelRoot = levelRoot;
        this.navMeshSurface = navMeshSurface;
    }


    public void BuildLevel(LevelData levelData)
    {
        BuildRings(levelData.rings);
        GroupWaypoints();
        BuildNavMesh();
    }


    private void BuildRings(List<Ring> rings)
    {
        foreach (var ring in rings)
        {
            BuildRing(ring);
        }
    }


    private void BuildRing(Ring ring)
    {
        foreach (var segment in ring.segments)
        {
            BuildSegment(segment);
            BuildBridge(segment);
            SpawnItem(segment);
            CachePlayerSpawn(segment);
            CacheEnemySpawn(segment);
            CacheWaypoint(segment);
        }
    }


    private void BuildSegment(Segment segment)
    {
        var (position, rotation) = GetSegmentPositionAndRotation(segment);

        GameObject ringSegment = levelPool.Get(segment.ringSegmentType);

        SetupGameObject(ringSegment, position, rotation, levelRoot);

        Slot slot = ringSegment.GetComponent<Slot>();
        slots.TryAdd(segment.ringIndex, new List<Slot>());
        slots[segment.ringIndex].Add(slot);
    }


    private void BuildBridge(Segment segment)
    {
        if (segment.hasBridge)
        {
            GameObject bridge = levelPool.Get(UtilityItem.Bridge);
            if (bridge != null)
            {
                // Calculate bridge position using dynamic math
                float currentRingRadius = RingConstants.BaseRadius + (segment.ringIndex * RingConstants.RingRadiusOffset);
                float gapBetweenRings = RingConstants.RingRadiusOffset - RingConstants.SegmentThickness;
                float bridgeRadius = currentRingRadius - (RingConstants.SegmentThickness / 2f) - (gapBetweenRings / 2f);

                // Calculate angle (same as segment angle)
                float ninetyDegreeOffset = Mathf.PI / 2f;
                float anglePerSegment = (Mathf.PI * 2f) / RingConstants.SegmentCount;
                float angle = -segment.segmentIndex * anglePerSegment + ninetyDegreeOffset;

                // Position and rotate the bridge
                Vector3 bridgePosition = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * bridgeRadius;
                Quaternion bridgeRotation = Quaternion.Euler(0, -angle * Mathf.Rad2Deg + 90f, 0);

                SetupGameObject(bridge, bridgePosition, bridgeRotation, levelRoot);

            }
        }
    }


    private (Vector3 position, Quaternion rotation) GetSegmentPositionAndRotation(Segment segment)
    {
        float radius = RingConstants.BaseRadius + (segment.ringIndex * RingConstants.RingRadiusOffset);
        float angle = -segment.segmentIndex * AnglePerSegment + NinetyDegreeOffset;
        Vector3 position = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
        Quaternion rotation = Quaternion.Euler(0, -angle * Mathf.Rad2Deg, 0);

        return (position, rotation);
    }


    private void BuildNavMesh()
    {
        if (navMeshSurface != null) navMeshSurface.BuildNavMesh();
    }


    private void SpawnItem(Segment segment)
    {
        if (segment.spawnType == SpawnType.None || segment.spawnType == SpawnType.Player || segment.enemySpawnType != EnemySpawnType.None) return;

        Slot slot = GetSlot(segment.ringIndex, segment.segmentIndex);


        GameObject spawnItem = levelPool.Get(segment.spawnType);

        SetupGameObject(spawnItem, slot.SpawnPoint.position, slot.SpawnPoint.rotation, slot.SpawnPoint);
    }


    private void CachePlayerSpawn(Segment segment)
    {
        if (segment.spawnType == SpawnType.Player)
        {
            playerSpawnSlot = GetSlot(segment.ringIndex, segment.segmentIndex);
        }
    }


    private void CacheEnemySpawn(Segment segment)
    {
        if (segment.enemySpawnType != EnemySpawnType.None)
        {
            Slot slot = GetSlot(segment.ringIndex, segment.segmentIndex);
            enemySpawns.Add(new EnemySpawn(segment.enemySpawnType, slot));
        }
    }


    private void CacheWaypoint(Segment segment)
    {
        if (segment.waypointType == WaypointType.None) return;


        Slot slot = GetSlot(segment.ringIndex, segment.segmentIndex);
        Waypoint waypoint = new Waypoint(segment.waypointType, slot.Waypoint.position);
        waypoints.Add(waypoint);
    }


    private Slot GetSlot(int ringIndex, int segmentIndex)
    {
        return slots[ringIndex][segmentIndex];
    }


    private void SetupGameObject(GameObject gameObject, Vector3 position, Quaternion rotation, Transform parent)
    {
        gameObject.transform.SetPositionAndRotation(position, rotation);
        gameObject.transform.SetParent(parent);
    }


    public void SpawnPlayer()
    {
        GameObject player = levelPool.Get(SpawnType.Player);
        this.player = player.GetComponent<Player>();
        player.GetComponent<NavMeshAgent>().Warp(playerSpawnSlot.SpawnPoint.position);
    }


    private void GroupWaypoints()
    {
        groupedWaypoints = waypoints
            .GroupBy(w => w.waypointType)
            .ToDictionary(g => g.Key, g => g.ToList());
    }


    public void SpawnEnemies()
    {
        foreach (EnemySpawn enemySpawn in enemySpawns)
        {
            GameObject enemy = levelPool.Get(enemySpawn.enemySpawnType);
            enemy.transform.rotation = enemySpawn.slot.SpawnPoint.rotation;
            enemy.GetComponent<NavMeshAgent>().Warp(enemySpawn.slot.SpawnPoint.position);

            // Find and assign waypoints for this enemy
            AssignWaypointsToEnemy(enemy, enemySpawn.enemySpawnType, enemySpawn.slot.SpawnPoint.position);
        }
    }


    private void AssignWaypointsToEnemy(GameObject enemy, EnemySpawnType enemySpawnType, Vector3 enemySpawnPoint)
    {
        // Find the first available waypoint group for this enemy type
        WaypointType waypointTypeToRemove = WaypointType.None;
        List<Waypoint> enemyWaypointsToAssign = null;

        foreach (var kvp in groupedWaypoints)
        {
            if (ConvertWaypointTypeToEnemySpawnType(kvp.Key) == enemySpawnType)
            {
                waypointTypeToRemove = kvp.Key;
                enemyWaypointsToAssign = kvp.Value;
                break;
            }
        }

        // Assign waypoints
        var enemyAI = enemy.GetComponent<EnemyAI>();
        enemyAI.Initialize(enemyWaypointsToAssign.Select(enemyWaypoint => enemyWaypoint.position).ToList(), enemySpawnPoint, player);

        // Remove the used waypoint group
        groupedWaypoints.Remove(waypointTypeToRemove);
    }


    private EnemySpawnType ConvertWaypointTypeToEnemySpawnType(WaypointType waypointType)
    {
        if (waypointType == WaypointType.None) return EnemySpawnType.None;

        string waypointName = waypointType.ToString();
        string enemyName = waypointName.Substring(0, waypointName.Length - 1); // Remove last character

        return System.Enum.TryParse<EnemySpawnType>(enemyName, out var enemySpawnType) ? enemySpawnType : EnemySpawnType.None;
    }
}