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
    public override void Attack(Character character)
    {
        // Reduce the character's health by the attack damage of the enemy
        int attackDamage = CalculateAttackDamage(character);
        character.Health -= attackDamage;

        Controller.GetInstance().AddNotification($"{Name} attacks with a dagger. You received {attackDamage} Damage.");
    }
}