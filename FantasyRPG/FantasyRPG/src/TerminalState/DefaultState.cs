using System;

public class DefaultState : ITerminalState
{
    public void PrintTerminal()
    {
        Controller controller = Controller.GetInstance();

        // Change TerminalState to CombatState when enemy is present
        if (GameWorld.GetInstance().GetEnemyAtCurrentLocation() != null)
        {
            controller.SetTerminalState(new CombatState());
            controller.ForceUpdate();
        }

        // Print map info
        Console.WriteLine($"FantasyRPG ⌚ {GameWorld.GetInstance().Time} ☀️ {GameWorld.GetInstance().Weather}");

        // Print current character and health
        Console.Write($"Active character: {GameWorld.GetInstance().GetActiveCharacter()}");
        Console.WriteLine(GameWorld.GetInstance().HasMoreThanOneCharacter() ? " (Press 'C' to change)" : "");
        Console.WriteLine();

        // Print game world map
        GameWorld.GetInstance().PrintMap();
        Console.WriteLine();

        // Print current quest
        GameWorld.GetInstance().PrintCurrentQuest();
        Console.WriteLine();

        // Print current character's location (structure, first NPC and available quests)
        GameWorld.GetInstance().PrintLocation();
        GameWorld.GetInstance().PrintAvailableQuest();
        Console.WriteLine();

        Console.WriteLine("Move using the arrow keys or");

        if (GameWorld.GetInstance().IsQuestAvailable())
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
            GameWorld.GetInstance().AcceptQuest();

        // Change active character when 'C' is pressed
        else if (key == ConsoleKey.C)
            GameWorld.GetInstance().ChangeActiveCharacter();

        // Move to the left when left arrow key is pressed
        else if (key == ConsoleKey.LeftArrow)
            GameWorld.GetInstance().Move((0, -1));

        // Move to the right when right arrow key is pressed
        else if (key == ConsoleKey.RightArrow)
            GameWorld.GetInstance().Move((0, 1));

        // Move up when up arrow key is pressed
        else if (key == ConsoleKey.UpArrow)
            GameWorld.GetInstance().Move((-1, 0));

        // Move down when down arrow key is pressed
        else if (key == ConsoleKey.DownArrow)
            GameWorld.GetInstance().Move((1, 0));
    }
}