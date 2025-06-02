using UnityEngine;

/// <summary>
/// Represents a segment of a ring in the game, which can contain various game elements.
/// </summary>
public class RingSegment : MonoBehaviour
{
    private int ringIndex;
    private int segmentIndex;

    [Header("Slot Anchors")]
    [SerializeField] private Transform slotEnemy;
    [SerializeField] private Transform slotCollectible;
    [SerializeField] private Transform slotHazard;
    [SerializeField] private Transform slotPickup;
    [SerializeField] private Transform slotPortal;
    [SerializeField] private Transform slotCheckpoint;


    // Segment identification properties
    public int RingIndex => ringIndex;
    public int SegmentIndex => segmentIndex;

    // Game element slot properties
    public Transform SlotEnemy => slotEnemy;
    public Transform SlotCollectible => slotCollectible;
    public Transform SlotHazard => slotHazard;
    public Transform SlotPickup => slotPickup;
    public Transform SlotPortal => slotPortal;
    public Transform SlotCheckpoint => slotCheckpoint;



    public void SetSegment(int ringIndex, int segmentIndex)
    {
        this.ringIndex = ringIndex;
        this.segmentIndex = segmentIndex;
    }
}