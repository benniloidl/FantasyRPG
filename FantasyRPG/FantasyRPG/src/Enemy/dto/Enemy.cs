public abstract class Enemy
{
    public string Name { get; protected set; }
    public int Health { get; set; }
    public int Mana { get; protected set; }
    public int Strength { get; protected set; }
    public int Agility { get; protected set; }
    public EnemyRank Rank { get; protected set; }
    public Weapon Weapon { get; protected set; }

    // Common actions
    public abstract void Move();
    public abstract void Attack(Character character);

    // Factory method to scale attributes based on rank
    protected void ScaleStatsByRank()
    {
        switch (Rank)
        {
            case EnemyRank.Elite:
                Health *= 2;
                Strength += 10;
                Agility += 5;
                break;
            case EnemyRank.Boss:
                Health *= 3;
                Strength += 20;
                Agility += 10;
                break;
            case EnemyRank.Normal:
            default:
                break;
        }
    }

    // Get a random weapon for the enemy
    protected Weapon GetRandomWeapon()
    {
        // Instantiate all item factories
        CommonItemFactory commonItemFactory = new CommonItemFactory();
        RareItemFactory rareItemFactory = new RareItemFactory();
        MagicalItemFactory magicalItemFactory = new MagicalItemFactory();
        LegendaryItemFactory legendaryItemFactory = new LegendaryItemFactory();

        // Randomly select an item factory
        ItemFactory[] itemFactories = { commonItemFactory, rareItemFactory, magicalItemFactory, legendaryItemFactory };
        ItemFactory randomItemFactory = itemFactories[new Random().Next(0, itemFactories.Length)];

        // Return the weapon created by the selected item factory
        return (Weapon)randomItemFactory.CreateWeapon();
    }

    // Calculate the attack damage of the enemy when attacking a character
    protected int CalculateAttackDamage(Character character)
    {
        // Calculate the enemy's attack damage based on its strength and rank
        int attackDamage = Strength;
        switch (Rank)
        {
            case EnemyRank.Elite:
                attackDamage += 10;
                break;
            case EnemyRank.Boss:
                attackDamage += 20;
                break;
        }

        // Randomly vary the enemy's base attack damage by +/- 10%, rounded to the nearest integer
        attackDamage += new Random().Next(-attackDamage / 10, attackDamage / 10 + 1);

        // Reduce the enemy's attack damage based on the character's agility and equipped defensive item
        attackDamage -= character.Agility;
        attackDamage -= character.GetEquippedDefensive()?.Defense ?? 0;

        // Make sure the enemy's attack damage doesn't go below 0
        if (attackDamage < 0) attackDamage = 0;

        return attackDamage;
    }
}