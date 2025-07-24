public struct EnemySpawn
{
    public EnemySpawnType enemySpawnType;
    public Slot slot;

    public EnemySpawn(EnemySpawnType enemySpawnType, Slot slot)
    {
        this.enemySpawnType = enemySpawnType;
        this.slot = slot;
    }
}