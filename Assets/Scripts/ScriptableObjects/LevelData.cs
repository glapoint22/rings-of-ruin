using System.Collections.Generic;
using UnityEngine;



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
    public SegmentType segmentType = SegmentType.Normal;
    public PortalType portalType = PortalType.None;
    public PickupType pickupType = PickupType.None;
    public EnemyType enemyType = EnemyType.None;
    public CollectibleType collectibleType = CollectibleType.None;
    public bool hasCheckpoint;
    public int treasureChestCoinCount = 1;
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