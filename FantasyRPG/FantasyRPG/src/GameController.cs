public class GameController
{
    private Stack<ICommand> _commandHistory = new Stack<ICommand>();

    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
        _commandHistory.Push(command);

        // Delay for 1s
        System.Threading.Thread.Sleep(1000);
    }
}