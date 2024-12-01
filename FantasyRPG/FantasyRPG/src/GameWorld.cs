public class GameWorld
{
    private static GameWorld? instance;

    public int time { get; set; }
    public string weather { get; set; }

    private WorldMapStructure[][] worldMap;
    private List<NPC> npcs;

    private Dictionary<Character, (int, int)> _characterLocations = new Dictionary<Character, (int, int)>();
    private Character _activeCharacter;

    private GameWorld()
    {
        time = 3600;
        weather = "sunny";

        // Initialize world map
        worldMap = new WorldMapStructure[][]
        {
            new WorldMapStructure[] { WorldMapStructure.None, WorldMapStructure.Dungeon, WorldMapStructure.None, WorldMapStructure.Town, WorldMapStructure.None },
            new WorldMapStructure[] { WorldMapStructure.None, WorldMapStructure.Village, WorldMapStructure.None, WorldMapStructure.None, WorldMapStructure.None },
            new WorldMapStructure[] { WorldMapStructure.None, WorldMapStructure.None, WorldMapStructure.None, WorldMapStructure.Dungeon, WorldMapStructure.None },
            new WorldMapStructure[] { WorldMapStructure.Town, WorldMapStructure.None, WorldMapStructure.None, WorldMapStructure.None, WorldMapStructure.Village },
            new WorldMapStructure[] { WorldMapStructure.None, WorldMapStructure.None, WorldMapStructure.Village, WorldMapStructure.None, WorldMapStructure.None }
        };

        // Initialize NPCs
        npcs = new List<NPC>
        {
            new NPC(1, "King/Queen", "Quest Giver", (0, 1)),
            new NPC(2, "Merchant", "Trader", (0, 3)),
            new NPC(3, "Villager", "Confederate", (1, 1)),
            new NPC(4, "King/Queen", "Quest Giver", (2, 3)),
            new NPC(5, "Merchant", "Trader", (3, 0)),
            new NPC(6, "Villager", "Confederate", (3, 4)),
            new NPC(7, "Villager", "Confederate", (4, 2))
        };
    }

    public static GameWorld GetInstance()
    {
        if (instance == null)
        {
            instance = new GameWorld();
        }
        return instance;
    }

    public WorldMapStructure[][] GetWorldMap()
    {
        return worldMap;
    }

    public void PrintLocation()
    {
        // Determine the current location of the active character
        (int, int) activeCharacterLocation = _characterLocations[_activeCharacter];

        // Determine and print the structure of the current location
        WorldMapStructure currentLocationStructure = worldMap[activeCharacterLocation.Item1][activeCharacterLocation.Item2];
        Console.WriteLine($"Current location structure: {currentLocationStructure}");

        // Determine and print the NPCs at the current location
        List<NPC> npcsAtLocation = npcs.FindAll(npc => npc.Location == activeCharacterLocation);
        Console.Write("NPCs at current location:");
        foreach (var npc in npcsAtLocation)
        {
            Console.Write($" {npc.Name} ({npc.Role})");
        }
        Console.WriteLine();
    }

    public void PrintMap()
    {
        int rowIterator = 0;

        foreach (var row in worldMap)
        {
            int cellIterator = 0;

            foreach (var cell in row)
            {
                Console.Write(cell.ToString().Substring(0, 1));

                // Write an X to the console if the active character is in the current row and cell
                (int, int) activeCharacterLocation = _characterLocations[_activeCharacter];
                if (activeCharacterLocation.Item1 == rowIterator && activeCharacterLocation.Item2 == cellIterator)
                {
                    Console.Write(" X");
                } else
                {
                    Console.Write("  ");
                }

                Console.Write(" | ");

                cellIterator++;
            }

            Console.WriteLine();

            rowIterator++;
        }
    }

    // Add character to the game world
    public void AddCharacter(Character character)
    {
        // Set starting location for character
        (int, int) startingLocation = (2, 2);

        // Add character to the game world
        _characterLocations.Add(character, startingLocation);
    }

    public Character GetActiveCharacter() { return _activeCharacter; }
    public void SetActiveCharacter(Character character) { _activeCharacter = character; }

    // Move character to new location
    public void MoveActiveCharacter((int, int) direction)
    {
        // Check if character exists in the game world
        if (_characterLocations.ContainsKey(_activeCharacter))
        {
            // Calculate new location
            (int, int) currentLocation = _characterLocations[_activeCharacter];
            (int, int) newLocation = (currentLocation.Item1 + direction.Item1, currentLocation.Item2 + direction.Item2);

            // Check if new location is within bounds of the world map
            if (newLocation.Item1 >= 0 && newLocation.Item1 < GetWorldMap().Length &&
                newLocation.Item2 >= 0 && newLocation.Item2 < GetWorldMap()[0].Length)
            {
                // Update character location
                _characterLocations[_activeCharacter] = newLocation;
            }
        }
    }
}