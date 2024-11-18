public class Character : IObserver
{
    public int health { get; set; }
    public int mana { get; set; }
    public int strength { get; set; }
    public int agility { get; set; }
    public int speed { get; set; }

    private IActionStrategy _actionStrategy;
    private ICharacterState _characterState = new IdleState();

    private Inventory _inventory = new Inventory();
    private Weapon? _equippedWeapon;
    private DefensiveItem? _equippedDefensive;
    private UtilityItem? _equippedUtility;

    public void PerformAction()
    {
        _characterState.HandleState();
    }

    public void setActionStrategy(IActionStrategy actionStrategy)
    {
        _actionStrategy = actionStrategy;
    }

    public void setCharacterState(ICharacterState characterState)
    {
        _characterState = characterState;
    }

    public void Update(string questStatus)
    {
        Console.WriteLine($"Character has been notified of the quest status: {questStatus}");
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