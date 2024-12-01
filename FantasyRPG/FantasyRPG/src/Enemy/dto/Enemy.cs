public abstract class Enemy
{
    public string Name { get; protected set; }
    public int Health { get; set; }
    public int Mana { get; protected set; }
    public int Strength { get; protected set; }
    public int Agility { get; protected set; }
    public EnemyRank Rank { get; protected set; }

    // Common actions
    public abstract void Move();
    public abstract void Attack();

    // Factory method to scale attributes based on rank
    protected void ScaleStatsByRank()
    {
        switch (Rank)
        {
            case EnemyRank.Elite:
                Health *= 2;
                Strength += 10;
                Agility += 5;
                break;
            case EnemyRank.Boss:
                Health *= 3;
                Strength += 20;
                Agility += 10;
                break;
            case EnemyRank.Normal:
            default:
                break;
        }
    }
}