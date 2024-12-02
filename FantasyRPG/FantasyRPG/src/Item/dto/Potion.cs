public class Potion : UtilityItem
{
    public string Effect { get; set; }
    public int Duration { get; set; }

    public Potion(string effect, int duration, ItemRarity itemRarity)
    {
        Effect = effect;
        Duration = duration;
        ItemRarity = itemRarity;
    }
}