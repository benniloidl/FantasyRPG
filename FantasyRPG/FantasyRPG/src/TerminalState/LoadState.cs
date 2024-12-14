using System;

public class LoadState : ITerminalState
{
    private readonly Controller _controller;

    public LoadState()
    {
        _controller = Controller.GetInstance();
    }

    public void PrintTerminal()
    {
        Console.WriteLine("Before opening the game, you can decide whether to import your progress.");
        Console.WriteLine();
        Console.WriteLine("--------------------");
        Console.WriteLine();
        Console.Write("Would you like to import ");

        // Check if saveObject is a character
        if (_controller.GetGameWorld().GetCharacters().Count > 0)
        {
            Character character = _controller.GetGameWorld().GetCharacters().First();
            Console.WriteLine($"character {character}");
            Console.Write($" ❤️ {_controller.GetGameWorld().GetActiveCharacter().Health}");
            Console.Write($" 🗡️ {_controller.GetGameWorld().GetActiveCharacter().GetEquippedWeapon()?.ToString() ?? "None"}");
            Console.Write($" 🛡️ {_controller.GetGameWorld().GetActiveCharacter().GetEquippedDefensive()?.ToString() ?? "None"}");
            Console.WriteLine($" 🧪 {_controller.GetGameWorld().GetActiveCharacter().GetEquippedUtility()?.ToString() ?? "None"}");
            Console.WriteLine($"and {character.GetInventory().GetItems().Count()} items in their inventory?");
        }
        else if (_controller.GetGameWorld().GetEnemyLocations().Count > 0)
        {
            Enemy enemy = _controller.GetGameWorld().GetEnemyLocations().First().Key;
            Console.WriteLine($"enemy {enemy}");
            Console.Write($" ❤️ {enemy.Health}");
            Console.Write($" 🗡️ {enemy.Weapon?.ToString() ?? "None"}");
            Console.WriteLine("?");
        }
        else
        {
            Console.WriteLine("the game world structures?");
        }

        Console.WriteLine();
        Console.WriteLine("--------------------");
        Console.WriteLine();

        Console.WriteLine("Actions:");
        Console.WriteLine(" Y: Yes");
        Console.WriteLine(" N: No");
        Console.WriteLine(" Q: Quit");
    }

    public void HandleInput(ConsoleKey key)
    {
        // Quit game when 'Q' is pressed
        if (key == ConsoleKey.Q)
        {
            Console.WriteLine("Quitting game...");
            Environment.Exit(0);
        }

        if (_controller.GetGameWorld().GetCharacters().Count > 0)
        {
            Character character = _controller.GetGameWorld().GetCharacters().First();

            if (key == ConsoleKey.Y)
            {
                // Save character and remove from game world
                _controller.GetDatabaseManager().AddCharacter(character);
                _controller.GetGameWorld().RemoveCharacter(character);
                _controller.AddNotification($"Character {character} saved.");
            }
            else if (key == ConsoleKey.N)
            {
                // Just remove character from game world
                _controller.GetGameWorld().RemoveCharacter(character);
                _controller.AddNotification($"Character {character} not saved.");
            }
        }
        else if (_controller.GetGameWorld().GetEnemyLocations().Count > 0)
        {
            Enemy enemy = _controller.GetGameWorld().GetEnemyLocations().First().Key;
            (int, int) enemyLocation = _controller.GetGameWorld().GetEnemyLocations().GetValueOrDefault(enemy);

            if (key == ConsoleKey.Y)
            {
                // Save enemy and remove from game world
                _controller.GetDatabaseManager().AddEnemy(enemy, enemyLocation);
                _controller.GetGameWorld().GetEnemyLocations().Remove(enemy);
                _controller.AddNotification($"Enemy {enemy} saved.");
            }
            else if (key == ConsoleKey.N)
            {
                // Just remove enemy from game world
                _controller.GetGameWorld().GetEnemyLocations().Remove(enemy);
                _controller.AddNotification($"Enemy {enemy} not saved.");
            }
        }
        else
        {
            // Save game world structures
            if (key == ConsoleKey.Y)
            {
                // Clear prior saved game world structures
                _controller.GetDatabaseManager().ClearGameWorldStructures();

                // Save game world structures
                for (int i = 0; i < _controller.GetGameWorld().GetWorldMap().Length; i++)
                {
                    for (int j = 0; j < _controller.GetGameWorld().GetWorldMap()[i].Length; j++)
                    {
                        WorldMapStructure structure = _controller.GetGameWorld().GetWorldMap()[i][j];
                        if (structure != WorldMapStructure.None)
                        {
                            _controller.GetDatabaseManager().AddGameWorldStructure(structure, (i, j));
                        }
                    }
                }

                _controller.AddNotification("Game world structures saved.");

                Console.WriteLine("Quitting game...");
                Environment.Exit(0);
            } else if (key == ConsoleKey.N)
            {
                _controller.AddNotification("Game world structures not saved.");

                Console.WriteLine("Quitting game...");
                Environment.Exit(0);
            }
        }
    }
}