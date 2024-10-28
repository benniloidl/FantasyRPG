public class DragonFactory : IEnemyFactory
{
    public Enemy CreateEnemy(EnemyRank rank) => new Dragon(rank);
}