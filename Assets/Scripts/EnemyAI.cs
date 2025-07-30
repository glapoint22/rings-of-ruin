using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public abstract class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent navMeshAgent;

    [SerializeField]
    private Animator animator;
    protected IEnemyState currentState;
    protected EnemyStateContext context;


    public void Initialize(List<Vector3> waypoints, Vector3 currentWaypoint, Player player)
    {
        context = new EnemyStateContext
        {
            health = 100f,
            transform = transform,
            navMeshAgent = navMeshAgent,
            animator = animator,
            waypoints = waypoints,
            player = player,
            currentWaypoint = currentWaypoint
        };
        currentState = GetInitialState();
        currentState.Enter(context);
    }



    protected virtual void Update()
    {
        IEnemyState nextState = currentState.ShouldTransition(context);
        if (nextState != null)
        {
            currentState.Exit(context);
            currentState = nextState;
            currentState.Enter(context);
        }

        currentState.Update(context);
    }


    void OnAnimatorMove()
    {
        if (animator.GetBool("Patrol") || animator.GetBool("Chase"))
        {
            navMeshAgent.speed = (animator.deltaPosition / Time.deltaTime).magnitude;
        }
    }

    protected abstract IEnemyState GetInitialState();
}