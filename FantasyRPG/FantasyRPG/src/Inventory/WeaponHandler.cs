public class WeaponHandler : IItemHandler
{
    public void Equip(Character character, Item item)
    {
        if (item is not Weapon weapon)
        {
            throw new ArgumentException("Item is not a weapon");
        }

        if (!character.GetInventory().GetItems().Contains(weapon)) return;

        if (character.GetEquippedWeapon() != null)
        {
            character.GetInventory().AddItem(character.GetEquippedWeapon());
        }

        character.SetEquippedWeapon(weapon);
        character.GetInventory().RemoveItem(weapon);
    }

    public void Unequip(Character character)
    {
        if (character.GetEquippedWeapon() == null) return;

        character.GetInventory().AddItem(character.GetEquippedWeapon());
        character.SetEquippedWeapon(null);
    }
}