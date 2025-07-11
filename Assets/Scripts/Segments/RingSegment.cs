using UnityEngine;

public class RingSegment : MonoBehaviour
{
    private int ringIndex;
    private int segmentIndex;

    [Header("Slot Anchors")]
    [SerializeField] private Transform slotFloat;
    [SerializeField] private Transform slotGround;
    


    // Segment identification properties
    public int RingIndex => ringIndex;
    public int SegmentIndex => segmentIndex;

    // Game element slot properties
    public Transform SlotGround => slotGround;
    public Transform SlotFloat => slotFloat;



    public void SetSegment(int ringIndex, int segmentIndex)
    {
        this.ringIndex = ringIndex;
        this.segmentIndex = segmentIndex;
    }
}