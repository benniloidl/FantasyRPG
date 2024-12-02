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

        // Reduce the target's health by the attacker's strength
        if (_targetCharacter != null) _targetCharacter.Health -= _attacker.Strength;
        else if (_targetEnemy != null) _targetEnemy.Health -= _attacker.Strength;
    }
}
