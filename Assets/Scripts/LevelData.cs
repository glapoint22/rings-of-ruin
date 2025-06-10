using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the configuration data for a level in the game.
/// </summary>
[CreateAssetMenu(menuName = "Rings of Ruin/Level Data")]
public class LevelData : ScriptableObject
{
    public int levelID;
    public List<RingConfiguration> rings = new();
    public List<EnemySpawn> enemies = new();
    public bool isAltarLockedByKey = false;
    public bool hasRuneflareHazard = false;
    public int maxConcurrentRuneflares = 1;
    public Vector2 runeflareIntervalRange = new(6f, 10f);

}

/// <summary>
/// Configuration for a single ring in the level.
/// </summary>
[System.Serializable]
public class RingConfiguration
{
    public int ringIndex;
    public RingRotationDirection rotation = RingRotationDirection.None;
    public List<SegmentConfiguration> segments = new List<SegmentConfiguration>();
}

/// <summary>
/// Configuration for a single segment within a ring.
/// </summary>
[System.Serializable]
public class SegmentConfiguration
{
    public int segmentIndex;
    public SegmentType segmentType = SegmentType.Normal;
    public PortalType portalType = PortalType.None;
    public HazardType hazardType = HazardType.None;
    public PickupType pickupType = PickupType.None;
    public EnemyType enemyType = EnemyType.None;
    public CollectibleType collectibleType = CollectibleType.None;
    public bool hasCheckpoint;
}

/// <summary>
/// Defines an enemy spawn point in the level.
/// </summary>
[System.Serializable]
public class EnemySpawn
{
    public EnemyType enemyType;
    public int ringIndex;
    public int segmentIndex;
}

/// <summary>
/// Defines a spell spawn point in the level.
/// </summary>
[System.Serializable]
public class SpellSpawnPoint
{
    public int ringIndex;
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
    Spike
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