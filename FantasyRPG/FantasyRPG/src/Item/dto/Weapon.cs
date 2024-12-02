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

    // Change toString method to return e.g. "Legendary Melee Weapon (10 Damage)"
    public override string ToString()
    {
        return $"{itemRarity} {weaponType} Weapon ({damage} Damage)";
    }
}