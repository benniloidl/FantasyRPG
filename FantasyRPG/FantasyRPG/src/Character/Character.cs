using System.Xml.Linq;

public class Character : IObserver
{
    public int Health { get; set; }
    public int Mana { get; set; }
    public int Strength { get; set; }
    public int Agility { get; set; }
    public int Speed { get; set; }

    private ICharacterState _characterState = new IdleState();

    private Inventory _inventory = new Inventory();
    private Weapon? _equippedWeapon;
    private DefensiveItem? _equippedDefensive;
    private UtilityItem? _equippedUtility;

    public void PerformAction()
    {
        _characterState.HandleState();
    }

    public ICharacterState getCharacterState()
    {
        return _characterState;
    }

    public void setCharacterState(ICharacterState characterState)
    {
        _characterState = characterState;
    }

    // When a quest is updated, check if the quest is completed
    public void Update(Quest quest)
    {
        if (quest.Progress >= quest.Goal)
        {
            Console.WriteLine($"Character has been notified that the quest has been completed!");

            // Add the quest reward to the character's inventory
            _inventory.AddItem(quest.Reward);
            Console.WriteLine($"Character has received the quest reward: {quest.Reward} ({quest.Reward.GetType})");

            // Delay for 1s
            System.Threading.Thread.Sleep(1000);

            return;
        }
    }

    public Inventory GetInventory() => _inventory;

    public Weapon? GetEquippedWeapon() => _equippedWeapon;
    public DefensiveItem? GetEquippedDefensive() => _equippedDefensive;
    public UtilityItem? GetEquippedUtility() => _equippedUtility;

    public void EquipItem(Item item)
    {
        ItemHandlerFactory.GetHandler(item).Equip(this, item);
    }

    public void UnequipItem(Item item)
    {
        ItemHandlerFactory.GetHandler(item).Unequip(this);
    }

    public void SetEquippedWeapon(Weapon weapon)
    {
        _equippedWeapon = weapon;
    }

    public void SetEquippedDefensive(DefensiveItem defensiveItem)
    {
        _equippedDefensive = defensiveItem;
    }
    
    public void SetEquippedUtility(UtilityItem utilityItem)
    {
        _equippedUtility = utilityItem;
    }
}