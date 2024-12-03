using System.Collections.Generic;

public class Controller : IObserver
{
    private static Controller? _instance;

    private GameController _controller;
    private GameWorld _gameWorld;
    private ITerminalState _terminalState;
    private List<string> _notifications;
    private bool? _forceUpdate;

    private Controller()
    {
        // Initialize game controller, game world, terminal state and notifications
        _controller = new GameController();
        _gameWorld = GameWorld.GetInstance();
        _terminalState = new DefaultState(this);
        _notifications = new List<string>();

        // Add the controller to the game world's quest manager as an observer
        _gameWorld.AddControllerToQuestManagerObservers(this);
    }

    // Singleton pattern
    public static Controller GetInstance()
    {
        if (_instance == null)
        {
            _instance = new Controller();
        }
        return _instance;
    }

    public GameController GetGameController() => _controller;
    public GameWorld GetGameWorld() => _gameWorld;

    // Allow the terminal state to be updated
    public void SetTerminalState(ITerminalState terminalState) => _terminalState = terminalState;

    // Allow the notification to be updated
    public void AddNotification(string notification) => _notifications.Add(notification);

    // Allow the terminal to be forced to update before waiting for input
    public void ForceUpdate() => _forceUpdate = true;

    public void HandleInput()
    {
        // Remove defeated enemies from the game world
        _gameWorld.RemoveDefeatedEnemies();

        // Clear console
        Console.Clear();

        // State specific terminal print
        _terminalState.PrintTerminal();

        // Immediately proceed into the next terminal iteration if the terminal was forced to update
        if (_forceUpdate == true)
        {
            _forceUpdate = false;
            return;
        }

        // Print all notifications
        Console.WriteLine();
        foreach (string notification in _notifications)
        {
            Console.WriteLine(notification);
        }

        // Remove all notifications
        _notifications.Clear();

        // Read key input
        ConsoleKey key = Console.ReadKey().Key;

        // State specific input handling
        _terminalState.HandleInput(key);
    }

    public void Update(Quest quest)
    {
        // When a quest is updated, check if the quest is completed
        if (quest.Progress >= quest.Goal)
        {
            AddNotification($"Character has been notified that the quest has been completed!");
            AddNotification($"Character has received the quest reward: {quest.Reward.ToString()}");
        }
    }
}
