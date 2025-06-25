public class Damage : IPlayerState
{
    public DamageInfo damageInfo;
    public Damage(DamageInfo damageInfo)
    {
        this.damageInfo = damageInfo;
    }
    public PlayerState UpdateState(PlayerState state)
    {
        state.health -= damageInfo.damage;
        return state;
    }
}