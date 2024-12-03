public class AttackCommand : ICommand
{
    private Character _attacker;
    private Enemy _target;

    // Constructor for attacking an enemy
    public AttackCommand(Character attacker, Enemy target)
    {
        _attacker = attacker;
        _target = target;
    }

    public void Execute()
    {
        // Calculate the attack damage based on the attacker's strength and the equipped weapon
        int attackDamage = _attacker.Strength + (_attacker.GetEquippedWeapon()?.Damage ?? 0);

        // The agility of the target may reduce the attack damage
        attackDamage -= _target.Agility;

        // Make sure the attack damage doesn't go below 0
        if (attackDamage < 0) attackDamage = 0;

        // Reduce the target's health by the attack damage
        _target.Health -= attackDamage;

        Controller.GetInstance().AddNotification($"An attack has been executed. You made {attackDamage} Damage.");

        // Make sure the target's health doesn't go below 0
        if (_target.Health < 0) _target.Health = 0;

        // By the chance of 50%, the target responds with a counter attack. Otherwise, it moves
        if (new Random().Next(0, 2) == 0)
        {
            _target.Attack(_attacker);
        }
        else
        {
            _target.Move();
        }
    }
}
