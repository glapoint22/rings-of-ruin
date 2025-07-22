public struct EnemySpawn
{
    public EnemyType enemyType;
    public Slot slot;

    public EnemySpawn(EnemyType enemyType, Slot slot)
    {
        this.enemyType = enemyType;
        this.slot = slot;
    }
}