public class DefendCommand : ICommand
{
    private readonly Controller _controller;
    private Character _character;

    public DefendCommand(Controller controller, Character character)
    {
        _controller = controller;
        _character = character;
    }

    public void Execute()
    {
        _controller.AddNotification("Character is defending.");
        _character.setCharacterState(new DefendingState());
        _character.PerformAction();
    }
}
