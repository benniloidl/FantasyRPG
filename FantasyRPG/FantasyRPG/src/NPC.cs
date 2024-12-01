using System.Data;
using System.Xml.Linq;

public class NPC : IObserver
{
    public int Id { get; }
    public string Name { get; }
    public string Role { get; }
    public (int, int) Location { get; }

    public NPC(int id, string name, string role, (int, int) location)
    {
        Id = id;
        Name = name;
        Role = role;
        Location = location;
    }

    public void Update(string questStatus)
    {
        Console.WriteLine($"{Name} (NPC) has been notified of the quest status: {questStatus}");
    }
}