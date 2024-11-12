using System.Collections.Generic;

public class Controller
{
    private Dictionary<ConsoleKey, ICommand> _keyMappings = new Dictionary<ConsoleKey, ICommand>();
    private GameController _controller = new GameController();

    public void SetCommand(ConsoleKey key, ICommand command)
    {
        _keyMappings[key] = command;
    }

    public void HandleInput()
    {
        Console.WriteLine("Press a key (A: Attack, D: Defend, H: Heal, M: Move, Q: Quit):");
        var key = Console.ReadKey(intercept: true).Key;
        Console.WriteLine();

        if (_keyMappings.TryGetValue(key, out ICommand command))
        {
            _controller.ExecuteCommand(command);
        }
        else if (key == ConsoleKey.Q)
        {
            Console.WriteLine("Quitting game...");
            Environment.Exit(0);
        }
        else
        {
            Console.WriteLine("Unrecognized key. Try again.");
        }
    }
}
