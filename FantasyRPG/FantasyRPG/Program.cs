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
            Character player1 = characterFactory.CreateCharacter("Warrior");
            Character player2 = characterFactory.CreateCharacter("Archer");

            // Add characters to the game world and set the player to be the active character
            gameWorld.AddCharacter(player1);
            gameWorld.AddCharacter(player2);
            gameWorld.SetActiveCharacter(player1);

            // Create enemy and add to the game world (3, 1)
            DragonFactory dragonFactory = new DragonFactory();
            Enemy dragon = dragonFactory.CreateEnemy(EnemyRank.Normal);
            gameWorld.AddEnemy(dragon, (3, 1));

            var controller = new Controller();

            // Main loop to handle input
            while (true)
            {
                controller.HandleInput();
            }
        }
    }
}