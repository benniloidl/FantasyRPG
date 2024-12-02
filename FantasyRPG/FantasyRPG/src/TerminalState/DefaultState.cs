using System;

public class DefaultState : ITerminalState
{
    private readonly Controller _controller;

    public DefaultState(Controller controller)
    {
        _controller = controller;
    }

    public void PrintTerminal()
    {
        // Change TerminalState to CombatState when enemy is present
        if (_controller.GetGameWorld().GetEnemyAtCurrentLocation() != null)
        {
            _controller.SetTerminalState(new CombatState(_controller));
            _controller.ForceUpdate();
        }

        // Print map info
        Console.WriteLine($"FantasyRPG ⌚ {_controller.GetGameWorld().Time} ☀️ {_controller.GetGameWorld().Weather}");

        // Print current character and health
        Console.Write($"Active character: {_controller.GetGameWorld().GetActiveCharacter()}");
        Console.WriteLine(_controller.GetGameWorld().HasMoreThanOneCharacter() ? " (Press 'C' to change)" : "");
        Console.WriteLine();

        // Print game world map
        _controller.GetGameWorld().PrintMap();
        Console.WriteLine();

        // Print current quest
        _controller.GetGameWorld().PrintCurrentQuest();
        Console.WriteLine();

        // Print current character's location (structure, first NPC and available quests)
        _controller.GetGameWorld().PrintLocation();
        _controller.GetGameWorld().PrintAvailableQuest();
        Console.WriteLine();

        Console.WriteLine("Move using the arrow keys or");

        if (_controller.GetGameWorld().IsQuestAvailable())
        {
            Console.WriteLine("You can press 'Y' to accept the quest");
        }

        Console.WriteLine("Press 'Q' to quit");
    }

    public void HandleInput(ConsoleKey key)
    {
        // Quit game when 'Q' is pressed
        if (key == ConsoleKey.Q)
        {
            Console.WriteLine("Quitting game...");
            Environment.Exit(0);
        }

        // Change TerminalState to InventoryState when 'I' is pressed
        else if (key == ConsoleKey.I)
            _controller.SetTerminalState(new InventoryState(_controller, this));

        // Accept quest when 'Y' is pressed
        else if (key == ConsoleKey.Y)
            _controller.GetGameWorld().AcceptQuest();

        // Change active character when 'C' is pressed
        else if (key == ConsoleKey.C)
            _controller.GetGameWorld().ChangeActiveCharacter();

        // Move to the left when left arrow key is pressed
        else if (key == ConsoleKey.LeftArrow)
            _controller.GetGameWorld().Move((0, -1));

        // Move to the right when right arrow key is pressed
        else if (key == ConsoleKey.RightArrow)
            _controller.GetGameWorld().Move((0, 1));

        // Move up when up arrow key is pressed
        else if (key == ConsoleKey.UpArrow)
            _controller.GetGameWorld().Move((-1, 0));

        // Move down when down arrow key is pressed
        else if (key == ConsoleKey.DownArrow)
            _controller.GetGameWorld().Move((1, 0));
    }
}