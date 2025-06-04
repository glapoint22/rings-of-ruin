using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CrumblingPlatform : MonoBehaviour
{
    [SerializeField] private float crumbleDelay = 1.25f;

    [Tooltip("Child object containing the visible mesh")]
    [SerializeField] private GameObject visualMesh;

    private bool hasStartedCrumble = false;
    private bool hasCrumbled = false;
    private bool playerOnPlatform = false;

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    public void OnPlayerStep(PlayerState player)
    {
        if (hasStartedCrumble) return;

        hasStartedCrumble = true;
        playerOnPlatform = true;

        StartCoroutine(CrumbleRoutine(player));
    }

    private IEnumerator CrumbleRoutine(PlayerState player)
    {
        Debug.Log("[CrumblingPlatform] Crumbling in " + crumbleDelay + "s...");

        yield return new WaitForSeconds(crumbleDelay);

        hasCrumbled = true;

        if (visualMesh != null)
        {
            Destroy(visualMesh);
        }

        if (playerOnPlatform && player != null)
        {
            PlayerController controller = player.GetComponent<PlayerController>();
            controller?.TriggerFall();
        }

        // NOTE: we keep this GameObject alive!
        // It becomes an invisible gap trigger from this point on.
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerState player = other.GetComponent<PlayerState>();
        if (player == null) return;

        Collider triggerCollider = GetComponent<Collider>();
        Collider playerCollider = other.GetComponent<Collider>();

        if (triggerCollider == null || playerCollider == null) return;

        if (hasCrumbled)
        {
            PlayerController controller = player.GetComponent<PlayerController>();
            controller?.TriggerFall();
        }
        else
        {
            // While intact: begin crumble logic
            playerOnPlatform = true;
            OnPlayerStep(player);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnPlatform = false;
        }
    }
}
