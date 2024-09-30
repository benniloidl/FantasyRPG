using System;

namespace FantasyRPG
{
    public class GameWorld
    {
        private static GameWorld? instance;

        public WorldMap map { get; set; }
        public NPC[] npcs { get; set; }
        public int time { get; set; }
        public string weather { get; set; }

        private GameWorld()
        {
            map = new WorldMap();
            npcs = [];
            time = 3600;
            weather = "sunny";
        }

        public static GameWorld GetInstance()
        {
            if (instance == null)
            {
                instance = new GameWorld();
            }
            return instance;
        }
    }

    public class WorldMap
    {
        public int[][] grid { get; set; }

        public WorldMap()
        {
            grid = new int[100][];
        }
    }

    public class NPC
    {
        public string name { get; set; }
        public string role { get; set; }
        public int health { get; set; }

        public NPC(string name, string role, int health)
        {
            this.name = name;
            this.role = role;
            this.health = health;
        }
    }

    public class Character
    {
        public int health { get; set; }
        public int mana { get; set; }
        public int strength { get; set; }
        public int agility { get; set; }
        public int speed { get; set; }
    }

    public class Warrior : Character
    {
        public int swordDamage { get; set; }

        public Warrior()
        {
            health = 100;
            mana = 50;
            strength = 10;
            agility = 5;
            speed = 5;

            swordDamage = 20;
        }
    }

    public class Mage : Character
    {
        public int crazyness { get; set; }

        public Mage()
        {
            health = 50;
            mana = 100;
            strength = 5;
            agility = 5;
            speed = 10;

            crazyness = 100;
        }
    }

    public class Archer : Character
    {
        public int amountOfArrows { get; set; }
        public Archer()
        {
            health = 75;
            mana = 75;
            strength = 5;
            agility = 10;
            speed = 5;

            amountOfArrows = 100;
        }
    }

    public class CharacterFactory
    {
        public Character CreateCharacter(string characterType)
        {
            if (characterType == "Warrior")
            {
                return new Warrior();
            }
            else if (characterType == "Mage")
            {
                return new Mage();
            }
            else if (characterType == "Archer")
            {
                return new Archer();
            }
            else
            {
                throw new ArgumentException("Invalid character type");
            }
        }
    }

    public class Program
    {
        public static void Main()
        {
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
        }
    }
}