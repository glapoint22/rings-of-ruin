using UnityEngine;

public struct Path
{
    public int ringIndex;
    public int segmentIndex;
    public Vector3 position;

    public Path(int ringIndex, int segmentIndex, Vector3 position)
    {
        this.ringIndex = ringIndex;
        this.segmentIndex = segmentIndex;
        this.position = position;
    }
}