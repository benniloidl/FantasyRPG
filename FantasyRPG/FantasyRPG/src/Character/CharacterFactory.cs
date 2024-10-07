public class CharacterFactory
{
    public Character CreateCharacter(string characterType)
    {
        if (characterType == "Warrior")
        {
            return new Warrior();
        }
        else if (characterType == "Mage")
        {
            return new Mage();
        }
        else if (characterType == "Archer")
        {
            return new Archer();
        }
        else
        {
            throw new ArgumentException("Invalid character type");
        }
    }
}