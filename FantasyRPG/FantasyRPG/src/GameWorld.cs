public class GameWorld
{
    private static GameWorld? instance;

    public int time { get; set; }
    public string weather { get; set; }

    private WorldMapStructure[][] worldMap;
    private List<NPC> npcs;

    private Dictionary<Character, (int, int)> _characterLocations = new Dictionary<Character, (int, int)>();
    private Dictionary<Enemy, (int, int)> _enemyLocations = new Dictionary<Enemy, (int, int)>();
    private Character _activeCharacter;

    private QuestManager _questManager = new QuestManager();

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

        // Initialize NPCs with quests
        npcs = new List<NPC>
        {
            new NPC(1, "King/Queen", "Quest Giver", (0, 1), new Quest("Attack Mastery", "Perform 3 Attacks", 3, new Item())),
            new NPC(2, "Merchant", "Trader", (0, 3), new Quest("Attack Mastery", "Perform 3 Attacks", 3, new Item())),
            new NPC(3, "Villager", "Confederate", (1, 1), new Quest("Attack Mastery", "Perform 3 Attacks", 3, new Item())),
            new NPC(4, "King/Queen", "Quest Giver", (2, 3), new Quest("Attack Mastery", "Perform 3 Attacks", 3, new Item())),
            new NPC(5, "Merchant", "Trader", (3, 0), new Quest("Attack Mastery", "Perform 3 Attacks", 3, new Item())),
            new NPC(6, "Villager", "Confederate", (3, 4), new Quest("Attack Mastery", "Perform 3 Attacks", 3, new Item())),
            new NPC(7, "Villager", "Confederate", (4, 2), new Quest("Attack Mastery", "Perform 3 Attacks", 3, new Item()))
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

    // Add character to the game world
    public void AddCharacter(Character character)
    {
        // Set starting location for character
        (int, int) startingLocation = (2, 2);

        // Add character to the game world
        _characterLocations.Add(character, startingLocation);

        // Add character to the quest manager as an observer
        _questManager.RegisterObserver(character);
    }

    // Add enemy to the game world
    public void AddEnemy(Enemy enemy, (int, int) location) => _enemyLocations.Add(enemy, location);

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

    public void PrintLocation()
    {
        // Determine the current location of the active character
        (int, int) activeCharacterLocation = _characterLocations[_activeCharacter];

        // Determine and print the structure of the current location
        WorldMapStructure currentLocationStructure = worldMap[activeCharacterLocation.Item1][activeCharacterLocation.Item2];
        Console.WriteLine($"Current location structure: {currentLocationStructure}");
    }

    // Determine and print the FIRST NPC in the current location of the active character
    private NPC? GetNPCForInteraction() => npcs.Find(npc => npc.Location == _characterLocations[_activeCharacter]);

    public void PrintAvailableQuest()
    {
        NPC? npc = GetNPCForInteraction();
        if (npc == null) return;

        Console.WriteLine($"NPC at current location: {npc.Name} ({npc.Role})");

        if (npc.Quest.Progress >= npc.Quest.Goal)
        {
            Console.WriteLine("Quest is completed!");
            return;
        }
        Console.WriteLine($"Available Quest: {npc.Quest.QuestName} - {npc.Quest.QuestDescription}");
    }

    // Check if there is a quest available at the current location
    public bool IsQuestAvailable()
    {
        NPC? npc = GetNPCForInteraction();
        return npc != null &&                                   // Check if there is a NPC
            npc.Quest.Progress < npc.Quest.Goal &&              // Check if the quest is not completed
            npc.Quest != _questManager.GetCurrentQuest();       // Check if the quest is not the current quest already
    }

    // Accept quest if there is a quest available at the current location
    public void AcceptQuest()
    {
        NPC? npc = GetNPCForInteraction();
        if (npc == null) return;

        if (IsQuestAvailable()) _questManager.SetCurrentQuest(npc.Quest);
    }

    // Update quest status if there is an active quest
    public void UpdateQuestStatus(int progress = 1)
    {
        if (_questManager._currentQuest == null) return;
        _questManager.UpdateQuestStatus(progress);
    }

    // Print current quest if active
    public void PrintCurrentQuest()
    {
        var currentQuest = _questManager.GetCurrentQuest();
        if (currentQuest == null)
        {
            Console.WriteLine("No active quest");
            return;
        }

        Console.WriteLine($"Current Quest: {currentQuest.QuestName} - {currentQuest.QuestDescription} ({currentQuest.Progress} / {currentQuest.Goal})");
    }

    // Get enemy at the current location of the active character
    public Enemy? GetEnemyAtCurrentLocation()
    {
        (int, int) activeCharacterLocation = _characterLocations[_activeCharacter];
        return _enemyLocations.FirstOrDefault(enemy => enemy.Value == activeCharacterLocation).Key;
    }

    // Print the game world map
    public void PrintMap()
    {
        int rowIterator = 0;

        foreach (var row in worldMap)
        {
            int cellIterator = 0;

            foreach (var cell in row)
            {
                Console.Write(cell.ToString().Substring(0, 1));
                // Write 'E' to the console if there is an enemy in the current row and cell
                if (_enemyLocations.ContainsValue((rowIterator, cellIterator)))
                {
                    Console.Write(" E");
                }
                else
                {
                    Console.Write("  ");
                }

                // Write 'X' to the console if the active character is in the current row and cell
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
}