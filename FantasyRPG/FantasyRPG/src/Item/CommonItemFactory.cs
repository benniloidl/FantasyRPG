public class CommonItemFactory : ItemFactory
{
    public override Item CreateWeapon()
    {
        return new Weapon(10, WeaponType.Melee, ItemRarity.Common);
    }

    public override Item CreatePotion()
    {
        return new Potion("Heal", 10, ItemRarity.Common);
    }

    public override Item CreateArmor()
    {
        return new Armor(10, 10, ItemRarity.Common);
    }
}