public class ActionState : ICharacterState
{
    private IActionStrategy _actionStrategy;

    public ActionState(IActionStrategy actionStrategy)
    {
        _actionStrategy = actionStrategy;
    }

    public void HandleState()
    {
        _actionStrategy.PerformAction();
    }
}