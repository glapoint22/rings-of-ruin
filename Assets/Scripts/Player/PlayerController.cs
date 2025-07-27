using UnityEngine;
using UnityEngine.AI;


public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent navMeshAgent;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Player player;

    private readonly float stopRunningDelay = 0.05f;
    private float lastDestinationSetTime;

    private void OnEnable()
    {
        GameEvents.OnRightMousePressed += HandleRightMousePressed;
    }

    private void HandleRightMousePressed(Vector3 screenPosition)
    {
        // If player is dead, don't move
        if (player.playerState.isDead) return;

        Ray ray = Camera.main.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            navMeshAgent.SetDestination(hit.point);
            animator.SetBool("Running", true);
            lastDestinationSetTime = Time.time;
        }
    }

    private void Update()
    {
        if (!navMeshAgent.pathPending && !navMeshAgent.hasPath && navMeshAgent.velocity.magnitude == 0 &&
            animator.GetBool("Running") && Time.time - lastDestinationSetTime > stopRunningDelay)
        {
            animator.SetBool("Running", false);
        }
    }

    void OnAnimatorMove()
    {
        if (animator.GetBool("Running"))
        {
            navMeshAgent.speed = (animator.deltaPosition / Time.deltaTime).magnitude;
        }
    }
}
