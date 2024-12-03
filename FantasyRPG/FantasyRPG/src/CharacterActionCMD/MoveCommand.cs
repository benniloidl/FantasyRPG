public class MoveCommand : ICommand
{
    private Character _character;

    public MoveCommand(Character character)
    {
        _character = character;
    }

    public void Execute()
    {
        Controller.GetInstance().AddNotification("Character is moving.");
    }
}
