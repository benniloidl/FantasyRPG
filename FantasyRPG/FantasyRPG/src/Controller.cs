using System.Collections.Generic;

public class Controller
{
    private Dictionary<ConsoleKey, ICommand> _keyMappings = new Dictionary<ConsoleKey, ICommand>();
    private GameController _controller = new GameController();
    private GameWorld _gameWorld = GameWorld.GetInstance();

    public void SetCommand(ConsoleKey key, ICommand command)
    {
        _keyMappings[key] = command;
    }

    public void HandleInput()
    {
        Console.Clear();

        // Print current character
        Console.WriteLine($"Active character: {_gameWorld.GetActiveCharacter()}");

        // Print current character's location (structure and NPCs)
        _gameWorld.PrintLocation();

        // Print game world map
        _gameWorld.PrintMap();

        Console.WriteLine("Move using the arrow keys or");
        Console.WriteLine("Perform an action (A: Attack, D: Defend, H: Heal, M: Move, Q: Quit)");
        var key = Console.ReadKey(intercept: true).Key;
        Console.WriteLine();

        if (_keyMappings.TryGetValue(key, out ICommand command))
        {
            _controller.ExecuteCommand(command);

            // Delay for 1s
            System.Threading.Thread.Sleep(1000);
        }
        else if (key == ConsoleKey.LeftArrow)
        {
            _gameWorld.MoveActiveCharacter((0, -1));
        }
        else if (key == ConsoleKey.RightArrow)
        {
            _gameWorld.MoveActiveCharacter((0, 1));
        }
        else if (key == ConsoleKey.UpArrow)
        {
            _gameWorld.MoveActiveCharacter((-1, 0));
        }
        else if (key == ConsoleKey.DownArrow)
        {
            _gameWorld.MoveActiveCharacter((1, 0));
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
