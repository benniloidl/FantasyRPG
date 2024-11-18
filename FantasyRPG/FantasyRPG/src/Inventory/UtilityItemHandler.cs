public class UtilityItemHandler : IItemHandler
{
    public void Equip(Character character, Item item)
    {
        if (item is not UtilityItem utilityItem)
        {
            throw new ArgumentException("Item is not a utility item");
        }

        if (!character.GetInventory().GetItems().Contains(utilityItem))
        {
            Console.WriteLine("Character does not have this utility item");
            return;
        }

        if (character.GetEquippedUtility() != null)
        {
            character.GetInventory().AddItem(character.GetEquippedUtility());
        }

        character.SetEquippedUtility(utilityItem);
        character.GetInventory().RemoveItem(utilityItem);
        Console.WriteLine($"Utility item {utilityItem} ({utilityItem.itemRarity }) has been equipped");
    }

    public void Unequip(Character character)
    {
        if (character.GetEquippedUtility() == null)
        {
            Console.WriteLine("Character does not have a utility item equipped");
            return;
        }

        character.GetInventory().AddItem(character.GetEquippedUtility());
        Console.WriteLine($"Utility item { character.GetEquippedUtility() } ({ character.GetEquippedUtility().itemRarity }) has been unequipped");
        character.SetEquippedUtility(null);
    }
}