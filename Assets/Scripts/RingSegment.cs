using UnityEngine;

/// <summary>
/// Represents a segment of a ring in the game, which can contain various game elements.
/// </summary>
public class RingSegment : MonoBehaviour
{
    //[Header("Segment Info")]
    //[SerializeField] private int ringIndex;
    //[SerializeField] private int segmentIndex;
    //[SerializeField] private bool isGap;

    [Header("Slot Anchors")]
    [SerializeField] private Transform slotEnemy;
    [SerializeField] private Transform slotCollectible;
    [SerializeField] private Transform slotHazard;
    [SerializeField] private Transform slotPickup;
    [SerializeField] private Transform slotPortal;
    [SerializeField] private Transform slotCheckpoint;
    

    // Segment identification properties
    //public int RingIndex => ringIndex;
    //public int SegmentIndex => segmentIndex;
    //public bool IsGap => isGap;

    // Game element slot properties
    public Transform SlotEnemy => slotEnemy;
    public Transform SlotCollectible => slotCollectible;
    public Transform SlotHazard => slotHazard;
    public Transform SlotPickup => slotPickup;
    public Transform SlotPortal => slotPortal;
    public Transform SlotCheckpoint => slotCheckpoint;

    //private void OnValidate()
    //{
    //    // Ensure indices are non-negative
    //    ringIndex = Mathf.Max(0, ringIndex);
    //    segmentIndex = Mathf.Max(0, segmentIndex);
    //}
}
