using System;

public class SaveState : ITerminalState
{
    private readonly Controller _controller;
    private List<Character> _characters;
    private Dictionary<Enemy, (int, int)> _enemies;

    public SaveState()
    {
        _controller = Controller.GetInstance();

        // Get characters and enemies from game world and store them in local variables (array copies that do not affect the originals)
        _characters = new List<Character>(_controller.GetGameWorld().GetCharacters());
        _enemies = new Dictionary<Enemy, (int, int)>(_controller.GetGameWorld().GetEnemyLocations());
    }

    public void PrintTerminal()
    {
        Console.WriteLine("Before closing the game, you can decide whether to save your progress.");
        Console.WriteLine();
        Console.WriteLine("--------------------");
        Console.WriteLine();
        Console.Write("Would you like to save ");

        // Check if saveObject is a character
        if (_characters.Count > 0)
        {
            Character character = _characters.First();
            Console.WriteLine($"character {character}");
            Console.Write($" ❤️ {_controller.GetGameWorld().GetActiveCharacter().Health}");
            Console.Write($" 🗡️ {_controller.GetGameWorld().GetActiveCharacter().GetEquippedWeapon()?.ToString() ?? "None"}");
            Console.Write($" 🛡️ {_controller.GetGameWorld().GetActiveCharacter().GetEquippedDefensive()?.ToString() ?? "None"}");
            Console.WriteLine($" 🧪 {_controller.GetGameWorld().GetActiveCharacter().GetEquippedUtility()?.ToString() ?? "None"}");
            Console.WriteLine($"and {character.GetInventory().GetItems().Count()} items in their inventory?");
        }
        else if (_enemies.Count > 0)
        {
            Enemy enemy = _enemies.First().Key;
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

        if (_characters.Count > 0)
        {
            Character character = _characters.First();

            if (key == ConsoleKey.Y)
            {
                // Save character and remove from the list of characters to be asked to save
                _controller.GetDatabaseManager().AddCharacter(character);
                _characters.Remove(character);
                _controller.AddNotification($"Character {character} saved.");
            }
            else if (key == ConsoleKey.N)
            {
                // Just remove character from the list of characters to be asked to save
                _characters.Remove(character);
                _controller.AddNotification($"Character {character} not saved.");
            }
        }
        else if (_enemies.Count > 0)
        {
            Enemy enemy = _enemies.First().Key;
            (int, int) enemyLocation = _enemies.GetValueOrDefault(enemy);

            if (key == ConsoleKey.Y)
            {
                // Save enemy and remove from the list of enemies to be asked to save
                _controller.GetDatabaseManager().AddEnemy(enemy, enemyLocation);
                _enemies.Remove(enemy);
                _controller.AddNotification($"Enemy {enemy} saved.");
            }
            else if (key == ConsoleKey.N)
            {
                // Just remove enemy from the list of enemies to be asked to save
                _enemies.Remove(enemy);
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

                Console.WriteLine("Game world structures saved.");

                Console.WriteLine("Quitting game...");
                Environment.Exit(0);
            } else if (key == ConsoleKey.N)
            {
                Console.WriteLine("Game world structures not saved.");

                Console.WriteLine("Quitting game...");
                Environment.Exit(0);
            }
        }
    }
}