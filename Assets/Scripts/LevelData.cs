using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Rings of Ruin/Level Data")]
public class LevelData : ScriptableObject
{
    public int levelID;
    public List<RingConfiguration> rings = new List<RingConfiguration>();
    public List<EnemySpawn> enemies = new List<EnemySpawn>();
    public bool isAltarLockedByKey = false;
}

[System.Serializable]
public class RingConfiguration
{
    public int ringIndex;
    public RingRotationDirection rotation = RingRotationDirection.None;
    public List<SegmentConfiguration> segments = new List<SegmentConfiguration>();
}


[System.Serializable]
public class SegmentConfiguration
{
    public int segmentIndex;
    public bool isLocked = false;



    public SegmentType segmentType = SegmentType.Normal;

    public bool hasPortal;

    public HazardType hazardType = HazardType.None;
    public PickupType pickupType = PickupType.None;
    public EnemyType enemyType = EnemyType.None;
    public CollectibleType collectibleType = CollectibleType.None;
    public bool hasCheckpoint;
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







public enum SegmentType
{
    Normal,
    Gap,
    Crumbling
}



public enum HazardType
{
    None,
    Crusher,
    Catapult,
    CrumblingTile
}

public enum PickupType
{
    None,
    Shield,
    Cloak,
    TimeDilation,
    Health,
    Decoy,
    Pathmaker,
    SpellSpawn
}



public enum RingRotationDirection
{
    None,
    Clockwise,
    CounterClockwise
}


public enum EnemyType
{
    None,
    Ruinwalker,
    Gravecaller,
    Bloodseeker
}



public enum CollectibleType
{
    None,
    Gem,
    Coin
}