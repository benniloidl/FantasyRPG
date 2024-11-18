public class Potion : UtilityItem
{
    public string effect { get; set; }
    public int duration { get; set; }

    public Potion(string effect, int duration, ItemRarity itemRarity)
    {
        this.effect = effect;
        this.duration = duration;
        this.itemRarity = itemRarity;
    }
}