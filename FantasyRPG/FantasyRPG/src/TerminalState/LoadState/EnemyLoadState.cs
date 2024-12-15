public class EnemyLoadState : ILoadState
{
    private ITerminalState? _nextState;
    private readonly ITerminalState _toBeNextState;

    private readonly Dictionary<Enemy, (int, int)> _savedEnemies;
    private List<Enemy> _enemies;

    private int cursorPosition = 0;

    public EnemyLoadState()
    {
        // Ensure that the next state will be the GameWorldLoadState
        _toBeNextState = new GameWorldLoadState();

        // Get enemies from the database
        _savedEnemies = DatabaseManager.GetInstance().GetEnemies();

        // Initialize the list of selected enemies
        _enemies = new List<Enemy>();
    }

    public ITerminalState? GetNextState() => _nextState;

    public void PrintTerminal()
    {
        Controller controller = Controller.GetInstance();

        // Skip this state if no enemies are saved
        if (_savedEnemies.Count == 0)
        {
            // Request a change to the next state
            _nextState = _toBeNextState;
        }

        Console.Write("Would you like to import one or many of the following saved enemies?");
        Console.WriteLine();

        // Print saved enemies
        for (int i = 0; i < _savedEnemies.Count; i++)
        {
            Console.Write(i == cursorPosition ? "> " : "  ");

            // Indicate if the enemy is selected
            Console.Write('[');
            if (_enemies.Contains(_savedEnemies.ElementAt(i).Key))
            {
                Console.Write('X');
            }
            else
            {
                Console.Write(' ');
            }
            Console.Write("] ");

            Console.Write(_savedEnemies.ElementAt(i).Key);
            Console.Write(" ❤️ " + _savedEnemies.ElementAt(i).Key.Health);

            // Print the enemy's weapon
            Console.WriteLine($" 🗡️ {_savedEnemies.ElementAt(i).Key.Weapon?.ToString() ?? "None"}");
        }

        Console.WriteLine();
        Console.WriteLine("--------------------");
        Console.WriteLine();

        Console.WriteLine("Actions:");
        Console.WriteLine(" ⬆️⬇️: Change selection");
        Console.WriteLine(" C: Confirm");
        Console.WriteLine(" Q: Quit");
    }

    public void HandleInput(ConsoleKey key)
    {
        // Update selection when arrow keys are pressed
        if (key == ConsoleKey.UpArrow)
        {
            cursorPosition = Math.Max(0, cursorPosition - 1);
        }
        else if (key == ConsoleKey.DownArrow)
        {
            cursorPosition = Math.Min(_savedEnemies.Count - 1, cursorPosition + 1);
        }

        // Confirm selection when 'C' is pressed
        else if (key == ConsoleKey.C)
        {
            if (_enemies.Contains(_savedEnemies.ElementAt(cursorPosition).Key))
            {
                _enemies.Remove(_savedEnemies.ElementAt(cursorPosition).Key);
            }
            else
            {
                _enemies.Add(_savedEnemies.ElementAt(cursorPosition).Key);
            }
        }
    }

    public void Quit()
    {
        Controller controller = Controller.GetInstance();

        // Add all selected enemies to the game world
        _enemies.ForEach(enemy => controller.GetGameWorld().AddEnemy(enemy, _savedEnemies.GetValueOrDefault(enemy)));

        // Request a change to the next state
        _nextState = _toBeNextState;
    }
}