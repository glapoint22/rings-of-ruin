using UnityEngine;

public class Cloak : IPlayerState, ITimeBasedBuff
{
    public float Duration { get; }
    public Cloak(float duration) {
        Duration = duration;
    }
    
    public PlayerState UpdateState(PlayerState state)
    {
        state.hasCloak = true;
        return state;
    }

    public PlayerState OnBuffExpired(PlayerState state)
    {
        state.hasCloak = false;
        GameEvents.RaiseBuffExpired(PickupType.Cloak);
        return state;
    }
}