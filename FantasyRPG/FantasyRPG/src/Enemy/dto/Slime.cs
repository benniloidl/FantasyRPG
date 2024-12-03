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

    public override void Move() => Controller.GetInstance().AddNotification($"{Name} moves by hopping.");
    public override void Attack(Character character)
    {
        // Reduce the character's health by the attack damage of the enemy
        int attackDamage = CalculateAttackDamage(character);
        character.Health -= attackDamage;

        Controller.GetInstance().AddNotification($"{Name} attacks by bouncing at the you. You received {attackDamage} Damage.");
    }
}