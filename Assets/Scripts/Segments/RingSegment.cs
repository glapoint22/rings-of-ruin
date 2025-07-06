using UnityEngine;

public class RingSegment : MonoBehaviour
{
    private int ringIndex;
    private int segmentIndex;
    [SerializeField] private SegmentType segmentType;

    [Header("Slot Anchors")]
    [SerializeField] private Transform slotGround;
    [SerializeField] private Transform slotFloat;


    // Segment identification properties
    public int RingIndex => ringIndex;
    public int SegmentIndex => segmentIndex;
    public SegmentType SegmentType => segmentType;

    // Game element slot properties
    public Transform SlotGround => slotGround;
    public Transform SlotFloat => slotFloat;



    public void SetSegment(int ringIndex, int segmentIndex)
    {
        this.ringIndex = ringIndex;
        this.segmentIndex = segmentIndex;
    }
}