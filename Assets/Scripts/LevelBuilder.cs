using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using System.Linq;

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
        if (segment.spawnType == SpawnType.None || segment.spawnType == SpawnType.Player || segment.spawnType == SpawnType.Enemy) return;

        Slot slot = GetSlot(segment.ringIndex, segment.segmentIndex);


        GameObject spawnItem = levelPool.Get(segment.spawnType);

        SetupGameObject(spawnItem, slot.SpawnPoint.position, slot.SpawnPoint.rotation, slot.SpawnPoint);
    }


    private void CachePlayerSpawn(Segment segment)
    {
        if (segment.spawnType == SpawnType.Player) playerSpawnSlot = GetSlot(segment.ringIndex, segment.segmentIndex);
    }


    private void CacheEnemySpawn(Segment segment)
    {

        if (segment.spawnType == SpawnType.Enemy)
        {
            EnemyType enemyType = ConvertWaypointTypeToEnemyType(segment.waypointType);
            Slot slot = GetSlot(segment.ringIndex, segment.segmentIndex);

            EnemySpawn enemySpawn = new EnemySpawn(enemyType, slot);
            enemySpawns.Add(enemySpawn);
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

    private EnemyType ConvertWaypointTypeToEnemyType(WaypointType waypointType)
    {
        if (waypointType == WaypointType.None) return EnemyType.None;

        string waypointName = waypointType.ToString();
        string enemyName = waypointName.Substring(0, waypointName.Length - 1); // Remove last character

        return System.Enum.TryParse<EnemyType>(enemyName, out var enemyType) ? enemyType : EnemyType.None;
    }


    public void SpawnPlayer()
    {
        GameObject player = levelPool.Get(SpawnType.Player);

        SetupGameObject(player, playerSpawnSlot.SpawnPoint.position, playerSpawnSlot.SpawnPoint.rotation, playerSpawnSlot.SpawnPoint);
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
            GameObject enemy = levelPool.Get(enemySpawn.enemyType);
            SetupGameObject(enemy, enemySpawn.slot.SpawnPoint.position, enemySpawn.slot.SpawnPoint.rotation, enemySpawn.slot.SpawnPoint);

            // Find and assign waypoints for this enemy
            AssignWaypointsToEnemy(enemy, enemySpawn.enemyType);
        }
    }

    private void AssignWaypointsToEnemy(GameObject enemy, EnemyType enemyType)
    {
        // Find the first available waypoint group for this enemy type
        WaypointType waypointTypeToRemove = WaypointType.None; // Use a default value
        List<Waypoint> waypointsToAssign = null;

        foreach (var kvp in groupedWaypoints)
        {
            if (ConvertWaypointTypeToEnemyType(kvp.Key) == enemyType)
            {
                waypointTypeToRemove = kvp.Key;
                waypointsToAssign = kvp.Value;
                break;
            }
        }

        // Assign waypoints
        var waypointComponent = enemy.GetComponent<EnemyStateMachine>();
        waypointComponent.SetWaypoints(waypointsToAssign.Select(w => w.position).ToList());

        // Remove the used waypoint group
        groupedWaypoints.Remove(waypointTypeToRemove);
    }
}