using System;

public class Health : IPlayerState
{
    public int HealthValue { get; }
    public Health(int healthValue)
    {
        HealthValue = healthValue;
    }

    public PlayerState UpdateState(PlayerState state)
    {
        int healh = Math.Min(state.health + HealthValue, 100);
        state.health = healh;
        return state;
    }
}
