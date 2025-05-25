using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Rings of Ruin/Level Data")]
public class LevelData : ScriptableObject
{
    public int levelID;
    public List<RingConfiguration> rings = new List<RingConfiguration>();
}

[System.Serializable]
public class RingConfiguration
{
    public int ringIndex;
    public List<SegmentConfiguration> segments = new List<SegmentConfiguration>();
}

public enum SegmentType
{
    Normal,
    Gap,
    Crumbling
}

[System.Serializable]
public class SegmentConfiguration
{
    public int segmentIndex;

    public SegmentType segmentType = SegmentType.Normal;

    public bool hasGem;
    public bool hasHazard;
    public bool hasPowerUp;
    public bool hasPortal;
}







//using System.Collections.Generic;
//using UnityEngine;

//[CreateAssetMenu(menuName = "Rings of Ruin/Level Data")]
//public class LevelData : ScriptableObject
//{
//    public int levelID;
//    public List<RingConfiguration> rings = new List<RingConfiguration>();
//}

//[System.Serializable]
//public class RingConfiguration
//{
//    public int ringIndex;
//    public List<SegmentConfiguration> segments = new List<SegmentConfiguration>();
//}

//[System.Serializable]
//public class SegmentConfiguration
//{
//    public int segmentIndex;

//    public bool isGap;
//    public bool isCrumbling;

//    public GameObject gemPrefab;
//    public GameObject hazardPrefab;
//    public GameObject powerUpPrefab;
//    public GameObject portalPrefab;
//}
