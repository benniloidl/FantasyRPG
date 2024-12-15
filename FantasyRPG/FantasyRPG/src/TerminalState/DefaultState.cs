using System;

public class DefaultState : ITerminalState
{
    public void PrintTerminal()
    {
        Controller controller = Controller.GetInstance();

        // Change TerminalState to CombatState when enemy is present
        if (controller.GetGameWorld().GetEnemyAtCurrentLocation() != null)
        {
            controller.SetTerminalState(new CombatState());
            controller.ForceUpdate();
        }

        // Print map info
        Console.WriteLine($"FantasyRPG ⌚ {controller.GetGameWorld().Time} ☀️ {controller.GetGameWorld().Weather}");

        // Print current character and health
        Console.Write($"Active character: {controller.GetGameWorld().GetActiveCharacter()}");
        Console.WriteLine(controller.GetGameWorld().HasMoreThanOneCharacter() ? " (Press 'C' to change)" : "");
        Console.WriteLine();

        // Print game world map
        controller.GetGameWorld().PrintMap();
        Console.WriteLine();

        // Print current quest
        controller.GetGameWorld().PrintCurrentQuest();
        Console.WriteLine();

        // Print current character's location (structure, first NPC and available quests)
        controller.GetGameWorld().PrintLocation();
        controller.GetGameWorld().PrintAvailableQuest();
        Console.WriteLine();

        Console.WriteLine("Move using the arrow keys or");

        if (controller.GetGameWorld().IsQuestAvailable())
        {
            Console.WriteLine("You can press 'Y' to accept the quest");
        }

        Console.WriteLine("Press 'Q' to quit");
    }

    public void HandleInput(ConsoleKey key)
    {
        Controller controller = Controller.GetInstance();

        // Change to SaveState when 'Q' is pressed
        if (key == ConsoleKey.Q)
            controller.SetTerminalState(new SaveState());

        // Change TerminalState to InventoryState when 'I' is pressed
        else if (key == ConsoleKey.I)
            controller.SetTerminalState(new InventoryState(this));

        // Accept quest when 'Y' is pressed
        else if (key == ConsoleKey.Y)
            controller.GetGameWorld().AcceptQuest();

        // Change active character when 'C' is pressed
        else if (key == ConsoleKey.C)
            controller.GetGameWorld().ChangeActiveCharacter();

        // Move to the left when left arrow key is pressed
        else if (key == ConsoleKey.LeftArrow)
            controller.GetGameWorld().Move((0, -1));

        // Move to the right when right arrow key is pressed
        else if (key == ConsoleKey.RightArrow)
            controller.GetGameWorld().Move((0, 1));

        // Move up when up arrow key is pressed
        else if (key == ConsoleKey.UpArrow)
            controller.GetGameWorld().Move((-1, 0));

        // Move down when down arrow key is pressed
        else if (key == ConsoleKey.DownArrow)
            controller.GetGameWorld().Move((1, 0));
    }
}