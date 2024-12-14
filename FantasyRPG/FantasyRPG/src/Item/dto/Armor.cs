public class Armor : DefensiveItem
{
    public int Durability { get; set; }

    public Armor(int defense, int durability, ItemRarity itemRarity)
    {
        Durability = durability;
        Defense = defense;
        ItemRarity = itemRarity;
    }
}