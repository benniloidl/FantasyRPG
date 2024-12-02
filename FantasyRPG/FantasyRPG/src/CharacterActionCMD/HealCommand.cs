public class HealCommand : ICommand
{
    private readonly Controller _controller;
    private Character _character;

    public HealCommand(Controller controller, Character character)
    {
        _controller = controller;
        _character = character;
    }

    // Increase the character's health by 10
    public void Execute()
    {
        _controller.AddNotification("Character is healing.");
        _character.Health += 10;
    }
}
