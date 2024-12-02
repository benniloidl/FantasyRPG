public class MoveCommand : ICommand
{
    private readonly Controller _controller;
    private Character _character;

    public MoveCommand(Controller controller, Character character)
    {
        _controller = controller;
        _character = character;
    }

    public void Execute()
    {
        _controller.AddNotification("Character is moving.");
    }
}
