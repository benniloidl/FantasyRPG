using System.Data;
using System.Xml.Linq;

public class NPC : IObserver
{
    public int Id { get; }
    public string Name { get; }
    public string Role { get; }
    public (int, int) Location { get; }
    public Quest Quest { get; set; }

    public NPC(int id, string name, string role, (int, int) location, Quest quest)
    {
        Id = id;
        Name = name;
        Role = role;
        Location = location;
        Quest = quest;
    }

    public void Update(Quest quest)
    {
        if (quest.Progress >= quest.Goal)
        {
            Console.WriteLine($"{Name} (NPC) has been notified that the quest has been completed!");
            return;
        }
        Console.WriteLine($"{Name} (NPC) has been notified of the quest status: {quest.Progress} / {quest.Goal}");
    }
}