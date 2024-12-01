public class Archer : Character
{
    public int AmountOfArrows { get; set; }
    public Archer()
    {
        Health = 75;
        Mana = 75;
        Strength = 5;
        Agility = 10;
        Speed = 5;

        AmountOfArrows = 100;
    }
}