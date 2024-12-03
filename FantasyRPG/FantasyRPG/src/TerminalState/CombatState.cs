using System;

public class CombatState : ITerminalState
{
    private readonly Controller _controller;
    
    private Enemy _enemy;

    public CombatState()
    {
        _controller = Controller.GetInstance();

        // Get the enemy at the current location, or throw an exception if no enemy is present
        _enemy = _controller.GetGameWorld().GetEnemyAtCurrentLocation() ?? throw new InvalidOperationException("No enemy present at current location");
    }

    public void PrintTerminal()
    {
        // Change TerminalState to DefaultState when no enemies are present
        if (_controller.GetGameWorld().GetEnemyAtCurrentLocation() == null)
        {
            _controller.SetTerminalState(new DefaultState());
            _controller.ForceUpdate();
        }

        Console.WriteLine("In Combat!");
        Console.WriteLine();
        Console.WriteLine("--------------------");
        Console.WriteLine();

        // Print current quest
        _controller.GetGameWorld().PrintCurrentQuest();
        Console.WriteLine();
        Console.WriteLine("--------------------");
        Console.WriteLine();

        Console.Write($"Characters:");

        // Iterate over characters
        foreach (Character character in _controller.GetGameWorld().GetCharacters())
        {
            // Make the active character stand out in blue
            if (character == _controller.GetGameWorld().GetActiveCharacter())
                Console.ForegroundColor = ConsoleColor.Blue;

            // Indicate downed characters with a dead skull
            if (character.Health <= 0)
                Console.Write(" 💀");

            Console.Write($" {character}");

            // Reset color
            Console.ResetColor();
        }

        // Print active character's health and equipment
        Console.WriteLine(_controller.GetGameWorld().HasMoreThanOneCharacter() ? " (Press 'C' to change)" : "");
        Console.Write($" ❤️ {_controller.GetGameWorld().GetActiveCharacter().Health}");
        Console.Write($" 🗡️ {_controller.GetGameWorld().GetActiveCharacter().GetEquippedWeapon()?.ToString() ?? "None"}");
        Console.Write($" 🛡️ {_controller.GetGameWorld().GetActiveCharacter().GetEquippedDefensive()?.ToString() ?? "None"}");
        Console.WriteLine($" 🧪 {_controller.GetGameWorld().GetActiveCharacter().GetEquippedUtility()?.ToString() ?? "None"}");

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
    }

    public void HandleInput(ConsoleKey key)
    {
        // Determine active character
        Character activeCharacter = _controller.GetGameWorld().GetActiveCharacter();

        // Change active character when 'C' is pressed
        if (key == ConsoleKey.C)
            _controller.GetGameWorld().ChangeActiveCharacter();

        // Open inventory when 'I' is pressed
        else if (key == ConsoleKey.I)
            _controller.SetTerminalState(new InventoryState(this));

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