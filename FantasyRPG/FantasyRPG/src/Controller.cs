using System.Collections.Generic;

public class Controller
{
    private GameController _controller;
    private GameWorld _gameWorld;
    private ITerminalState _terminalState;

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

    public void HandleInput()
    {
        _terminalState.HandleState();
    }
}
