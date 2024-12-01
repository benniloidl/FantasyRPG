public class HealCommand : ICommand
{
    private Character _character;

    public HealCommand(Character character)
    {
        _character = character;
    }

    // Increase the character's health by 10
    public void Execute()
    {
        Console.WriteLine("Character is healing.");
        _character.Health += 10;
    }
}
