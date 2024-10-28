public class Dragon : Enemy
{
    public Dragon(EnemyRank rank)
    {
        Name = "Dragon";
        Health = 100;
        Mana = 50;
        Strength = 20;
        Agility = 7;
        Rank = rank;
        ScaleStatsByRank();
    }

    public override void Move() => Console.WriteLine($"{Name} flies through the air.");
    public override void Attack() => Console.WriteLine($"{Name} breathes fire.");
}