using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class CrumblingPlatform : MonoBehaviour
{
    [SerializeField] private float crumbleDelay = 1.25f;

    private bool hasStartedCrumble = false;

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    public void OnPlayerStep(PlayerState player)
    {
        if (hasStartedCrumble) return;

        hasStartedCrumble = true;
        StartCoroutine(CrumbleRoutine());
    }

    private IEnumerator CrumbleRoutine()
    {
        Debug.Log("[CrumblingPlatform] Crumbling in " + crumbleDelay + "s...");

        yield return new WaitForSeconds(crumbleDelay);

        Destroy(gameObject); // Simple: just destroy the whole segment
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerState player = other.GetComponent<PlayerState>();
            if (player != null)
            {
                OnPlayerStep(player);
            }
        }
    }
}