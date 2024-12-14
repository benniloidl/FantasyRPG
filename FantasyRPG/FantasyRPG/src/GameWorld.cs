public class GameWorld
{
    private static GameWorld? instance;

    public int Time { get; set; }
    public string Weather { get; set; }

    private WorldMapStructure[][] _worldMap;
    private List<NPC> _npcs;

    private (int, int) _location;
    private List<Character> _characters = new List<Character>();
    private Character _activeCharacter;
    private Dictionary<Enemy, (int, int)> _enemyLocations = new Dictionary<Enemy, (int, int)>();

    private QuestManager _questManager = new QuestManager();

    private GameWorld()
    {
        Time = 3600;
        Weather = "sunny";

        // Initialize world map
        _worldMap = new WorldMapStructure[][]
        {
            new WorldMapStructure[] { WorldMapStructure.None, WorldMapStructure.Dungeon, WorldMapStructure.None, WorldMapStructure.Town, WorldMapStructure.None },
            new WorldMapStructure[] { WorldMapStructure.None, WorldMapStructure.Village, WorldMapStructure.None, WorldMapStructure.None, WorldMapStructure.None },
            new WorldMapStructure[] { WorldMapStructure.None, WorldMapStructure.None, WorldMapStructure.None, WorldMapStructure.Dungeon, WorldMapStructure.None },
            new WorldMapStructure[] { WorldMapStructure.Town, WorldMapStructure.None, WorldMapStructure.None, WorldMapStructure.None, WorldMapStructure.Village },
            new WorldMapStructure[] { WorldMapStructure.None, WorldMapStructure.None, WorldMapStructure.Village, WorldMapStructure.None, WorldMapStructure.None }
        };

        // Random quest reward item
        LegendaryItemFactory legendaryItemFactory = new LegendaryItemFactory();
        Item legendaryItem = legendaryItemFactory.CreateWeapon();

        // Initialize NPCs with quests
        _npcs = new List<NPC>
        {
            new NPC(1, "King/Queen", "Quest Giver", (0, 1), new Quest("Attack Mastery", "Perform 3 Attacks", 3, legendaryItem)),
            new NPC(2, "Merchant", "Trader", (0, 3), new Quest("Attack Mastery", "Perform 3 Attacks", 3, legendaryItem)),
            new NPC(3, "Villager", "Confederate", (1, 1), new Quest("Attack Mastery", "Perform 3 Attacks", 3, legendaryItem)),
            new NPC(4, "King/Queen", "Quest Giver", (2, 3), new Quest("Attack Mastery", "Perform 3 Attacks", 3, legendaryItem)),
            new NPC(5, "Merchant", "Trader", (3, 0), new Quest("Attack Mastery", "Perform 3 Attacks", 3, legendaryItem)),
            new NPC(6, "Villager", "Confederate", (3, 4), new Quest("Attack Mastery", "Perform 3 Attacks", 3, legendaryItem)),
            new NPC(7, "Villager", "Confederate", (4, 2), new Quest("Attack Mastery", "Perform 3 Attacks", 3, legendaryItem))
        };

        // Set the starting location
        _location = (2, 2);
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
        return _worldMap;
    }

    // Add character to the game world
    public void AddCharacter(Character character)
    {
        // Add character to the game world
        _characters.Add(character);

        // Add character to the quest manager as an observer
        _questManager.RegisterObserver(character);
    }

    public List<Character> GetCharacters() => _characters;

    // Check if there is more than one character in the game world
    public bool HasMoreThanOneCharacter() => _characters.Count > 1;

    // Change active character to the next character in the game world
    public void ChangeActiveCharacter()
    {
        if (!HasMoreThanOneCharacter()) return;

        // Find the next character in the game world
        Character character = _characters.SkipWhile(character => character != _activeCharacter).Skip(1).FirstOrDefault() ?? _characters.First();

        SetActiveCharacter(character);
    }

    public Character GetActiveCharacter() { return _activeCharacter; }
    public void SetActiveCharacter(Character character) { _activeCharacter = character; }

    // Add enemy to the game world
    public void AddEnemy(Enemy enemy, (int, int) location) => _enemyLocations.Add(enemy, location);

    // Get enemy at the current location of the active character
    public Enemy? GetEnemyAtCurrentLocation() => _enemyLocations.FirstOrDefault(enemy => enemy.Value == _location).Key;

    // Remove defeated enemies (health <= 0) from the game world
    public void RemoveDefeatedEnemies()
    {
        foreach (var enemy in _enemyLocations.Where(enemy => enemy.Key.Health <= 0).ToList())
        {
            _enemyLocations.Remove(enemy.Key);
        }
    }

    // Move to new location
    public void Move((int, int) direction)
    {
        // Calculate new location
        (int, int) newLocation = (_location.Item1 + direction.Item1, _location.Item2 + direction.Item2);

        // Check if new location is within bounds of the world map
        if (newLocation.Item1 >= 0 && newLocation.Item1 < GetWorldMap().Length &&
            newLocation.Item2 >= 0 && newLocation.Item2 < GetWorldMap()[0].Length)
        {
            // Update location
            _location = newLocation;
        }
    }

    public void PrintLocation()
    {
        // Determine and print the structure of the current location
        WorldMapStructure currentLocationStructure = _worldMap[_location.Item1][_location.Item2];
        Console.WriteLine($"Current location structure: {currentLocationStructure}");
    }

    // Determine and print the FIRST NPC in the current location of the active character
    private NPC? GetNPCForInteraction() => _npcs.Find(npc => npc.Location == _location);

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

    // Print the game world map
    public void PrintMap()
    {
        int rowIterator = 0;

        foreach (var row in _worldMap)
        {
            int cellIterator = 0;

            foreach (var cell in row)
            {
                // Indicate whether there is an enemy in the current row and cell
                if (_enemyLocations.ContainsValue((rowIterator, cellIterator)))
                {
                    Console.Write(" 👻");
                }
                else
                {
                    Console.Write("   ");
                }

                switch (cell)
                {
                    case WorldMapStructure.Town:
                        Console.Write(" 🏠 ");
                        break;
                    case WorldMapStructure.Village:
                        Console.Write(" 💒 ");
                        break;
                    case WorldMapStructure.Dungeon:
                        Console.Write(" 🏰 ");
                        break;
                    default:
                        Console.Write(" 🌳 ");
                        break;
                }

                // Indicate whether the player currently is in the current row and cell
                if (_location.Item1 == rowIterator && _location.Item2 == cellIterator)
                {
                    Console.Write("📍");
                } else
                {
                    Console.Write("  ");
                }

                Console.Write(" |");

                cellIterator++;
            }

            Console.WriteLine();

            rowIterator++;
        }
    }

    // Add controller to quest manager observers to receive updates whenever a quest is updated
    public void AddControllerToQuestManagerObservers(Controller controller)
    {
        if (!(controller is IObserver)) return;
        _questManager.RegisterObserver((IObserver)controller);
    }
}