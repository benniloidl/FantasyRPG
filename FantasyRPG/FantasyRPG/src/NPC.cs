public class NPC : IObserver
{
    public string name { get; set; }
    public string role { get; set; }
    public int health { get; set; }

    public NPC(string name, string role, int health)
    {
        this.name = name;
        this.role = role;
        this.health = health;
    }

    public void Update(string questStatus)
    {
        Console.WriteLine($"{name} (NPC) has been notified of the quest status: {questStatus}");
    }
}