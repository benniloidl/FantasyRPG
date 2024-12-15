using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DatabaseManager
{
    private static DatabaseManager? _instance;

    private readonly SQLiteConnection _connection;

    private readonly List<Item> _items;

    private DatabaseManager()
    {
        // Initialize database
        _connection = CreateConnection();

        // Create tables if they do not exist yet
        CreateTablesIfNotExisting();

        // Load items from the database
        _items = GetItems();
    }

    // Singleton pattern
    public static DatabaseManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new DatabaseManager();
        }
        return _instance;
    }

    private SQLiteConnection CreateConnection()
    {

        SQLiteConnection connection;

        // Create a new database connection
        connection = new SQLiteConnection("Data Source=database.db; Version = 3; New = True; Compress = True; ");

        // Open the connection
        try
        {
            connection.Open();
        }
        catch (Exception ex)
        {

        }
        return connection;
    }

    private void CreateTablesIfNotExisting()
    {
        SQLiteCommand command = _connection.CreateCommand();

        // All the commands for the tables to be created in the database
        string[] tables = new string[] {
            "CREATE TABLE IF NOT EXISTS Character (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, type VARCHAR(20) NOT NULL, health INTEGER NOT NULL, mana INTEGER NOT NULL, strength INTEGER NOT NULL, agility INTEGER NOT NULL, speed INTEGER NOT NULL, equippedWeaponId INTEGER NULL, equippedDefensiveId INTEGER NULL, equippedUtilityId INTEGER NULL, swordDamage INTEGER NULL, crazyness INTEGER NULL, amountOfArrows INTEGER NULL)",
            "CREATE TABLE IF NOT EXISTS Enemy (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, type VARCHAR(20) NOT NULL, name VARCHAR(20) NULL, health INTEGER NOT NULL, mana INTEGER NOT NULL, strength INTEGER NOT NULL, agility INTEGER NOT NULL, rank INTEGER NOT NULL, weaponId INTEGER NULL)",
            "CREATE TABLE IF NOT EXISTS Item (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, type VARCHAR(20) NOT NULL, rarity INTEGER NOT NULL, damage INTEGER NULL, defense INTEGER NULL, durability INTEGER NULL, effect VARCHAR(20) NULL, duration INTEGER NULL, weaponType INTEGER NULL)",
            "CREATE TABLE IF NOT EXISTS InventoryItem (characterId INTEGER NOT NULL, itemId INTEGER NOT NULL, PRIMARY KEY (characterId, itemId))",
            "CREATE TABLE IF NOT EXISTS GameWorldStructure (worldMapStructure INTEGER NOT NULL, coordX INTEGER NOT NULL, coordY INTEGER NOT NULL, PRIMARY KEY (worldMapStructure, coordX, coordY))",
            "CREATE TABLE IF NOT EXISTS GameWorldEnemyPosition (enemyId INTEGER NOT NULL, coordX INTEGER NOT NULL, coordY INTEGER NOT NULL, PRIMARY KEY (enemyId, coordX, coordY))"
        };

        // Execute the SQL commands to create the tables
        foreach (string table in tables)
        {
            command.CommandText = table;
            command.ExecuteNonQuery();
        }
    }

    // Load items from the database
    private List<Item> GetItems()
    {
        SQLiteDataReader datareader;
        SQLiteCommand command;
        command = _connection.CreateCommand();
        command.CommandText = "SELECT * FROM Item";

        datareader = command.ExecuteReader();
        List<Item> items = new List<Item>();
        while (datareader.Read())
        {
            int id = datareader.GetInt32(0);
            string type = datareader.GetString(1);
            int rarity = datareader.GetInt32(2);
            int? damage = datareader.IsDBNull(3) ? null : datareader.GetInt32(3);
            int? defense = datareader.IsDBNull(4) ? null : datareader.GetInt32(4);
            int? durability = datareader.IsDBNull(5) ? null : datareader.GetInt32(5);
            string? effect = datareader.IsDBNull(6) ? null : datareader.GetString(6);
            int? duration = datareader.IsDBNull(7) ? null : datareader.GetInt32(7);
            int? weaponType = datareader.IsDBNull(8) ? null : datareader.GetInt32(8);

            Item item = null;
            switch (type)
            {
                case "Weapon":
                    item = new Weapon(damage ?? 0, (WeaponType)(weaponType ?? 0), (ItemRarity)rarity);
                    break;
                case "Armor":
                    item = new Armor(defense ?? 0, durability ?? 0, (ItemRarity)rarity);
                    break;
                case "Potion":
                    item = new Potion(effect ?? "", duration ?? 0, (ItemRarity)rarity);
                    break;
            }

            if (item != null)
            {
                item.Id = id;
                items.Add(item);
            }
        }
        return items;
    }

    // Save a new item to the database and return its id or update an existing item
    public int? AddItem(Item? item)
    {
        if (item == null) return null;

        SQLiteCommand command;
        command = _connection.CreateCommand();

        // Check if the item already exists in the database
        if (item.Id != null)
        {
            // Check if the item exists in the database
            command.CommandText = "SELECT COUNT(*) FROM Item WHERE id = @id";
            command.Parameters.AddWithValue("@id", item.Id);
            int count = Convert.ToInt32(command.ExecuteScalar());
            if (count > 0)
            {
                // Update the item in the database
                command.CommandText = "UPDATE Item SET type = @type, rarity = @rarity, damage = @damage, defense = @defense, durability = @durability, effect = @effect, duration = @duration, weaponType = @weaponType WHERE id = @id";
                command.Parameters.AddWithValue("@type", item.GetType().Name);
                command.Parameters.AddWithValue("@rarity", (int)item.ItemRarity);
                command.Parameters.AddWithValue("@damage", item is Weapon ? ((Weapon)item).Damage : (int?)null);
                command.Parameters.AddWithValue("@defense", item is Armor ? ((Armor)item).Defense : (int?)null);
                command.Parameters.AddWithValue("@durability", item is Armor ? ((Armor)item).Durability : (int?)null);
                command.Parameters.AddWithValue("@effect", item is Potion ? ((Potion)item).Effect : (string)null);
                command.Parameters.AddWithValue("@duration", item is Potion ? ((Potion)item).Duration : (int?)null);
                command.Parameters.AddWithValue("@weaponType", item is Weapon ? (int)((Weapon)item).WeaponType : (int?)null);
                command.Parameters.AddWithValue("@id", item.Id);
                command.ExecuteNonQuery();
                return item.Id.Value;
            }
        }

        // Insert the item into the database
        command.CommandText = "INSERT INTO Item (type, rarity, damage, defense, durability, effect, duration, weaponType) VALUES(@type, @rarity, @damage, @defense, @durability, @effect, @duration, @weaponType); SELECT last_insert_rowid();";
        command.Parameters.AddWithValue("@type", item.GetType().Name);
        command.Parameters.AddWithValue("@rarity", (int)item.ItemRarity);
        command.Parameters.AddWithValue("@damage", item is Weapon ? ((Weapon)item).Damage : (int?)null);
        command.Parameters.AddWithValue("@defense", item is Armor ? ((Armor)item).Defense : (int?)null);
        command.Parameters.AddWithValue("@durability", item is Armor ? ((Armor)item).Durability : (int?)null);
        command.Parameters.AddWithValue("@effect", item is Potion ? ((Potion)item).Effect : (string)null);
        command.Parameters.AddWithValue("@duration", item is Potion ? ((Potion)item).Duration : (int?)null);
        command.Parameters.AddWithValue("@weaponType", item is Weapon ? (int)((Weapon)item).WeaponType : (int?)null);
        return Convert.ToInt32(command.ExecuteScalar());
    }

    public void RemoveItem(int id)
    {
        SQLiteCommand command;
        command = _connection.CreateCommand();
        command.CommandText = "DELETE FROM Item WHERE id = @id";
        command.Parameters.AddWithValue("@id", id);
        command.ExecuteNonQuery();
    }

    // Load characters from the database
    public List<Character> GetCharacters()
    {
        SQLiteDataReader datareader;
        SQLiteCommand command;
        command = _connection.CreateCommand();
        command.CommandText = "SELECT * FROM Character";

        datareader = command.ExecuteReader();
        List<Character> characters = new List<Character>();
        while (datareader.Read())
        {
            int id = datareader.GetInt32(0);
            string type = datareader.GetString(1);
            int health = datareader.GetInt32(2);
            int mana = datareader.GetInt32(3);
            int strength = datareader.GetInt32(4);
            int agility = datareader.GetInt32(5);
            int speed = datareader.GetInt32(6);
            int? equippedWeaponId = datareader.IsDBNull(7) ? null : datareader.GetInt32(7);
            int? equippedDefensiveId = datareader.IsDBNull(8) ? null : datareader.GetInt32(8);
            int? equippedUtilityId = datareader.IsDBNull(9) ? null : datareader.GetInt32(9);
            int? swordDamage = datareader.IsDBNull(10) ? null : datareader.GetInt32(10);
            int? crazyness = datareader.IsDBNull(11) ? null : datareader.GetInt32(11);
            int? amountOfArrows = datareader.IsDBNull(12) ? null : datareader.GetInt32(12);

            // Create a character based on the type with basic attributes
            CharacterFactory characterFactory = new CharacterFactory();
            Character character = characterFactory.CreateCharacter(type);
            character.Id = id;
            character.Health = health;
            character.Mana = mana;
            character.Strength = strength;
            character.Agility = agility;
            character.Speed = speed;

            // Equip items to the character
            if (equippedWeaponId != null)
                character.EquipItem(_items.First(item => item.Id == equippedWeaponId));
            if (equippedDefensiveId != null)
                character.EquipItem(_items.First(item => item.Id == equippedDefensiveId));
            if (equippedUtilityId != null)
                character.EquipItem(_items.First(item => item.Id == equippedUtilityId));

            // Apply character type specific attributes
            if (swordDamage != null)
                ((Warrior)character).SwordDamage = swordDamage.Value;
            if (crazyness != null)
                ((Mage)character).Crazyness = crazyness.Value;
            if (amountOfArrows != null)
                ((Archer)character).AmountOfArrows = amountOfArrows.Value;

            // Get all items in the character's inventory
            LoadCharacterInventoryItems(character);

            characters.Add(character);
        }
        return characters;
    }

    // Add all items that are saved in InventoryItem and belong to the character to the character's inventory
    private void LoadCharacterInventoryItems(Character character)
    {
        if (character == null) return;

        SQLiteCommand command;
        command = _connection.CreateCommand();
        command.CommandText = "SELECT itemId FROM InventoryItem WHERE characterId = @characterId";
        command.Parameters.AddWithValue("@characterId", character.Id);
        SQLiteDataReader sqlite_datareaderInventory = command.ExecuteReader();
        while (sqlite_datareaderInventory.Read())
        {
            int itemId = sqlite_datareaderInventory.GetInt32(0);
            character.GetInventory().AddItem(_items.First(item => item.Id == itemId));
        }
    }

    // Save a new character to the database and return its id or update an existing character
    public int AddCharacter(Character character)
    {
        SQLiteCommand command;
        command = _connection.CreateCommand();

        // Check if the character already exists in the database
        if (character.Id != null)
        {
            // Check if the character exists in the database
            command.CommandText = "SELECT COUNT(*) FROM Character WHERE id = @id";
            command.Parameters.AddWithValue("@id", character.Id);
            int count = Convert.ToInt32(command.ExecuteScalar());
            if (count > 0)
            {
                // Update the character in the database
                command.CommandText = "UPDATE Character SET type = @type, health = @health, mana = @mana, strength = @strength, agility = @agility, speed = @speed, equippedWeaponId = @equippedWeaponId, equippedDefensiveId = @equippedDefensiveId, equippedUtilityId = @equippedUtilityId, swordDamage = @swordDamage, crazyness = @crazyness, amountOfArrows = @amountOfArrows WHERE id = @id";
                command.Parameters.AddWithValue("@type", character.GetType().Name);
                command.Parameters.AddWithValue("@health", character.Health);
                command.Parameters.AddWithValue("@mana", character.Mana);
                command.Parameters.AddWithValue("@strength", character.Strength);
                command.Parameters.AddWithValue("@agility", character.Agility);
                command.Parameters.AddWithValue("@speed", character.Speed);
                command.Parameters.AddWithValue("@equippedWeaponId", character.GetEquippedWeapon()?.Id ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@equippedDefensiveId", character.GetEquippedDefensive()?.Id ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@equippedUtilityId", character.GetEquippedUtility()?.Id ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@swordDamage", character is Warrior ? ((Warrior)character).SwordDamage : (int?)null);
                command.Parameters.AddWithValue("@crazyness", character is Mage ? ((Mage)character).Crazyness : (int?)null);
                command.Parameters.AddWithValue("@amountOfArrows", character is Archer ? ((Archer)character).AmountOfArrows : (int?)null);
                command.Parameters.AddWithValue("@id", character.Id);
                command.ExecuteNonQuery();

                AddCharacterItems(character);

                return character.Id.Value;
            }
        }

        // Insert the character into the database
        command.CommandText = "INSERT INTO Character (type, health, mana, strength, agility, speed, equippedWeaponId, equippedDefensiveId, equippedUtilityId, swordDamage, crazyness, amountOfArrows) VALUES(@type, @health, @mana, @strength, @agility, @speed, @equippedWeaponId, @equippedDefensiveId, @equippedUtilityId, @swordDamage, @crazyness, @amountOfArrows); SELECT last_insert_rowid();";
        command.Parameters.AddWithValue("@type", character.GetType().Name);
        command.Parameters.AddWithValue("@health", character.Health);
        command.Parameters.AddWithValue("@mana", character.Mana);
        command.Parameters.AddWithValue("@strength", character.Strength);
        command.Parameters.AddWithValue("@agility", character.Agility);
        command.Parameters.AddWithValue("@speed", character.Speed);
        command.Parameters.AddWithValue("@equippedWeaponId", character.GetEquippedWeapon()?.Id ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@equippedDefensiveId", character.GetEquippedDefensive()?.Id ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@equippedUtilityId", character.GetEquippedUtility()?.Id ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@swordDamage", character is Warrior ? ((Warrior)character).SwordDamage : (int?)null);
        command.Parameters.AddWithValue("@crazyness", character is Mage ? ((Mage)character).Crazyness : (int?)null);
        command.Parameters.AddWithValue("@amountOfArrows", character is Archer ? ((Archer)character).AmountOfArrows : (int?)null);

        AddCharacterItems(character);

        return Convert.ToInt32(command.ExecuteScalar());
    }

    private void AddCharacterItems(Character character)
    {
        // Save the equipped items of the character
        AddItem(character.GetEquippedWeapon());
        AddItem(character.GetEquippedDefensive());
        AddItem(character.GetEquippedUtility());

        // Save every item in the character's inventory
        foreach (Item item in character.GetInventory().GetItems())
        {
            AddItem(item);
        }
    }

    public void RemoveCharacter(int id)
    {
        SQLiteCommand command;
        command = _connection.CreateCommand();
        command.CommandText = "DELETE FROM Character WHERE id = @id";
        command.Parameters.AddWithValue("@id", id);
        command.ExecuteNonQuery();
    }

    // Load enemies from the database
    public Dictionary<Enemy, (int, int)> GetEnemies()
    {
        SQLiteDataReader datareader;
        SQLiteCommand command;
        command = _connection.CreateCommand();
        command.CommandText = "SELECT * FROM Enemy";

        datareader = command.ExecuteReader();
        Dictionary<Enemy, (int, int)> enemies = new Dictionary<Enemy, (int, int)>();
        while (datareader.Read())
        {
            int id = datareader.GetInt32(0);
            string type = datareader.GetString(1);
            string name = datareader.GetString(2);
            int health = datareader.GetInt32(3);
            int mana = datareader.GetInt32(4);
            int strength = datareader.GetInt32(5);
            int agility = datareader.GetInt32(6);
            int rank = datareader.GetInt32(7);
            int? weaponId = datareader.IsDBNull(8) ? null : datareader.GetInt32(8);

            // Determine the factory to use based on the enemy type
            IEnemyFactory enemyFactory = null;
            switch (type)
            {
                case "Dragon":
                    enemyFactory = new DragonFactory();
                    break;
                case "Goblin":
                    enemyFactory = new GoblinFactory();
                    break;
                case "Slime":
                    enemyFactory = new SlimeFactory();
                    break;
            }
            if (enemyFactory == null) continue;

            // Create the enemy and set its attributes
            Enemy enemy = enemyFactory.CreateEnemy((EnemyRank)rank);
            enemy.Id = id;
            enemy.Name = name;
            enemy.Health = health;
            enemy.Mana = mana;
            enemy.Strength = strength;
            enemy.Agility = agility;

            // Equip items to the enemy
            if (weaponId != null)
                enemy.Weapon = (Weapon)_items.First(item => item.Id == weaponId);
            else enemy.Weapon = null;

            // Get the enemy position from the database
            (int, int)? position = LoadEnemyGameWorldPosition(id);

            enemies.Add(enemy, position ?? GameWorld.DEFAULT_SPAWN_LOCATION);
        }
        return enemies;
    }

    // Load the world map position of an enemy from the database
    private (int, int)? LoadEnemyGameWorldPosition(int enemyId)
    {
        SQLiteCommand command;
        command = _connection.CreateCommand();
        command.CommandText = "SELECT * FROM GameWorldEnemyPosition WHERE enemyId = @enemyId";
        command.Parameters.AddWithValue("@enemyId", enemyId);
        SQLiteDataReader sqlite_datareaderPosition = command.ExecuteReader();
        if (sqlite_datareaderPosition.Read())
        {
            int coordX = sqlite_datareaderPosition.GetInt32(1);
            int coordY = sqlite_datareaderPosition.GetInt32(2);
            return (coordX, coordY);
        }
        return null;
    }

    // Save a new enemy to the database and return its id or update an existing enemy
    public int AddEnemy(Enemy enemy, (int, int) position)
    {
        SQLiteCommand command;
        command = _connection.CreateCommand();

        // Check if the enemy already exists in the database
        if (enemy.Id != null)
        {
            // Check if the enemy exists in the database
            command.CommandText = "SELECT COUNT(*) FROM Enemy WHERE id = @id";
            command.Parameters.AddWithValue("@id", enemy.Id);
            int count = Convert.ToInt32(command.ExecuteScalar());
            if (count > 0)
            {
                // Update the enemy in the database
                command.CommandText = "UPDATE Enemy SET type = @type, name = @name, health = @health, mana = @mana, strength = @strength, agility = @agility, rank = @rank, weaponId = @weaponId WHERE id = @id";
                command.Parameters.AddWithValue("@type", enemy.GetType().Name);
                command.Parameters.AddWithValue("@name", enemy.Name);
                command.Parameters.AddWithValue("@health", enemy.Health);
                command.Parameters.AddWithValue("@mana", enemy.Mana);
                command.Parameters.AddWithValue("@strength", enemy.Strength);
                command.Parameters.AddWithValue("@agility", enemy.Agility);
                command.Parameters.AddWithValue("@rank", (int)enemy.Rank);
                command.Parameters.AddWithValue("@weaponId", AddItem(enemy.Weapon));
                command.Parameters.AddWithValue("@id", enemy.Id);
                command.ExecuteNonQuery();

                // Save the enemy position to the database
                
                AddEnemyPosition(enemy.Id.Value, position);

                return enemy.Id.Value;
            }
        }

        // Insert the enemy into the database
        command.CommandText = "INSERT INTO Enemy (type, name, health, mana, strength, agility, rank, weaponId) VALUES(@type, @name, @health, @mana, @strength, @agility, @rank, @weaponId); SELECT last_insert_rowid(); SELECT last_insert_rowid();";
        command.Parameters.AddWithValue("@type", enemy.GetType().Name);
        command.Parameters.AddWithValue("@name", enemy.Name);
        command.Parameters.AddWithValue("@health", enemy.Health);
        command.Parameters.AddWithValue("@mana", enemy.Mana);
        command.Parameters.AddWithValue("@strength", enemy.Strength);
        command.Parameters.AddWithValue("@agility", enemy.Agility);
        command.Parameters.AddWithValue("@rank", (int)enemy.Rank);
        command.Parameters.AddWithValue("@weaponId", AddItem(enemy.Weapon));

        // Save the enemy position to the database
        int id = Convert.ToInt32(command.ExecuteScalar());
        AddEnemyPosition(id, position);

        return id;
    }

    private void AddEnemyPosition(int enemyId, (int, int) position)
    {
        SQLiteCommand command;
        command = _connection.CreateCommand();

        // Clear prior positions
        command.CommandText = "DELETE FROM GameWorldEnemyPosition WHERE enemyId = @enemyId";
        command.Parameters.AddWithValue("@enemyId", enemyId);
        command.ExecuteNonQuery();

        // Insert the enemy position into the database
        command.CommandText = "INSERT INTO GameWorldEnemyPosition (enemyId, coordX, coordY) VALUES(@enemyId, @coordX, @coordY)";
        command.Parameters.AddWithValue("@enemyId", enemyId);
        command.Parameters.AddWithValue("@coordX", position.Item1);
        command.Parameters.AddWithValue("@coordY", position.Item2);
        command.ExecuteNonQuery();
    }

    public void RemoveEnemy(int id)
    {
        SQLiteCommand command;
        command = _connection.CreateCommand();
        command.CommandText = "DELETE FROM Enemy WHERE id = @id";
        command.Parameters.AddWithValue("@id", id);
        command.ExecuteNonQuery();

        // Also remove the enemy position from the database
        command.CommandText = "DELETE FROM GameWorldEnemyPosition WHERE enemyId = @enemyId";
        command.Parameters.AddWithValue("@enemyId", id);
        command.ExecuteNonQuery();
    }

    // Load game world structures and their coordinates from the database
    public Dictionary<(int, int), WorldMapStructure> GetGameWorldStructures()
    {
        SQLiteDataReader datareader;
        SQLiteCommand command;
        command = _connection.CreateCommand();
        command.CommandText = "SELECT * FROM GameWorldStructure";

        datareader = command.ExecuteReader();
        Dictionary<(int, int), WorldMapStructure> worldMap = new Dictionary<(int, int), WorldMapStructure>();
        while (datareader.Read())
        {
            WorldMapStructure structure = (WorldMapStructure)datareader.GetInt32(0);
            int coordX = datareader.GetInt32(1);
            int coordY = datareader.GetInt32(2);
            worldMap.Add((coordX, coordY), structure);
        }
        return worldMap;
    }

    public void AddGameWorldStructure(WorldMapStructure structure, (int, int) position)
    {
        SQLiteCommand command;
        command = _connection.CreateCommand();

        // Insert the structure into the database
        command.CommandText = "INSERT INTO GameWorldStructure (worldMapStructure, coordX, coordY) VALUES(@worldMapStructure, @coordX, @coordY)";
        command.Parameters.AddWithValue("@worldMapStructure", (int)structure);
        command.Parameters.AddWithValue("@coordX", position.Item1);
        command.Parameters.AddWithValue("@coordY", position.Item2);
        command.ExecuteNonQuery();
    }

    public void ClearGameWorldStructures()
    {
        SQLiteCommand command;
        command = _connection.CreateCommand();
        command.CommandText = "DELETE FROM GameWorldStructure";
        command.ExecuteNonQuery();
    }
}