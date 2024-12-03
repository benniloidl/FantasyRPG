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

    public override void Move() => Controller.GetInstance().AddNotification($"{Name} flies through the air.");
    public override void Attack(Character character)
    {
        // Reduce the character's health by the attack damage of the enemy
        int attackDamage = CalculateAttackDamage(character);
        character.Health -= attackDamage;

        Controller.GetInstance().AddNotification($"{Name} breathes fire. You received {attackDamage} Damage.");
    }
}