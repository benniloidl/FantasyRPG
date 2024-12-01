using System;

namespace FantasyRPG
{
    internal class Program
    {
        public static void Main()
        {
            // Instantiate game world (Singleton)
            GameWorld gameWorld = GameWorld.GetInstance();

            // Create characters
            CharacterFactory characterFactory = new CharacterFactory();
            Character player = characterFactory.CreateCharacter("Warrior");
            Character enemy = characterFactory.CreateCharacter("Archer");

            // Add characters to the game world and set the player to be the active character
            gameWorld.AddCharacter(player);
            gameWorld.AddCharacter(enemy);
            gameWorld.SetActiveCharacter(player);

            var controller = new Controller();

            // Set up key mappings
            controller.SetCommand(ConsoleKey.A, new AttackCommand(player, enemy));
            controller.SetCommand(ConsoleKey.D, new DefendCommand(player));
            controller.SetCommand(ConsoleKey.H, new HealCommand(player));
            controller.SetCommand(ConsoleKey.M, new MoveCommand(player));

            // Main loop to handle input
            while (true)
            {
                controller.HandleInput();
            }
        }
    }
}