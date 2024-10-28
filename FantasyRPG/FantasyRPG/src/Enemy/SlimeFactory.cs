public class SlimeFactory : IEnemyFactory
{
    public Enemy CreateEnemy(EnemyRank rank) => new Slime(rank);
}