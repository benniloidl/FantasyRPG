using System;

namespace FantasyRPG
{
    internal class Program
    {
        public static void Main()
        {
            /*
            GameWorld gameWorld = GameWorld.GetInstance();
            Console.WriteLine("GameWorld instance created");
            Console.WriteLine(gameWorld.weather);
            gameWorld.weather = "rainy";
            GameWorld gameWorld2 = GameWorld.GetInstance();
            Console.WriteLine(gameWorld2.weather);

            CharacterFactory characterFactory = new CharacterFactory();
            Character warrior = characterFactory.CreateCharacter("Warrior");
            Console.WriteLine(warrior.health);
            Console.WriteLine(((Warrior)warrior).swordDamage);

            CommonItemFactory commonItemFactory = new CommonItemFactory();
            Item weapon = commonItemFactory.CreateWeapon();
            Console.WriteLine(((Weapon)weapon).damage);

            LegendaryItemFactory legendaryItemFactory = new LegendaryItemFactory();
            Item potion = legendaryItemFactory.CreatePotion();
            Console.WriteLine(((Potion)potion).duration);

            warrior.PerformAction();

            IActionStrategy actionStrategy = new RangedAction();
            warrior.setActionStrategy(actionStrategy);
            warrior.setCharacterState(new ActionState(actionStrategy));
            warrior.PerformAction();

            warrior.setCharacterState(new DefendingState());
            warrior.PerformAction();
            */

            /*
            IEnemyFactory slimeFactory = new SlimeFactory();
            IEnemyFactory goblinFactory = new GoblinFactory();
            IEnemyFactory dragonFactory = new DragonFactory();

            Enemy slime = slimeFactory.CreateEnemy(EnemyRank.Elite);
            Enemy goblin = goblinFactory.CreateEnemy(EnemyRank.Normal);
            Enemy dragon = dragonFactory.CreateEnemy(EnemyRank.Boss);

            slime.Move();
            slime.Attack();

            goblin.Move();
            goblin.Attack();

            dragon.Move();
            dragon.Attack();
            */

            /*
            // Create QuestManager (Subject)
            QuestManager questManager = new QuestManager();

            // Create Characters (Observers)
            CharacterFactory characterFactory = new CharacterFactory();
            IObserver player = characterFactory.CreateCharacter("Warrior");
            
            IObserver npc = new NPC("Old Wise Man", "Useless", 15);

            // Register Observers
            questManager.RegisterObserver(player);
            questManager.RegisterObserver(npc);

            // Update quest status
            questManager.UpdateQuestStatus("Quest Started");
            questManager.UpdateQuestStatus("Quest Completed");

            // Unregister player
            questManager.UnregisterObserver(player);

            // Update quest status again
            questManager.UpdateQuestStatus("Quest Failed");
            */

            /*
            CharacterFactory characterFactory = new CharacterFactory();
            Character player = characterFactory.CreateCharacter("Warrior");
            Character enemy = characterFactory.CreateCharacter("Archer");

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
            */

            CharacterFactory characterFactory = new CharacterFactory();
            Character player = characterFactory.CreateCharacter("Warrior");

            Inventory inventory = player.GetInventory();
            
            CommonItemFactory commonItemFactory = new CommonItemFactory();
            Item weapon = commonItemFactory.CreateWeapon();

            inventory.AddItem(weapon);

            Console.WriteLine("Items in inventory:");
            inventory.GetItems().ForEach(item => Console.WriteLine($"{ item } ({ item.itemRarity })"));

            Console.WriteLine($"Equipped weapon: { player.GetEquippedWeapon() }");

            Console.WriteLine("\nEquipping weapon...\n");
            player.EquipItem(weapon);

            Console.WriteLine($"Equipped weapon: { player.GetEquippedWeapon() }");

            Console.WriteLine("Items in inventory:");
            inventory.GetItems().ForEach(item => Console.WriteLine($"{item} ({item.itemRarity})"));

            Console.WriteLine("\nUnequipping weapon...\n");
            player.UnequipItem(weapon);

            Console.WriteLine($"Equipped weapon: {player.GetEquippedWeapon()}");

            Console.WriteLine("Items in inventory:");
            inventory.GetItems().ForEach(item => Console.WriteLine($"{item} ({item.itemRarity})"));
        }
    }
}