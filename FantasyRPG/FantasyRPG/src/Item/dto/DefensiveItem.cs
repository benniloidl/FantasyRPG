using static System.Net.Mime.MediaTypeNames;

public class DefensiveItem : Item
{
    public int Defense { get; set; }

    // Change toString method to return e.g. "Legendary Armor (10 Defense)"
    public override string ToString()
    {
        return $"{ItemRarity} Weapon ({Defense} Defense)";
    }
}