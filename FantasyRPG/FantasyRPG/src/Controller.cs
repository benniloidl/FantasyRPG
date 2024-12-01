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

        // Print current character and health
        Console.Write($"Active character: {_gameWorld.GetActiveCharacter()}");
        Console.WriteLine($" (Health: {_gameWorld.GetActiveCharacter().Health})");

        // Print character's inventory
        Console.WriteLine($"Inventory:");
        _gameWorld.GetActiveCharacter().GetInventory().GetItems().ForEach(item => Console.Write(" " + item));
        Console.WriteLine();

        // Print game world map
        _gameWorld.PrintMap();
        Console.WriteLine();

        // Print current quest
        _gameWorld.PrintCurrentQuest();
        Console.WriteLine();

        // Print current character's location (structure, first NPC and available quests)
        _gameWorld.PrintLocation();
        _gameWorld.PrintAvailableQuest();
        Console.WriteLine();

        Console.WriteLine("Move using the arrow keys or");

        if (_gameWorld.IsQuestAvailable())
        {
            Console.WriteLine("You can press 'Y' to accept the quest");
        }
        
        Console.WriteLine("Perform an action (A: Attack, D: Defend, H: Heal, M: Move, Q: Quit)");

        Enemy? enemy = _gameWorld.GetEnemyAtCurrentLocation();

        if (enemy != null)
        {
            Console.WriteLine($"Attacking {enemy.Name} ({enemy.Rank}) (Health: {enemy.Health}): Press 'A' to attack");
        }

        var key = Console.ReadKey(intercept: true).Key;
        Console.WriteLine();

        if (_keyMappings.TryGetValue(key, out ICommand command))
        {
            _controller.ExecuteCommand(command);
        }
        else if (key == ConsoleKey.A)
        {
            // Check if enemy is present
            if (enemy != null)
            {
                // Perform attack
                _controller.ExecuteCommand(new AttackCommand(_gameWorld.GetActiveCharacter(), enemy));

                // Check if quest is active and update its progress
                _gameWorld.UpdateQuestStatus();
            }
        }
        else if (key == ConsoleKey.D)
        {
            // Execute defend command for active character
            _controller.ExecuteCommand(new DefendCommand(_gameWorld.GetActiveCharacter()));
        }
        else if (key == ConsoleKey.H)
        {
            // Execute heal command for active character
            _controller.ExecuteCommand(new HealCommand(_gameWorld.GetActiveCharacter()));
        }
        else if (key == ConsoleKey.M)
        {
            // Execute move command for active character
            _controller.ExecuteCommand(new MoveCommand(_gameWorld.GetActiveCharacter()));
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
        else if (key == ConsoleKey.Y)
        {
            _gameWorld.AcceptQuest();
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
