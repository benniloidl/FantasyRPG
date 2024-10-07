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