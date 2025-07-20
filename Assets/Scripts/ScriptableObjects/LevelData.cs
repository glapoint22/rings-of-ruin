using System.Collections.Generic;
using UnityEngine;
using System.Linq;



[CreateAssetMenu(menuName = "Rings of Ruin/Level Data")]
public class LevelData : ScriptableObject
{

    public int levelID;
    public List<RingConfiguration> rings = new();
    public List<EnemySpawn> enemies = new();
    public bool hasRuneflareHazard = false;
    public int maxConcurrentRuneflares = 1;
    public Vector2 runeflareIntervalRange = new(6f, 10f);

    
    public int GemCount
    {
        get
        {
            int count = 0;
            foreach (var ring in rings)
            {
                foreach (var segment in ring.segments)
                {
                    if (segment.collectibleType == CollectibleType.Gem)
                    {
                        count++;
                    }
                }
            }
            return count;
        }
    }

    // New method to get waypoint groups for enemy spawning
    public Dictionary<EnemyType, List<WaypointLocation>> GetEnemyWaypointGroups()
    {
        var waypointGroups = new Dictionary<EnemyType, List<WaypointLocation>>();

        // Initialize lists for each enemy type
        foreach (EnemyType enemyType in System.Enum.GetValues(typeof(EnemyType)))
        {
            if (enemyType != EnemyType.None)
            {
                waypointGroups[enemyType] = new List<WaypointLocation>();
            }
        }

        // Build waypoint groups from segment data
        for (int ringIndex = 0; ringIndex < rings.Count; ringIndex++)
        {
            var ring = rings[ringIndex];
            for (int segmentIndex = 0; segmentIndex < ring.segments.Count; segmentIndex++)
            {
                var segment = ring.segments[segmentIndex];
                if (segment.enemyType != EnemyType.None)
                {
                    var waypoint = new WaypointLocation
                    {
                        ringIndex = ringIndex,
                        segmentIndex = segmentIndex
                    };
                    waypointGroups[segment.enemyType].Add(waypoint);
                }
            }
        }

        return waypointGroups;
    }
}



[System.Serializable]
public class RingConfiguration
{
    public int ringIndex;
    public List<SegmentConfiguration> segments = new List<SegmentConfiguration>();
}



[System.Serializable]
public class SegmentConfiguration
{
    public int segmentIndex;
    public SegmentType segmentType = SegmentType.Normal;
    public SpellType spellType = SpellType.None;
    public EnemyType enemyType = EnemyType.None;
    public CollectibleType collectibleType = CollectibleType.None;
    public int treasureChestCoinCount = 1;
    
    // Boolean fields for Health and Key
    public bool hasHealth = false;
    public bool hasKey = false;
    public bool isPlayerStart;
    public bool hasBridge = false;
}



[System.Serializable]
public class EnemySpawn
{
    public EnemyType enemyType;
    public int ringIndex;
    public int segmentIndex;
}



[System.Serializable]
public class SpellSpawnPoint
{
    public int ringIndex;
    public int segmentIndex;
}

// New data structure for waypoint locations
[System.Serializable]
public class WaypointLocation
{
    public int ringIndex;
    public int segmentIndex;
}