public class AttackCommand : ICommand
{
    private readonly Controller _controller;
    private Character _attacker;
    private Character? _targetCharacter;
    private Enemy? _targetEnemy;

    // Constructor for attacking another character
    public AttackCommand(Controller controller, Character attacker, Character target)
    {
        _controller = controller;
        _attacker = attacker;
        _targetCharacter = target;
    }

    // Constructor for attacking an enemy
    public AttackCommand(Controller controller, Character attacker, Enemy target)
    {
        _controller = controller;
        _attacker = attacker;
        _targetEnemy = target;
    }

    public void Execute()
    {
        _controller.AddNotification("An attack has been executed.");

        // Calculate the attack damage based on the attacker's strength and the equipped weapon
        int attackDamage = _attacker.Strength + (_attacker.GetEquippedWeapon()?.Damage ?? 0);

        // Reduce the target's health by the attacker's strength
        if (_targetCharacter != null) _targetCharacter.Health -= attackDamage;
        else if (_targetEnemy != null) _targetEnemy.Health -= attackDamage;
    }
}
