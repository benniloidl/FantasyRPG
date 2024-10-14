public class RareItemFactory : ItemFactory
{
    public override Item CreateWeapon()
    {
        return new Weapon(20, WeaponType.Melee, ItemRarity.Rare);
    }

    public override Item CreatePotion()
    {
        return new Potion("Heal", 20, ItemRarity.Rare);
    }

    public override Item CreateArmor()
    {
        return new Armor(20, 20, ItemRarity.Rare);
    }
}