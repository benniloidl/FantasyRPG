public class MagicalItemFactory : ItemFactory
{
    public override Item CreateWeapon()
    {
        return new Weapon(30, WeaponType.Melee, ItemRarity.Magical);
    }

    public override Item CreatePotion()
    {
        return new Potion("Heal", 30, ItemRarity.Magical);
    }

    public override Item CreateArmor()
    {
        return new Armor(30, 30, ItemRarity.Magical);
    }
}