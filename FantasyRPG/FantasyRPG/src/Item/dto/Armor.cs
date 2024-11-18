public class Armor : DefensiveItem
{
    public int durability { get; set; }

    public Armor(int defense, int durability, ItemRarity itemRarity)
    {
        this.durability = durability;
        this.defense = defense;
        this.itemRarity = itemRarity;
    }
}