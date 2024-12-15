public class GameWorldLoadState : ILoadState
{
    private ITerminalState? _nextState;
    private readonly ITerminalState _toBeNextState;

    private readonly Dictionary<(int, int), WorldMapStructure> _worldMapStructures;

    public GameWorldLoadState()
    {
        // Ensure that the next state will be the default state
        _toBeNextState = new DefaultState();

        // Get world map structures from the database
        _worldMapStructures = DatabaseManager.GetInstance().GetGameWorldStructures();
    }

    public ITerminalState? GetNextState() => _nextState;

    public void PrintTerminal()
    {
        Controller controller = Controller.GetInstance();

        // Skip this state if no game world structures are saved
        if (_worldMapStructures.Count == 0)
        {
            // Request a change to the next state
            _nextState = _toBeNextState;
        }

        Console.Write("Would you like to import the following saved game world structures?");
        Console.WriteLine();

        // Print saved game world structures
        _worldMapStructures.Keys.ToList().ForEach(key =>
        {
            Console.WriteLine($"({key.Item1}, {key.Item2}): {_worldMapStructures[key]}");
        });

        Console.WriteLine();
        Console.WriteLine("--------------------");
        Console.WriteLine();

        Console.WriteLine("Actions:");
        Console.WriteLine(" C: Confirm and load");
        Console.WriteLine(" Q: Quit and skip");
    }

    public void HandleInput(ConsoleKey key)
    {
        // Confirm and load when 'C' is pressed
        if (key == ConsoleKey.C)
        {
            // Load the game world structures and quit the LoadState
            Controller controller = Controller.GetInstance();
            GameWorld.GetInstance().LoadWorldMapStructures(_worldMapStructures);

            Quit();
        }
    }

    public void Quit()
    {
        // Request a change to the next state
        _nextState = _toBeNextState;
    }
}