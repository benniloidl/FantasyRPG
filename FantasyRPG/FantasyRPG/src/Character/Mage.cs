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