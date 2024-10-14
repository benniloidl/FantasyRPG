public class LegendaryItemFactory : ItemFactory
{
    public override Item CreateWeapon()
    {
        return new Weapon(40, WeaponType.Melee, ItemRarity.Legendary);
    }

    public override Item CreatePotion()
    {
        return new Potion("Heal", 40, ItemRarity.Legendary);
    }

    public override Item CreateArmor()
    {
        return new Armor(40, 40, ItemRarity.Legendary);
    }
}