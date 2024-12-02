public class AttackCommand : ICommand
{
    private readonly Controller _controller;
    private Character _attacker;
    private Enemy _target;

    // Constructor for attacking an enemy
    public AttackCommand(Controller controller, Character attacker, Enemy target)
    {
        _controller = controller;
        _attacker = attacker;
        _target = target;
    }

    public void Execute()
    {
        // Calculate the attack damage based on the attacker's strength and the equipped weapon
        int attackDamage = _attacker.Strength + (_attacker.GetEquippedWeapon()?.Damage ?? 0);

        // Reduce the target's health by the attacker's strength
        _target.Health -= attackDamage;

        _controller.AddNotification($"An attack has been executed. You made {attackDamage} Damage.");

        // Make sure the target's health doesn't go below 0
        if (_target.Health < 0) _target.Health = 0;

        // By the chance of 50%, the target does not respond with a counter-attack
        if (new Random().Next(0, 2) == 0) return;

        // Calculate the target's attack damage based on its strength and rank
        int targetAttackDamage = _target.Strength;
        switch (_target.Rank)
        {
            case EnemyRank.Elite:
                targetAttackDamage += 10;
                break;
            case EnemyRank.Boss:
                targetAttackDamage += 20;
                break;
        }

        // Randomly vary the target's base attack damage by +/- 10%, rounded to the nearest integer
        targetAttackDamage += new Random().Next(-targetAttackDamage / 10, targetAttackDamage / 10 + 1);

        // Reduce the target's attack damage based on the attacker's agility and equipped defensive item
        targetAttackDamage -= _attacker.Agility;
        targetAttackDamage -= _attacker.GetEquippedDefensive()?.Defense ?? 0;

        // Make sure the target's attack damage doesn't go below 0
        if (targetAttackDamage < 0) targetAttackDamage = 0;

        // Reduce the attacker's health by the target's attack damage
        _attacker.Health -= targetAttackDamage;

        _controller.AddNotification($"The enemy attacked you. You received {targetAttackDamage} Damage.");
    }
}
