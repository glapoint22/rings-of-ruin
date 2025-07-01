using System.Collections.Generic;
using UnityEngine;



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
    public PortalType portalType = PortalType.None;
    public SpellType spellType = SpellType.None;
    public EnemyType enemyType = EnemyType.None;
    public CollectibleType collectibleType = CollectibleType.None;
    public int treasureChestCoinCount = 1;
    
    // Boolean fields for Health and Key
    public bool hasHealth = false;
    public bool hasKey = false;
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