public class MoveCommand : ICommand
{
    private Character _character;

    public MoveCommand(Character character)
    {
        _character = character;
    }

    public void Execute()
    {
        Console.WriteLine("Character is moving.");
    }
}
