public class DefensiveItemHandler : IItemHandler
{
    public void Equip(Character character, Item item)
    {
        if (item is not DefensiveItem defensiveItem)
        {
            throw new ArgumentException("Item is not a defensive item");
        }

        if (!character.GetInventory().GetItems().Contains(defensiveItem)) return;

        if (character.GetEquippedDefensive() != null)
        {
            character.GetInventory().AddItem(character.GetEquippedDefensive());
        }

        character.SetEquippedDefensive(defensiveItem);
        character.GetInventory().RemoveItem(defensiveItem);
    }

    public void Unequip(Character character)
    {
        if (character.GetEquippedDefensive() == null) return;

        character.GetInventory().AddItem(character.GetEquippedDefensive());
        character.SetEquippedDefensive(null);
    }
}