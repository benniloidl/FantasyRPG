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

        // By the chance of 50%, the goblin has a weapon
        if (new Random().Next(0, 2) == 0)
        {
            Weapon = GetRandomWeapon();
        }
    }

    public override void Move() => Controller.GetInstance().AddNotification($"{Name} moves stealthily.");
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

        Controller.GetInstance().AddNotification($"{Name} attacks with a dagger. You received {attackDamage} Damage.");
    }
}