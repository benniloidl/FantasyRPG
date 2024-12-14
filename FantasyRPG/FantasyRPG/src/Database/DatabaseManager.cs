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

    private readonly SQLiteConnection sqlite_conn;

    private DatabaseManager()
    {
        // Initialize database
        sqlite_conn = CreateConnection();

        // Create tables if they do not exist yet
        CreateTablesIfNotExisting(sqlite_conn);
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

        SQLiteConnection sqlite_conn;

        // Create a new database connection
        sqlite_conn = new SQLiteConnection("Data Source=database.db; Version = 3; New = True; Compress = True; ");

        // Open the connection
        try
        {
            sqlite_conn.Open();
        }
        catch (Exception ex)
        {

        }
        return sqlite_conn;
    }

    private void CreateTablesIfNotExisting(SQLiteConnection conn)
    {
        SQLiteCommand sqlite_cmd = conn.CreateCommand();

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
            sqlite_cmd.CommandText = table;
            sqlite_cmd.ExecuteNonQuery();
        }
    }

    // Load items from the database
    public List<Item> GetItems()
    {
        SQLiteDataReader sqlite_datareader;
        SQLiteCommand sqlite_cmd;
        sqlite_cmd = sqlite_conn.CreateCommand();
        sqlite_cmd.CommandText = "SELECT * FROM Item";

        sqlite_datareader = sqlite_cmd.ExecuteReader();
        List<Item> items = new List<Item>();
        while (sqlite_datareader.Read())
        {
            int id = sqlite_datareader.GetInt32(0);
            string type = sqlite_datareader.GetString(1);
            int rarity = sqlite_datareader.GetInt32(2);
            int? damage = sqlite_datareader.IsDBNull(3) ? null : sqlite_datareader.GetInt32(3);
            int? defense = sqlite_datareader.IsDBNull(4) ? null : sqlite_datareader.GetInt32(4);
            int? durability = sqlite_datareader.IsDBNull(5) ? null : sqlite_datareader.GetInt32(5);
            string? effect = sqlite_datareader.IsDBNull(6) ? null : sqlite_datareader.GetString(6);
            int? duration = sqlite_datareader.IsDBNull(7) ? null : sqlite_datareader.GetInt32(7);
            int? weaponType = sqlite_datareader.IsDBNull(8) ? null : sqlite_datareader.GetInt32(8);

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
    public int AddItem(Item? item)
    {
        if (item == null) return -1;

        SQLiteCommand sqlite_cmd;
        sqlite_cmd = sqlite_conn.CreateCommand();

        // Check if the item already exists in the database
        if (item.Id != null)
        {
            // Check if the item exists in the database
            sqlite_cmd.CommandText = "SELECT COUNT(*) FROM Item WHERE id = @id";
            sqlite_cmd.Parameters.AddWithValue("@id", item.Id);
            int count = Convert.ToInt32(sqlite_cmd.ExecuteScalar());
            if (count > 0)
            {
                // Update the item in the database
                sqlite_cmd.CommandText = "UPDATE Item SET type = @type, rarity = @rarity, damage = @damage, defense = @defense, durability = @durability, effect = @effect, duration = @duration, weaponType = @weaponType WHERE id = @id";
                sqlite_cmd.Parameters.AddWithValue("@type", item.GetType().Name);
                sqlite_cmd.Parameters.AddWithValue("@rarity", (int)item.ItemRarity);
                sqlite_cmd.Parameters.AddWithValue("@damage", item is Weapon ? ((Weapon)item).Damage : (int?)null);
                sqlite_cmd.Parameters.AddWithValue("@defense", item is Armor ? ((Armor)item).Defense : (int?)null);
                sqlite_cmd.Parameters.AddWithValue("@durability", item is Armor ? ((Armor)item).Durability : (int?)null);
                sqlite_cmd.Parameters.AddWithValue("@effect", item is Potion ? ((Potion)item).Effect : (string)null);
                sqlite_cmd.Parameters.AddWithValue("@duration", item is Potion ? ((Potion)item).Duration : (int?)null);
                sqlite_cmd.Parameters.AddWithValue("@weaponType", item is Weapon ? (int)((Weapon)item).WeaponType : (int?)null);
                sqlite_cmd.Parameters.AddWithValue("@id", item.Id);
                sqlite_cmd.ExecuteNonQuery();
                return item.Id.Value;
            }
        }

        // Insert the item into the database
        sqlite_cmd.CommandText = "INSERT INTO Item (type, rarity, damage, defense, durability, effect, duration, weaponType) VALUES(@type, @rarity, @damage, @defense, @durability, @effect, @duration, @weaponType); SELECT last_insert_rowid();";
        sqlite_cmd.Parameters.AddWithValue("@type", item.GetType().Name);
        sqlite_cmd.Parameters.AddWithValue("@rarity", (int)item.ItemRarity);
        sqlite_cmd.Parameters.AddWithValue("@damage", item is Weapon ? ((Weapon)item).Damage : (int?)null);
        sqlite_cmd.Parameters.AddWithValue("@defense", item is Armor ? ((Armor)item).Defense : (int?)null);
        sqlite_cmd.Parameters.AddWithValue("@durability", item is Armor ? ((Armor)item).Durability : (int?)null);
        sqlite_cmd.Parameters.AddWithValue("@effect", item is Potion ? ((Potion)item).Effect : (string)null);
        sqlite_cmd.Parameters.AddWithValue("@duration", item is Potion ? ((Potion)item).Duration : (int?)null);
        sqlite_cmd.Parameters.AddWithValue("@weaponType", item is Weapon ? (int)((Weapon)item).WeaponType : (int?)null);
        return Convert.ToInt32(sqlite_cmd.ExecuteScalar());
    }

    public void RemoveItem(int id)
    {
        SQLiteCommand sqlite_cmd;
        sqlite_cmd = sqlite_conn.CreateCommand();
        sqlite_cmd.CommandText = "DELETE FROM Item WHERE id = @id";
        sqlite_cmd.Parameters.AddWithValue("@id", id);
        sqlite_cmd.ExecuteNonQuery();
    }

    // Load characters from the database
    public List<Character> GetCharacters()
    {
        // Load items from the database to be equipped by the characters later
        List<Item> items = GetItems();

        SQLiteDataReader sqlite_datareader;
        SQLiteCommand sqlite_cmd;
        sqlite_cmd = sqlite_conn.CreateCommand();
        sqlite_cmd.CommandText = "SELECT * FROM Character";

        sqlite_datareader = sqlite_cmd.ExecuteReader();
        List<Character> characters = new List<Character>();
        while (sqlite_datareader.Read())
        {
            int id = sqlite_datareader.GetInt32(0);
            string type = sqlite_datareader.GetString(1);
            int health = sqlite_datareader.GetInt32(2);
            int mana = sqlite_datareader.GetInt32(3);
            int strength = sqlite_datareader.GetInt32(4);
            int agility = sqlite_datareader.GetInt32(5);
            int speed = sqlite_datareader.GetInt32(6);
            int? equippedWeaponId = sqlite_datareader.IsDBNull(7) ? null : sqlite_datareader.GetInt32(7);
            int? equippedDefensiveId = sqlite_datareader.IsDBNull(8) ? null : sqlite_datareader.GetInt32(8);
            int? equippedUtilityId = sqlite_datareader.IsDBNull(9) ? null : sqlite_datareader.GetInt32(9);
            int? swordDamage = sqlite_datareader.IsDBNull(10) ? null : sqlite_datareader.GetInt32(10);
            int? crazyness = sqlite_datareader.IsDBNull(11) ? null : sqlite_datareader.GetInt32(11);
            int? amountOfArrows = sqlite_datareader.IsDBNull(12) ? null : sqlite_datareader.GetInt32(12);

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
                character.EquipItem(items.First(item => item.Id == equippedWeaponId));
            if (equippedDefensiveId != null)
                character.EquipItem(items.First(item => item.Id == equippedDefensiveId));
            if (equippedUtilityId != null)
                character.EquipItem(items.First(item => item.Id == equippedUtilityId));

            // Apply character type specific attributes
            if (swordDamage != null)
                ((Warrior)character).SwordDamage = swordDamage.Value;
            if (crazyness != null)
                ((Mage)character).Crazyness = crazyness.Value;
            if (amountOfArrows != null)
                ((Archer)character).AmountOfArrows = amountOfArrows.Value;

            characters.Add(character);
        }
        return characters;
    }

    // Save a new character to the database and return its id or update an existing character
    public int AddCharacter(Character character)
    {
        SQLiteCommand sqlite_cmd;
        sqlite_cmd = sqlite_conn.CreateCommand();

        // Check if the character already exists in the database
        if (character.Id != null)
        {
            // Check if the character exists in the database
            sqlite_cmd.CommandText = "SELECT COUNT(*) FROM Character WHERE id = @id";
            sqlite_cmd.Parameters.AddWithValue("@id", character.Id);
            int count = Convert.ToInt32(sqlite_cmd.ExecuteScalar());
            if (count > 0)
            {
                // Update the character in the database
                sqlite_cmd.CommandText = "UPDATE Character SET type = @type, health = @health, mana = @mana, strength = @strength, agility = @agility, speed = @speed, equippedWeaponId = @equippedWeaponId, equippedDefensiveId = @equippedDefensiveId, equippedUtilityId = @equippedUtilityId, swordDamage = @swordDamage, crazyness = @crazyness, amountOfArrows = @amountOfArrows WHERE id = @id";
                sqlite_cmd.Parameters.AddWithValue("@type", character.GetType().Name);
                sqlite_cmd.Parameters.AddWithValue("@health", character.Health);
                sqlite_cmd.Parameters.AddWithValue("@mana", character.Mana);
                sqlite_cmd.Parameters.AddWithValue("@strength", character.Strength);
                sqlite_cmd.Parameters.AddWithValue("@agility", character.Agility);
                sqlite_cmd.Parameters.AddWithValue("@speed", character.Speed);
                sqlite_cmd.Parameters.AddWithValue("@equippedWeaponId", character.GetEquippedWeapon()?.Id ?? (object)DBNull.Value);
                sqlite_cmd.Parameters.AddWithValue("@equippedDefensiveId", character.GetEquippedDefensive()?.Id ?? (object)DBNull.Value);
                sqlite_cmd.Parameters.AddWithValue("@equippedUtilityId", character.GetEquippedUtility()?.Id ?? (object)DBNull.Value);
                sqlite_cmd.Parameters.AddWithValue("@swordDamage", character is Warrior ? ((Warrior)character).SwordDamage : (int?)null);
                sqlite_cmd.Parameters.AddWithValue("@crazyness", character is Mage ? ((Mage)character).Crazyness : (int?)null);
                sqlite_cmd.Parameters.AddWithValue("@amountOfArrows", character is Archer ? ((Archer)character).AmountOfArrows : (int?)null);
                sqlite_cmd.Parameters.AddWithValue("@id", character.Id);
                sqlite_cmd.ExecuteNonQuery();

                AddCharacterItems(character);

                return character.Id.Value;
            }
        }

        // Insert the character into the database
        sqlite_cmd.CommandText = "INSERT INTO Character (type, health, mana, strength, agility, speed, equippedWeaponId, equippedDefensiveId, equippedUtilityId, swordDamage, crazyness, amountOfArrows) VALUES(@type, @health, @mana, @strength, @agility, @speed, @equippedWeaponId, @equippedDefensiveId, @equippedUtilityId, @swordDamage, @crazyness, @amountOfArrows); SELECT last_insert_rowid();";
        sqlite_cmd.Parameters.AddWithValue("@type", character.GetType().Name);
        sqlite_cmd.Parameters.AddWithValue("@health", character.Health);
        sqlite_cmd.Parameters.AddWithValue("@mana", character.Mana);
        sqlite_cmd.Parameters.AddWithValue("@strength", character.Strength);
        sqlite_cmd.Parameters.AddWithValue("@agility", character.Agility);
        sqlite_cmd.Parameters.AddWithValue("@speed", character.Speed);
        sqlite_cmd.Parameters.AddWithValue("@equippedWeaponId", character.GetEquippedWeapon()?.Id ?? (object)DBNull.Value);
        sqlite_cmd.Parameters.AddWithValue("@equippedDefensiveId", character.GetEquippedDefensive()?.Id ?? (object)DBNull.Value);
        sqlite_cmd.Parameters.AddWithValue("@equippedUtilityId", character.GetEquippedUtility()?.Id ?? (object)DBNull.Value);
        sqlite_cmd.Parameters.AddWithValue("@swordDamage", character is Warrior ? ((Warrior)character).SwordDamage : (int?)null);
        sqlite_cmd.Parameters.AddWithValue("@crazyness", character is Mage ? ((Mage)character).Crazyness : (int?)null);
        sqlite_cmd.Parameters.AddWithValue("@amountOfArrows", character is Archer ? ((Archer)character).AmountOfArrows : (int?)null);

        AddCharacterItems(character);

        return Convert.ToInt32(sqlite_cmd.ExecuteScalar());
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
        SQLiteCommand sqlite_cmd;
        sqlite_cmd = sqlite_conn.CreateCommand();
        sqlite_cmd.CommandText = "DELETE FROM Character WHERE id = @id";
        sqlite_cmd.Parameters.AddWithValue("@id", id);
        sqlite_cmd.ExecuteNonQuery();
    }

    // Load enemies from the database
    public Dictionary<Enemy, (int, int)?> GetEnemies()
    {
        // Load items from the database to be equipped by the enemies later
        List<Item> items = GetItems();

        SQLiteDataReader sqlite_datareader;
        SQLiteCommand sqlite_cmd;
        sqlite_cmd = sqlite_conn.CreateCommand();
        sqlite_cmd.CommandText = "SELECT * FROM Enemy";

        sqlite_datareader = sqlite_cmd.ExecuteReader();
        Dictionary<Enemy, (int, int)?> enemies = new Dictionary<Enemy, (int, int)?>();
        while (sqlite_datareader.Read())
        {
            int id = sqlite_datareader.GetInt32(0);
            string type = sqlite_datareader.GetString(1);
            string name = sqlite_datareader.GetString(2);
            int health = sqlite_datareader.GetInt32(3);
            int mana = sqlite_datareader.GetInt32(4);
            int strength = sqlite_datareader.GetInt32(5);
            int agility = sqlite_datareader.GetInt32(6);
            int rank = sqlite_datareader.GetInt32(7);
            int? weaponId = sqlite_datareader.IsDBNull(8) ? null : sqlite_datareader.GetInt32(8);

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
                enemy.Weapon = (Weapon)items.First(item => item.Id == weaponId);

            // Get the enemy position from the database
            sqlite_cmd.CommandText = "SELECT * FROM GameWorldEnemyPosition WHERE enemyId = @enemyId";
            sqlite_cmd.Parameters.AddWithValue("@enemyId", id);
            SQLiteDataReader sqlite_datareaderPosition = sqlite_cmd.ExecuteReader();
            (int, int)? position = null;
            if (sqlite_datareaderPosition.Read())
            {
                int coordX = sqlite_datareaderPosition.GetInt32(1);
                int coordY = sqlite_datareaderPosition.GetInt32(2);
                position = (coordX, coordY);
            }

            enemies.Add(enemy, position);
        }
        return enemies;
    }

    // Save a new enemy to the database and return its id or update an existing enemy
    public int AddEnemy(Enemy enemy, (int, int) position)
    {
        SQLiteCommand sqlite_cmd;
        sqlite_cmd = sqlite_conn.CreateCommand();

        // Check if the enemy already exists in the database
        if (enemy.Id != null)
        {
            // Check if the enemy exists in the database
            sqlite_cmd.CommandText = "SELECT COUNT(*) FROM Enemy WHERE id = @id";
            sqlite_cmd.Parameters.AddWithValue("@id", enemy.Id);
            int count = Convert.ToInt32(sqlite_cmd.ExecuteScalar());
            if (count > 0)
            {
                // Update the enemy in the database
                sqlite_cmd.CommandText = "UPDATE Enemy SET type = @type, name = @name, health = @health, mana = @mana, strength = @strength, agility = @agility, rank = @rank, weaponId = @weaponId WHERE id = @id";
                sqlite_cmd.Parameters.AddWithValue("@type", enemy.GetType().Name);
                sqlite_cmd.Parameters.AddWithValue("@name", enemy.Name);
                sqlite_cmd.Parameters.AddWithValue("@health", enemy.Health);
                sqlite_cmd.Parameters.AddWithValue("@mana", enemy.Mana);
                sqlite_cmd.Parameters.AddWithValue("@strength", enemy.Strength);
                sqlite_cmd.Parameters.AddWithValue("@agility", enemy.Agility);
                sqlite_cmd.Parameters.AddWithValue("@rank", (int)enemy.Rank);
                sqlite_cmd.Parameters.AddWithValue("@weaponId", enemy.Weapon?.Id ?? (object)DBNull.Value);
                sqlite_cmd.Parameters.AddWithValue("@id", enemy.Id);
                sqlite_cmd.ExecuteNonQuery();

                // Save the equipped weapon of the enemy to the database
                AddItem(enemy.Weapon);

                // Save the enemy position to the database
                
                AddEnemyPosition(enemy.Id.Value, position);

                return enemy.Id.Value;
            }
        }

        // Insert the enemy into the database
        sqlite_cmd.CommandText = "INSERT INTO Enemy (type, name, health, mana, strength, agility, rank, weaponId) VALUES(@type, @name, @health, @mana, @strength, @agility, @rank, @weaponId); SELECT last_insert_rowid(); SELECT last_insert_rowid();";
        sqlite_cmd.Parameters.AddWithValue("@type", enemy.GetType().Name);
        sqlite_cmd.Parameters.AddWithValue("@name", enemy.Name);
        sqlite_cmd.Parameters.AddWithValue("@health", enemy.Health);
        sqlite_cmd.Parameters.AddWithValue("@mana", enemy.Mana);
        sqlite_cmd.Parameters.AddWithValue("@strength", enemy.Strength);
        sqlite_cmd.Parameters.AddWithValue("@agility", enemy.Agility);
        sqlite_cmd.Parameters.AddWithValue("@rank", (int)enemy.Rank);
        sqlite_cmd.Parameters.AddWithValue("@weaponId", enemy.Weapon?.Id ?? (object)DBNull.Value);

        // Save the equipped weapon of the enemy to the database
        AddItem(enemy.Weapon);

        int id = Convert.ToInt32(sqlite_cmd.ExecuteScalar());

        // Save the enemy position to the database
        
        AddEnemyPosition(id, position);

        return id;
    }

    private void AddEnemyPosition(int enemyId, (int, int) position)
    {
        SQLiteCommand sqlite_cmd;
        sqlite_cmd = sqlite_conn.CreateCommand();

        // Clear prior positions
        sqlite_cmd.CommandText = "DELETE FROM GameWorldEnemyPosition WHERE enemyId = @enemyId";
        sqlite_cmd.Parameters.AddWithValue("@enemyId", enemyId);
        sqlite_cmd.ExecuteNonQuery();

        // Insert the enemy position into the database
        sqlite_cmd.CommandText = "INSERT INTO GameWorldEnemyPosition (enemyId, coordX, coordY) VALUES(@enemyId, @coordX, @coordY)";
        sqlite_cmd.Parameters.AddWithValue("@enemyId", enemyId);
        sqlite_cmd.Parameters.AddWithValue("@coordX", position.Item1);
        sqlite_cmd.Parameters.AddWithValue("@coordY", position.Item2);
        sqlite_cmd.ExecuteNonQuery();
    }

    public void RemoveEnemy(int id)
    {
        SQLiteCommand sqlite_cmd;
        sqlite_cmd = sqlite_conn.CreateCommand();
        sqlite_cmd.CommandText = "DELETE FROM Enemy WHERE id = @id";
        sqlite_cmd.Parameters.AddWithValue("@id", id);
        sqlite_cmd.ExecuteNonQuery();

        // Also remove the enemy position from the database
        sqlite_cmd.CommandText = "DELETE FROM GameWorldEnemyPosition WHERE enemyId = @enemyId";
        sqlite_cmd.Parameters.AddWithValue("@enemyId", id);
        sqlite_cmd.ExecuteNonQuery();
    }

    // Load game world structures and their coordinates from the database
    public Dictionary<(int, int), WorldMapStructure> GetGameWorldStructures()
    {
        SQLiteDataReader sqlite_datareader;
        SQLiteCommand sqlite_cmd;
        sqlite_cmd = sqlite_conn.CreateCommand();
        sqlite_cmd.CommandText = "SELECT * FROM GameWorldStructure";

        sqlite_datareader = sqlite_cmd.ExecuteReader();
        Dictionary<(int, int), WorldMapStructure> worldMap = new Dictionary<(int, int), WorldMapStructure>();
        while (sqlite_datareader.Read())
        {
            WorldMapStructure structure = (WorldMapStructure)sqlite_datareader.GetInt32(0);
            int coordX = sqlite_datareader.GetInt32(1);
            int coordY = sqlite_datareader.GetInt32(2);
            worldMap.Add((coordX, coordY), structure);
        }
        return worldMap;
    }

    public void AddGameWorldStructure(WorldMapStructure structure, (int, int) position)
    {
        SQLiteCommand sqlite_cmd;
        sqlite_cmd = sqlite_conn.CreateCommand();

        // Insert the structure into the database
        sqlite_cmd.CommandText = "INSERT INTO GameWorldStructure (worldMapStructure, coordX, coordY) VALUES(@worldMapStructure, @coordX, @coordY)";
        sqlite_cmd.Parameters.AddWithValue("@worldMapStructure", (int)structure);
        sqlite_cmd.Parameters.AddWithValue("@coordX", position.Item1);
        sqlite_cmd.Parameters.AddWithValue("@coordY", position.Item2);
        sqlite_cmd.ExecuteNonQuery();
    }

    public void ClearGameWorldStructures()
    {
        SQLiteCommand sqlite_cmd;
        sqlite_cmd = sqlite_conn.CreateCommand();
        sqlite_cmd.CommandText = "DELETE FROM GameWorldStructure";
        sqlite_cmd.ExecuteNonQuery();
    }
}