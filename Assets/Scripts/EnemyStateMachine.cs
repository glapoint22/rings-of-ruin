using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public abstract class EnemyStateMachine : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent navMeshAgent;

    [SerializeField]
    private Animator animator;


    protected IEnemyState currentState;
    protected EnemyStateContext context;
    private List<Vector3> waypoints;
    private Vector3 targetWaypoint;

    private void OnEnable()
    {
        GameEvents.OnLevelLoaded += OnLevelLoaded;
    }

    private void OnLevelLoaded(LevelDataOld levelData)
    {
        context = new EnemyStateContext
        {
            health = 100f,
            transform = transform,
            navMeshAgent = navMeshAgent,
            animator = animator,
            waypoints = waypoints,
            targetWaypoint = targetWaypoint
        };
        currentState = GetInitialState();
        currentState.Enter(context);
    }


    public void SetWaypoints(List<Vector3> waypoints, Vector3 targetWaypoint)
    {
        this.waypoints = waypoints;
        this.targetWaypoint = targetWaypoint;
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
        if (animator.GetBool("Patrol"))
        {
            navMeshAgent.speed = (animator.deltaPosition / Time.deltaTime).magnitude;
        }
    }

    protected abstract IEnemyState GetInitialState();
}