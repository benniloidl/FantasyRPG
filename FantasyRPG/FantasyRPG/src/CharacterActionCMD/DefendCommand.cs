public class DefendCommand : ICommand
{
    private Character _character;

    public DefendCommand(Character character)
    {
        _character = character;
    }

    public void Execute()
    {
        Console.WriteLine("Character is defending.");
        _character.setCharacterState(new DefendingState());
        _character.PerformAction();
    }
}
