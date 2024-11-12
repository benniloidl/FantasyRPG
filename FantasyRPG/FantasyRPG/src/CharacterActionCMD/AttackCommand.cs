public class AttackCommand : ICommand
{
    private Character _attacker;
    private Character _target;

    public AttackCommand(Character attacker, Character target)
    {
        _attacker = attacker;
        _target = target;
    }

    public void Execute()
    {
        Console.WriteLine("An Attack has been executed.");
        _target.health -= 10;
    }
}
