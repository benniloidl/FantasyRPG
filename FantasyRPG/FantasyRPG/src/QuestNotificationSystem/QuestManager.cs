public class QuestManager : ISubject
{
    private List<IObserver> _observers = new List<IObserver>();
    private string _questStatus;

    // Method to change the quest status and notify observers
    public void UpdateQuestStatus(string status)
    {
        _questStatus = status;
        NotifyObservers();
    }

    // Register an observer
    public void RegisterObserver(IObserver observer)
    {
        _observers.Add(observer);
    }

    // Unregister an observer
    public void UnregisterObserver(IObserver observer)
    {
        _observers.Remove(observer);
    }

    // Notify all registered observers of a quest status change
    public void NotifyObservers()
    {
        foreach (var observer in _observers)
        {
            observer.Update(_questStatus);
        }
    }
}
