public class Armor : Item
{
    public int defense { get; set; }
    public int durability { get; set; }

    public Armor(int defense, int durability, ItemRarity itemRarity)
    {
        this.defense = defense;
        this.durability = durability;
        this.itemRarity = itemRarity;
    }
}