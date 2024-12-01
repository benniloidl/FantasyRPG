public class Quest
{
    private Guid _questId;

    public string QuestName { get; }
    public string QuestDescription { get; }
    public int Progress { get; set; }
    public int Goal { get; }
    public Item Reward { get; }

    public Quest(string questName, string questDescription, int goal, Item reward)
    {
        _questId = Guid.NewGuid();
        QuestName = questName;
        QuestDescription = questDescription;
        Progress = 0;
        Goal = goal;
        Reward = reward;
    }
}