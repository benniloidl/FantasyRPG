public class Warrior : Character
{
    public int SwordDamage { get; set; }

    public Warrior()
    {
        Health = 100;
        Mana = 50;
        Strength = 10;
        Agility = 5;
        Speed = 5;

        SwordDamage = 20;
    }
}