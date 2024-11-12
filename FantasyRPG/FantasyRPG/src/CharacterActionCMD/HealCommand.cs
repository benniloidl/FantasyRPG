public class HealCommand : ICommand
{
    private Character _character;

    public HealCommand(Character character)
    {
        _character = character;
    }

    public void Execute()
    {
        Console.WriteLine("Character is healing.");
        _character.health += 10;
    }
}
