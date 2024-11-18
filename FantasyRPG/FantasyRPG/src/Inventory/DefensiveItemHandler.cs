public class DefensiveItemHandler : IItemHandler
{
    public void Equip(Character character, Item item)
    {
        if (item is not DefensiveItem defensiveItem)
        {
            throw new ArgumentException("Item is not a defensive item");
        }

        if (!character.GetInventory().GetItems().Contains(defensiveItem))
        {
            Console.WriteLine("Character does not have this defensive item");
            return;
        }

        if (character.GetEquippedDefensive() != null)
        {
            character.GetInventory().AddItem(character.GetEquippedDefensive());
        }

        character.SetEquippedDefensive(defensiveItem);
        character.GetInventory().RemoveItem(defensiveItem);
        Console.WriteLine($"Defensive item {defensiveItem} ({defensiveItem.itemRarity }) has been equipped");
    }

    public void Unequip(Character character)
    {
        if (character.GetEquippedDefensive() == null)
        {
            Console.WriteLine("Character does not have a defensive item equipped");
            return;
        }

        character.GetInventory().AddItem(character.GetEquippedDefensive());
        Console.WriteLine($"Defensive item { character.GetEquippedDefensive() } ({ character.GetEquippedDefensive().itemRarity }) has been unequipped");
        character.SetEquippedDefensive(null);
    }
}