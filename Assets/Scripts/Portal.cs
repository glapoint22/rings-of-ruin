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
        if (segment != null)
        {
            ringIndex = segment.RingIndex;
            segmentIndex = segment.SegmentIndex;
        }
        else
        {
            Debug.LogWarning("[Portal] No RingSegment found on portal: " + gameObject.name);
        }
    }


    public void OnPlayerStep(PlayerState player)
    {
        PlayerController controller = player.GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.SetPositionOnRing(linkedPortal.TargetRingIndex, linkedPortal.TargetSegmentIndex);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasPorted) return;
        if (other.CompareTag("Player"))
        {
            PlayerState player = other.GetComponent<PlayerState>();
            if (player != null)
            {
                linkedPortal.hasPorted = true;
                OnPlayerStep(player);
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        hasPorted = false;
    }
}