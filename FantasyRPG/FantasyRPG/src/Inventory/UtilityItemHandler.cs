public class UtilityItemHandler : IItemHandler
{
    public void Equip(Character character, Item item)
    {
        if (item is not UtilityItem utilityItem)
        {
            throw new ArgumentException("Item is not a utility item");
        }

        if (!character.GetInventory().GetItems().Contains(utilityItem)) return;

        if (character.GetEquippedUtility() != null)
        {
            character.GetInventory().AddItem(character.GetEquippedUtility());
        }

        character.SetEquippedUtility(utilityItem);
        character.GetInventory().RemoveItem(utilityItem);
    }

    public void Unequip(Character character)
    {
        if (character.GetEquippedUtility() == null) return;

        character.GetInventory().AddItem(character.GetEquippedUtility());
        character.SetEquippedUtility(null);
    }
}