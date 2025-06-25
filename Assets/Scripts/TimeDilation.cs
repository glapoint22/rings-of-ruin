using UnityEngine;

public class TimeDilation : IPlayerState, ITimeBasedBuff
{
    public float Duration { get; } = 10f;

    public PlayerState UpdateState(PlayerState state)
    {
        state.hasTimeDilation = true;
        return state;
    }

    public PlayerState OnBuffExpired(PlayerState state)
    {
        state.hasTimeDilation = false;
        GameEvents.RaiseBuffExpired(PickupType.TimeDilation);
        return state;
    }
}
