public class Character
{
    public int health { get; set; }
    public int mana { get; set; }
    public int strength { get; set; }
    public int agility { get; set; }
    public int speed { get; set; }

    private IActionStrategy _actionStrategy;
    private ICharacterState _characterState = new IdleState();

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
}