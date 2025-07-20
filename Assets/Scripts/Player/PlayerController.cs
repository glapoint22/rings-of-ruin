using UnityEngine;
using UnityEngine.AI;


public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent navMeshAgent;

    [SerializeField]
    private Animator animator;

    private void OnEnable()
    {
        GameEvents.OnRightMousePressed += HandleRightMousePressed;
    }

    private void HandleRightMousePressed(Vector3 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            navMeshAgent.SetDestination(hit.point);
            animator.SetBool("Running", true);
        }
    }

    private void Update()
    {
        if (!navMeshAgent.pathPending && !navMeshAgent.hasPath && navMeshAgent.velocity.magnitude == 0 && animator.GetBool("Running"))
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
