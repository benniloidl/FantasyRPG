public class Slime : Enemy
{
    public Slime(EnemyRank rank)
    {
        Name = "Slime";
        Health = 20;
        Mana = 5;
        Strength = 3;
        Agility = 1;
        Rank = rank;
        ScaleStatsByRank();
    }

    public override void Move() => Console.WriteLine($"{Name} moves by hopping.");
    public override void Attack() => Console.WriteLine($"{Name} attacks by bouncing at the enemy.");
}