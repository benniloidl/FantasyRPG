public class GoblinFactory : IEnemyFactory
{
    public Enemy CreateEnemy(EnemyRank rank) => new Goblin(rank);
}