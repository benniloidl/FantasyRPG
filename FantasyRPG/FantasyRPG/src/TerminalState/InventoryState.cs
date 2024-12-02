using System;

public class InventoryState : ITerminalState
{
    private readonly Controller _controller;
    private readonly ITerminalState _returnToState;

    private int _selectedInventoryIndex;
    private int _selectedEquipmentIndex;

    public InventoryState(Controller controller, ITerminalState returnToState)
    {
        _controller = controller;
        _returnToState = returnToState;

        // Initialize selected indexes to 0 (the first item)
        _selectedInventoryIndex = 0;
        _selectedEquipmentIndex = 0;
    }

    public void HandleState()
    {
        Console.Clear();

        Console.WriteLine("Inventory:");
        Console.WriteLine();
        Console.WriteLine("--------------------");
        Console.WriteLine();

        // Get items from active character's inventory
        List<Item> items = _controller.GetGameWorld().GetActiveCharacter().GetInventory().GetItems();

        // Print items in inventory
        for (int i = 0; i < items.Count; i++)
        {
            // Print selected item with a ">" symbol
            if (i == _selectedInventoryIndex)
            {
                Console.Write("> ");
            }

            Console.WriteLine(items[i].GetType());
        }

        // If inventory is empty, print a message
        if (items.Count == 0)
        {
            Console.WriteLine("Inventory is empty");
        }

        // Print equipment slots
        Console.WriteLine();
        Console.WriteLine("--------------------");
        Console.WriteLine();
        if (_selectedEquipmentIndex == 0)
        {
            Console.Write("> ");
        }
        Console.WriteLine("Equipped Weapon: " + _controller.GetGameWorld().GetActiveCharacter().GetEquippedWeapon()?.GetType());
        if (_selectedEquipmentIndex == 1)
        {
            Console.Write("> ");
        }
        Console.WriteLine("Equipped Defensive: " + _controller.GetGameWorld().GetActiveCharacter().GetEquippedDefensive()?.GetType());
        if (_selectedEquipmentIndex == 2)
        {
            Console.Write("> ");
        }
        Console.WriteLine("Equipped Utility: " + _controller.GetGameWorld().GetActiveCharacter().GetEquippedUtility()?.GetType());

        Console.WriteLine();
        Console.WriteLine("--------------------");
        Console.WriteLine();
        Console.WriteLine("Select an item using the arrow keys");
        Console.WriteLine("Press 'E' to equip the selected item and 'U' to unequip an item");
        Console.WriteLine("Press 'Q' to close the inventory");

        HandleInput();
    }

    void HandleInput()
    {
        // Read key input
        ConsoleKey key = Console.ReadKey().Key;

        // Exit the inventory state and return to the returnToState state using 'Q'
        if (key == ConsoleKey.Q)
            _controller.SetTerminalState(_returnToState);

        // Move the selection left using the left arrow key, selected index cannot be less than 0
        else if (key == ConsoleKey.LeftArrow)
            _selectedInventoryIndex = Math.Max(0, _selectedInventoryIndex - 1);

        // Move the selection right using the right arrow key, selected index cannot be greater than the number of items
        else if (key == ConsoleKey.RightArrow)
            _selectedInventoryIndex = Math.Min(_controller.GetGameWorld().GetActiveCharacter().GetInventory().GetItems().Count - 1, _selectedInventoryIndex + 1);

        // Move the currently selected equipment slot up using the arrow up key
        else if (key == ConsoleKey.UpArrow)
            _selectedEquipmentIndex = Math.Max(0, _selectedEquipmentIndex - 1);

        // Move the currently selected equipment slot down using the arrow down key
        else if (key == ConsoleKey.DownArrow)
            _selectedEquipmentIndex = Math.Min(2, _selectedEquipmentIndex + 1);

        // Equip the selected item using 'E'
        else if (key == ConsoleKey.E)
        {
            // Check if the inventory is empty
            if (_controller.GetGameWorld().GetActiveCharacter().GetInventory().GetItems().Count == 0)
            {
                return;
            }

            Item item = _controller.GetGameWorld().GetActiveCharacter().GetInventory().GetItems()[_selectedInventoryIndex];
            _controller.GetGameWorld().GetActiveCharacter().EquipItem(item);
        }

        // Unequip the selected item using 'U'
        else if (key == ConsoleKey.U)
        {
            // Check if the inventory is empty
            if (_controller.GetGameWorld().GetActiveCharacter().GetInventory().GetItems().Count == 0)
            {
                return;
            }

            // Determine which equipment slot should be unequipped
            switch (_selectedEquipmentIndex)
            {
                case 0:
                    _controller.GetGameWorld().GetActiveCharacter().UnequipItem(_controller.GetGameWorld().GetActiveCharacter().GetEquippedWeapon());
                    break;
                case 1:
                    _controller.GetGameWorld().GetActiveCharacter().UnequipItem(_controller.GetGameWorld().GetActiveCharacter().GetEquippedDefensive());
                    break;
                case 2:
                    _controller.GetGameWorld().GetActiveCharacter().UnequipItem(_controller.GetGameWorld().GetActiveCharacter().GetEquippedUtility());
                    break;
            }
        }
    }
}