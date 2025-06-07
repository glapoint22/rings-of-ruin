using UnityEngine;
using System.Collections;

public class SpikeHazard : MonoBehaviour
{
    [Header("Spike Settings")]
    [SerializeField] private Transform spikeGroup; // The moving spike mesh
    [SerializeField] private float extendOffset = 0.5f; // How far it moves up
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float activeDuration = 0.5f;
    [SerializeField] private float cycleDelay = 2.0f;

    private Vector3 retractedPosition;
    private Vector3 extendedPosition;
    private Coroutine cycleSpikesRoutine;


    private void Start()
    {
        retractedPosition = spikeGroup.localPosition;
        extendedPosition = retractedPosition + Vector3.up * extendOffset;

        if (cycleSpikesRoutine == null)
        {
            cycleSpikesRoutine = StartCoroutine(CycleSpikes());
        }
    }

    private IEnumerator CycleSpikes()
    {
        while (true)
        {
            // Extend
            yield return StartCoroutine(MoveTo(extendedPosition));

            // Stay extended
            yield return new WaitForSeconds(activeDuration);

            // Retract
            yield return StartCoroutine(MoveTo(retractedPosition));

            // Wait before next cycle
            yield return new WaitForSeconds(cycleDelay);
        }
    }

    private IEnumerator MoveTo(Vector3 target)
    {
        while (Vector3.Distance(spikeGroup.localPosition, target) > 0.01f)
        {
            spikeGroup.localPosition = Vector3.MoveTowards(
                spikeGroup.localPosition, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
        spikeGroup.localPosition = target;
    }

    public void OnPlayerTriggered(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("[SpikeHazard] Player hit by spike!");
            // Later: apply damage, trigger shield, etc.
        }
    }

}