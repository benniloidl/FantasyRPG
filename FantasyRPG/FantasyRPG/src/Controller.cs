using System.Collections.Generic;

public class Controller
{
    private GameController _controller;
    private GameWorld _gameWorld;
    private ITerminalState _terminalState;
    private string? _notification;

    public Controller()
    {
        // Initialize game controller, game world and terminal state
        _controller = new GameController();
        _gameWorld = GameWorld.GetInstance();
        _terminalState = new DefaultState(this);
    }

    public GameController GetGameController() => _controller;
    public GameWorld GetGameWorld() => _gameWorld;

    // Allow the terminal state to be updated
    public void SetTerminalState(ITerminalState terminalState) => _terminalState = terminalState;

    // Allow the notification to be updated
    public void SetNotification(string notification) => _notification = notification;

    public void HandleInput()
    {
        // Remove defeated enemies from the game world
        _gameWorld.RemoveDefeatedEnemies();

        // Return to default state when in combat state and no enemies are present
        if (_terminalState is CombatState combatState && _gameWorld.GetEnemyAtCurrentLocation() == null)
        {
            SetTerminalState(new DefaultState(this));
        }

        // State specific terminal print
        _terminalState.PrintTerminal();

        // Print notification
        if (_notification != null)
        {
            Console.WriteLine();
            Console.WriteLine(_notification);
        }

        // State specific input handling
        _terminalState.HandleInput();

        // Before the next iteration, clear the notification
        _notification = null;
    }
}
