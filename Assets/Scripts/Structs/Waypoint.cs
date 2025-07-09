using UnityEngine;

public struct Waypoint
{
    public int ringIndex;
    public int segmentIndex;
    public Vector3 position;

    public Waypoint(int ringIndex, int segmentIndex, Vector3 position)
    {
        this.ringIndex = ringIndex;
        this.segmentIndex = segmentIndex;
        this.position = position;
    }
}