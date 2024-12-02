using System;

public class CombatState : ITerminalState
{
    private readonly Controller _controller;
    
    private Enemy _enemy;

    public CombatState(Controller controller)
    {
        _controller = controller;

        // Get the enemy at the current location, or throw an exception if no enemy is present
        _enemy = controller.GetGameWorld().GetEnemyAtCurrentLocation() ?? throw new InvalidOperationException("No enemy present at current location");
    }

    public void HandleState()
    {
        Console.Clear();

        Console.WriteLine("In Combat!");
        Console.WriteLine();
        Console.WriteLine("--------------------");
        Console.WriteLine();

        // Print current quest
        _controller.GetGameWorld().PrintCurrentQuest();
        Console.WriteLine();
        Console.WriteLine("--------------------");
        Console.WriteLine();

        // Print character name, health and equipment
        Console.Write($"Active character: {_controller.GetGameWorld().GetActiveCharacter()}");
        Console.WriteLine(_controller.GetGameWorld().HasMoreThanOneCharacter() ? " (Press 'C' to change)" : "");
        Console.Write($" ❤️ {_controller.GetGameWorld().GetActiveCharacter().Health}");
        Console.Write($" 🗡️ {_controller.GetGameWorld().GetActiveCharacter().GetEquippedWeapon()?.GetType().Name ?? "None"}");
        Console.Write($" 🛡️ {_controller.GetGameWorld().GetActiveCharacter().GetEquippedDefensive()?.GetType().Name ?? "None"}");
        Console.WriteLine($" 🧪 {_controller.GetGameWorld().GetActiveCharacter().GetEquippedUtility()?.GetType().Name ?? "None"}");

        Console.WriteLine();
        Console.WriteLine("--------------------");
        Console.WriteLine();

        // Print enemy name and health
        Console.WriteLine($"{_enemy.Name} ({_enemy.Rank})");
        Console.WriteLine($" ❤️ {_enemy.Health}");

        Console.WriteLine();
        Console.WriteLine("--------------------");
        Console.WriteLine();

        Console.WriteLine("Actions:");
        Console.WriteLine(" A: Attack");
        Console.WriteLine(" D: Defend");
        Console.WriteLine(" H: Heal");
        Console.WriteLine(" M: Move");

        Console.WriteLine();
        Console.WriteLine("Press 'I' to open the inventory");

        HandleInput();
    }

    void HandleInput()
    {
        // Read key input
        ConsoleKey key = Console.ReadKey().Key;

        // Determine active character
        Character activeCharacter = _controller.GetGameWorld().GetActiveCharacter();

        // Change active character when 'C' is pressed
        if (key == ConsoleKey.C)
            _controller.GetGameWorld().ChangeActiveCharacter();

        // Open inventory when 'I' is pressed
        else if (key == ConsoleKey.I)
            _controller.SetTerminalState(new InventoryState(_controller, this));

        // Attack when 'A' is pressed
        else if (key == ConsoleKey.A)
        {
            // Check if enemy is present
            if (_enemy != null)
            {
                // Perform attack
                _controller.GetGameController().ExecuteCommand(new AttackCommand(activeCharacter, _enemy));

                // Check if quest is active and update its progress
                _controller.GetGameWorld().UpdateQuestStatus();
            }
        }

        // Execute defend command for active character
        else if (key == ConsoleKey.D)
            _controller.GetGameController().ExecuteCommand(new DefendCommand(activeCharacter));

        // Execute heal command for active character
        else if (key == ConsoleKey.H)
            _controller.GetGameController().ExecuteCommand(new HealCommand(activeCharacter));

        // Execute move command for active character
        else if (key == ConsoleKey.M)
            _controller.GetGameController().ExecuteCommand(new MoveCommand(activeCharacter));
    }
}