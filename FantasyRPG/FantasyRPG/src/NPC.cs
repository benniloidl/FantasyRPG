public class NPC
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
}