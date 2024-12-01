public class GameWorld
{
    private static GameWorld? instance;

    public WorldMap map { get; set; }
    public int time { get; set; }
    public string weather { get; set; }

    private GameWorld()
    {
        map = new WorldMap();
        time = 3600;
        weather = "sunny";
    }

    public static GameWorld GetInstance()
    {
        if (instance == null)
        {
            instance = new GameWorld();
        }
        return instance;
    }
}