public class QuestManager : ISubject
{
    private List<IObserver> _observers = new List<IObserver>();
    public Quest? _currentQuest;

    public Quest? GetCurrentQuest() => _currentQuest;

    public void SetCurrentQuest(Quest quest)
    {
        _currentQuest = quest;
    }

    // Method to change the quest status and notify observers
    public void UpdateQuestStatus(int progress)
    {
        // Return if there is no active quest
        if (_currentQuest == null) return;

        // Update quest progress
        _currentQuest.Progress += progress;

        // Notify observers of the quest status change
        NotifyObservers();

        // Remove current quest if completed
        if (_currentQuest.Progress >= _currentQuest.Goal)
        {
            _currentQuest = null;
        }
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
        if (_currentQuest == null) return;

        foreach (var observer in _observers)
        {
            observer.Update(_currentQuest);
        }
    }
}
