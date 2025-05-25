using UnityEngine;

public class RingSegment : MonoBehaviour
{
    [Header("Segment Info")]
    [SerializeField] private int ringIndex;       // 0 = Ring1, 1 = Ring2...
    [SerializeField] private int segmentIndex;    // Index within the ring
    [SerializeField] private bool isGap;          // For logic/editor use

    [Header("Slot Anchors")]
    [SerializeField] private Transform slotCenter;
    [SerializeField] private Transform slotTrap;
    [SerializeField] private Transform slotAbove;
    [SerializeField] private Transform slotPortal;

    // Properties for read-only access
    public int RingIndex => ringIndex;
    public int SegmentIndex => segmentIndex;
    public bool IsGap => isGap;

    public Transform SlotCenter => slotCenter;
    public Transform SlotTrap => slotTrap;
    public Transform SlotAbove => slotAbove;
    public Transform SlotPortal => slotPortal;
}