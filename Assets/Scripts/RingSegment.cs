using UnityEngine;

public class RingSegment : MonoBehaviour
{
    [Header("Segment Info")]
    [SerializeField] private int ringIndex;
    [SerializeField] private int segmentIndex;
    [SerializeField] private bool isGap;

    [Header("Slot Anchors")]
    [SerializeField] private Transform slotEnemy;
    [SerializeField] private Transform slotCollectible;
    [SerializeField] private Transform slotHazard;
    [SerializeField] private Transform slotPickup;
    [SerializeField] private Transform slotPortal;
    [SerializeField] private Transform slotCheckpoint;
    

    public int RingIndex => ringIndex;
    public int SegmentIndex => segmentIndex;
    public bool IsGap => isGap;

    public Transform SlotCollectible => slotCollectible;
    public Transform SlotHazard => slotHazard;
    public Transform SlotPickup => slotPickup;
    public Transform SlotPortal => slotPortal;
    public Transform SlotCheckpoint => slotCheckpoint;
    public Transform SlotEnemy => slotEnemy;
}
