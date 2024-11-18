public class WeaponHandler : IItemHandler
{
    public void Equip(Character character, Item item)
    {
        if (item is not Weapon weapon)
        {
            throw new ArgumentException("Item is not a weapon");
        }

        if (!character.GetInventory().GetItems().Contains(weapon))
        {
            Console.WriteLine("Character does not have this weapon");
            return;
        }

        if (character.GetEquippedWeapon() != null)
        {
            character.GetInventory().AddItem(character.GetEquippedWeapon());
        }

        character.SetEquippedWeapon(weapon);
        character.GetInventory().RemoveItem(weapon);
        Console.WriteLine($"Weapon { weapon } ({ weapon.itemRarity }) has been equipped");
    }

    public void Unequip(Character character)
    {
        if (character.GetEquippedWeapon() == null)
        {
            Console.WriteLine("Character does not have a weapon equipped");
            return;
        }

        character.GetInventory().AddItem(character.GetEquippedWeapon());
        Console.WriteLine($"Weapon { character.GetEquippedWeapon() } ({ character.GetEquippedWeapon().itemRarity }) has been unequipped");
        character.SetEquippedWeapon(null);
    }
}