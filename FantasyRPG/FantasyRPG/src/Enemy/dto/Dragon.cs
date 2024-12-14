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

        // By the chance of 50%, the dragon has a weapon
        if (new Random().Next(0, 2) == 0)
        {
            Weapon = GetRandomWeapon();
        }
    }

    public override void Move() => Controller.GetInstance().AddNotification($"{Name} flies through the air.");
    public override void Attack(Character character)
    {
        // Reduce the character's health by the attack damage of the enemy
        int attackDamage = CalculateAttackDamage(character);

        // By the chance of 75%, the enemy uses its weapon if available
        if (Weapon != null && new Random().Next(0, 4) == 0)
        {
            attackDamage += Weapon.Damage;

            // Apply attack damage on character's health
            character.Health -= attackDamage;

            Controller.GetInstance().AddNotification($"{Name} attacks with {Weapon.ToString()}. You received {attackDamage} Damage.");
            return;
        }

        // Apply attack damage on character's health
        character.Health -= attackDamage;

        Controller.GetInstance().AddNotification($"{Name} breathes fire. You received {attackDamage} Damage.");
    }
}