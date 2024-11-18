public static class ItemHandlerFactory
{
    public static IItemHandler GetHandler(Item item)
    {
        return item switch
        {
            Weapon => new WeaponHandler(),
            DefensiveItem => new DefensiveItemHandler(),
            UtilityItem => new UtilityItemHandler(),
            _ => throw new ArgumentException("Item type not supported")
        };
    }
}