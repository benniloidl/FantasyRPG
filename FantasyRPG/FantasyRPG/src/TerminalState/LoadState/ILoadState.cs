public interface ILoadState : ITerminalState
{
    public ITerminalState? GetNextState();
    public void Quit();
}