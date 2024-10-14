using System;

namespace FantasyRPG
{
    internal class Program
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

            CommonItemFactory commonItemFactory = new CommonItemFactory();
            Item weapon = commonItemFactory.CreateWeapon();
            Console.WriteLine(((Weapon)weapon).damage);

            LegendaryItemFactory legendaryItemFactory = new LegendaryItemFactory();
            Item potion = legendaryItemFactory.CreatePotion();
            Console.WriteLine(((Potion)potion).duration);
        }
    }
}