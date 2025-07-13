using UnityEngine;

public abstract class EnemyStateMachine : MonoBehaviour
{
    protected IEnemyState currentState;
    protected EnemyStateContext context;

    protected virtual void Start()
    {
        context = new EnemyStateContext();
        context.transform = transform;
        context.playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        
        Initialize();
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

    protected abstract void Initialize();
    protected abstract IEnemyState GetInitialState();
}