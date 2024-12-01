public class Mage : Character
{
    public int Crazyness { get; set; }

    public Mage()
    {
        Health = 50;
        Mana = 100;
        Strength = 5;
        Agility = 5;
        Speed = 10;

        Crazyness = 100;
    }
}