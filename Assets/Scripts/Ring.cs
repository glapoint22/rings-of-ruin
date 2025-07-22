using System.Collections.Generic;

[System.Serializable]
public class Ring
{
    public List<Segment> segments = new List<Segment>(24);
    
    public Ring(int ringIndex)
    {
        segments = new List<Segment>(24);
        for (int i = 0; i < 24; i++)
        {
            segments.Add(new Segment { ringIndex = ringIndex, segmentIndex = i, segmentType = SegmentType.Normal });
        }
    }
}