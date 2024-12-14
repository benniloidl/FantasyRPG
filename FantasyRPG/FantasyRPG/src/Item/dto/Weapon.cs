public class Weapon : Item
{
    public int Damage { get; set; }
    public WeaponType WeaponType { get; set; }

    public Weapon(int damage, WeaponType weaponType, ItemRarity itemRarity)
    {
        Damage = damage;
        WeaponType = weaponType;
        ItemRarity = itemRarity;
    }

    // Change toString method to return e.g. "Legendary Melee Weapon (10 Damage)"
    public override string ToString()
    {
        return $"{ItemRarity} {WeaponType} Weapon ({Damage} Damage)";
    }
}