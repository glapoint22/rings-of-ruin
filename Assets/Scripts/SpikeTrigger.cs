using UnityEngine;

public class SpikeTrigger : MonoBehaviour
{
    private SpikeHazard spike;

    private void Start()
    {
        spike = GetComponentInParent<SpikeHazard>();
    }

    private void OnTriggerEnter(Collider other)
    {
        spike?.OnPlayerTriggered(other);
    }
}