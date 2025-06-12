using UnityEngine;

public class SpikeTrigger : MonoBehaviour
{
    private SpikeSegment spike;

    private void Start()
    {
        spike = GetComponentInParent<SpikeSegment>();
    }

    private void OnTriggerEnter(Collider other)
    {
        spike?.OnPlayerTriggered(other);
    }
}