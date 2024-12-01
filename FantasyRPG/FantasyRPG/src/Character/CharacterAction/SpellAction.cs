public class SpellAction : IActionStrategy
{
    public SpellAction()
    {
    }

    public void PerformAction()
    {
        Console.WriteLine("Character casts a magic spell.");
    }
}