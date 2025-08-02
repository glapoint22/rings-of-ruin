public struct DamageInfo
{
    public int damage;
    public DamageSource source;

    
    public DamageInfo(int damage, DamageSource source)
    {
        this.damage = damage;
        this.source = source;
    }
}