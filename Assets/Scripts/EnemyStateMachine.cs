using UnityEngine;
using System.Collections.Generic;

public abstract class EnemyStateMachine : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    protected IEnemyState currentState;
    protected EnemyStateContext context;
    private List<Vector3> waypoints;

    private void OnEnable()
    {
        GameEvents.OnLevelLoaded += OnLevelLoaded;
    }

    private void OnLevelLoaded(LevelData levelData)
    {
        context = new EnemyStateContext
        {
            health = 100f,
            transform = transform,
            playerTransform = playerTransform,
            pathfinder = new Pathfinder(levelData),
            pathMover = GetComponent<PathMover>(),
            waypoints = waypoints,
        };
        context.pathMover.SetStartPosition(transform.position);
        currentState = GetInitialState();
        currentState.Enter(context);
    }


    public void SetWaypoints(List<Vector3> waypoints)
    {
        this.waypoints = waypoints;
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

    protected abstract IEnemyState GetInitialState();
}