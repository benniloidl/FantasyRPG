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