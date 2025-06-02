using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the configuration data for a level in the game.
/// </summary>
[CreateAssetMenu(menuName = "Rings of Ruin/Level Data")]
public class LevelData : ScriptableObject
{
    [Tooltip("Unique identifier for this level")]
    public int levelID;
    
    [Tooltip("Configuration for each ring in the level")]
    public List<RingConfiguration> rings = new List<RingConfiguration>();
    
    [Tooltip("Enemy spawn points in the level")]
    public List<EnemySpawn> enemies = new List<EnemySpawn>();
    
    [Tooltip("Whether the altar requires a key to unlock")]
    public bool isAltarLockedByKey = false;
}

/// <summary>
/// Configuration for a single ring in the level.
/// </summary>
[System.Serializable]
public class RingConfiguration
{
    [Tooltip("Index of this ring in the level")]
    public int ringIndex;
    
    [Tooltip("Direction this ring rotates")]
    public RingRotationDirection rotation = RingRotationDirection.None;
    
    [Tooltip("Configuration for each segment in this ring")]
    public List<SegmentConfiguration> segments = new List<SegmentConfiguration>();
}

/// <summary>
/// Configuration for a single segment within a ring.
/// </summary>
[System.Serializable]
public class SegmentConfiguration
{
    [Tooltip("Index of this segment in its ring")]
    public int segmentIndex;
    
    [Tooltip("Whether this segment is locked")]
    public bool isLocked = false;

    [Tooltip("Type of segment")]
    public SegmentType segmentType = SegmentType.Normal;

    [Tooltip("Type of portal on this segment, if any")]
    public PortalType portalType = PortalType.None;

    [Tooltip("Type of hazard on this segment, if any")]
    public HazardType hazardType = HazardType.None;
    
    [Tooltip("Type of pickup on this segment, if any")]
    public PickupType pickupType = PickupType.None;
    
    [Tooltip("Type of enemy on this segment, if any")]
    public EnemyType enemyType = EnemyType.None;
    
    [Tooltip("Type of collectible on this segment, if any")]
    public CollectibleType collectibleType = CollectibleType.None;
    
    [Tooltip("Whether this segment contains a checkpoint")]
    public bool hasCheckpoint;
}

/// <summary>
/// Defines an enemy spawn point in the level.
/// </summary>
[System.Serializable]
public class EnemySpawn
{
    [Tooltip("Type of enemy to spawn")]
    public EnemyType enemyType;
    
    [Tooltip("Ring index where the enemy spawns")]
    public int ringIndex;
    
    [Tooltip("Segment index where the enemy spawns")]
    public int segmentIndex;
}

/// <summary>
/// Defines a spell spawn point in the level.
/// </summary>
[System.Serializable]
public class SpellSpawnPoint
{
    [Tooltip("Ring index where the spell spawns")]
    public int ringIndex;
    
    [Tooltip("Segment index where the spell spawns")]
    public int segmentIndex;
}

#region Enums

/// <summary>
/// Types of segments that can exist in a ring.
/// </summary>
public enum SegmentType
{
    Normal,
    Gap,
    Crumbling
}

/// <summary>
/// Types of hazards that can be placed on segments.
/// </summary>
public enum HazardType
{
    None,
    Spike,
    Catapult
}

/// <summary>
/// Types of pickups that can be placed on segments.
/// </summary>
public enum PickupType
{
    None,
    Shield,
    Cloak,
    TimeDilation,
    Health,
    Decoy,
    Pathmaker,
    Spell,
    Key
}

/// <summary>
/// Possible rotation directions for rings.
/// </summary>
public enum RingRotationDirection
{
    None,
    Clockwise,
    CounterClockwise
}

/// <summary>
/// Types of enemies that can spawn in the level.
/// </summary>
public enum EnemyType
{
    None,
    Ruinwalker,
    Gravecaller,
    Bloodseeker
}

/// <summary>
/// Types of collectibles that can be placed on segments.
/// </summary>
public enum CollectibleType
{
    None,
    Gem,
    Coin
}

/// <summary>
/// Types of portals that can be placed on segments.
/// </summary>
public enum PortalType
{
    None,
    PortalA,
    PortalB
}

#endregion