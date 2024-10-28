public class Goblin : Enemy
{
    public Goblin(EnemyRank rank)
    {
        Name = "Goblin";
        Health = 30;
        Mana = 10;
        Strength = 5;
        Agility = 3;
        Rank = rank;
        ScaleStatsByRank();
    }

    public override void Move() => Console.WriteLine($"{Name} moves stealthily.");
    public override void Attack() => Console.WriteLine($"{Name} attacks with a dagger.");
}