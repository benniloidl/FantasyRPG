public class WorldMap
{
    private string[][] worldMap;
    private List<NPC> npcs;

    public WorldMap()
    {
        // Initialize world map (🏔️ town, 🛣️ village, 🏰 dungeon)
        worldMap = new string[][]
        {
            new string[] { "🌳", "🏰", "🌳", "🏔️", "🌳" },
            new string[] { "🌳", "🛣️", "🌳", "🌳", "🌳" },
            new string[] { "🌳", "🌳", "🌳", "🏰", "🌳" },
            new string[] { "🏔️", "🌳", "🌳", "🌳", "🛣️" },
            new string[] { "🌳", "🌳", "🛣️", "🌳", "🌳" }
        };

        // Initialize NPCs
        npcs = new List<NPC>
        {
            new NPC(1, "King/Queen", "Quest Giver", (0, 1)),
            new NPC(2, "Merchant", "Trader", (0, 3)),
            new NPC(3, "Villager", "Confederate", (1, 1)),
            new NPC(4, "King/Queen", "Quest Giver", (2, 3)),
            new NPC(4, "Merchant", "Trader", (3, 0)),
            new NPC(4, "Villager", "Confederate", (3, 4)),
            new NPC(4, "Villager", "Confederate", (4, 2))
        };
    }
}