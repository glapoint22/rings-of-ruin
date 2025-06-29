using UnityEngine;
using System;

[RequireComponent(typeof(Collider))]
public class Portal : MonoBehaviour
{
    [NonSerialized]
    public Portal linkedPortal;

    [NonSerialized]
    public bool hasPorted;


    public int TargetRingIndex => ringIndex;
    public int TargetSegmentIndex => segmentIndex;
    private int ringIndex;
    private int segmentIndex;



    private void Start()
    {
        RingSegment segment = GetComponentInParent<RingSegment>();
        if (segment == null) return;
        ringIndex = segment.RingIndex;
        segmentIndex = segment.SegmentIndex;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (hasPorted) return;
        if (other.CompareTag("Player"))
        {
            PlayerController controller = other.GetComponent<PlayerController>();
            if (controller == null) return;
            linkedPortal.hasPorted = true;
            //controller.SetPositionOnRing(linkedPortal.TargetRingIndex, linkedPortal.TargetSegmentIndex);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        hasPorted = false;
    }
}