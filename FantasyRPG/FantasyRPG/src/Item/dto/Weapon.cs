public class Weapon : Item
{
    public int damage { get; set; }
    public WeaponType weaponType { get; set; }

    public Weapon(int damage, WeaponType weaponType, ItemRarity itemRarity)
    {
        this.damage = damage;
        this.weaponType = weaponType;
        this.itemRarity = itemRarity;
    }
}