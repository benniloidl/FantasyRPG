using System;

public class LoadState : ITerminalState
{
    private ILoadState _loadState;

    public LoadState()
    {
        // Start LoadState with CharacterLoadState
        _loadState = new CharacterLoadState();
    }

    public void PrintTerminal()
    {
        Console.WriteLine("Before opening the game, you can decide whether to import your progress.");
        Console.WriteLine();
        Console.WriteLine("--------------------");
        Console.WriteLine();

        // LoadState specific print
        _loadState.PrintTerminal();

        // If there is a request to change state, update the state and force an update, skipping HandleInput
        if (HandleStateChangeRequest()) Controller.GetInstance().ForceUpdate();
    }

    public void HandleInput(ConsoleKey key)
    {
        // Change to DefaultState when 'Q' is pressed
        if (key == ConsoleKey.Q)
        {
            // Load the selection into the game and quit the LoadState
            _loadState.Quit();
        }

        // LoadState specific handling
        _loadState.HandleInput(key);

        HandleStateChangeRequest();
    }

    private bool HandleStateChangeRequest()
    {
        // Check if there is a request to change state
        ITerminalState? nextState = _loadState.GetNextState();
        if (nextState == null) return false;

        // Check if the next state is a LoadState
        if (nextState is ILoadState)
        {
            // Update the state to the next state
            _loadState = (ILoadState)nextState;
            return true;
        }

        // Update the terminal state to the next state
        Controller.GetInstance().SetTerminalState(nextState);
        return true;
    }
}