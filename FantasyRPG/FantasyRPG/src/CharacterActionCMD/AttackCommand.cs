public class AttackCommand : ICommand
{
    private Character _attacker;
    private Character? _targetCharacter;
    private Enemy? _targetEnemy;

    // Constructor for attacking another character
    public AttackCommand(Character attacker, Character target)
    {
        _attacker = attacker;
        _targetCharacter = target;
    }

    // Constructor for attacking an enemy
    public AttackCommand(Character attacker, Enemy target)
    {
        _attacker = attacker;
        _targetEnemy = target;
    }

    public void Execute()
    {
        Console.WriteLine("An attack has been executed.");

        // Reduce the target's health by the attacker's strength
        if (_targetCharacter != null) _targetCharacter.Health -= _attacker.Strength;
        else if (_targetEnemy != null) _targetEnemy.Health -= _attacker.Strength;
    }
}
