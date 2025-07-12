using UnityEngine;

public class RingSegment : MonoBehaviour
{
    private int ringIndex;
    private int segmentIndex;

    [Header("Slot Anchors")]
    [SerializeField] private Transform slotFloat;
    [SerializeField] private Transform slotGround;
    [SerializeField] private Transform waypoint;
    


    // Segment identification properties
    public int RingIndex => ringIndex;
    public int SegmentIndex => segmentIndex;
    public Transform Waypoint => waypoint;


    // Game element slot properties
    public Transform SlotGround => slotGround;
    public Transform SlotFloat => slotFloat;



    public void SetSegment(int ringIndex, int segmentIndex)
    {
        this.ringIndex = ringIndex;
        this.segmentIndex = segmentIndex;
    }
}