public class CharacterLoadState : ILoadState
{
    private ITerminalState? _nextState;
    private readonly ITerminalState _toBeNextState;

    private readonly List<Character> _savedCharacters;
    private List<Character> _characters;

    private int cursorPosition = 0;

    public CharacterLoadState()
    {
        // Ensure that the next state will be the EnemyLoadState
        _toBeNextState = new EnemyLoadState();

        // Get characters from the database
        _savedCharacters = DatabaseManager.GetInstance().GetCharacters();

        // Initialize the list of selected characters
        _characters = new List<Character>();
    }

    public ITerminalState? GetNextState() => _nextState;

    public void PrintTerminal()
    {
        Controller controller = Controller.GetInstance();

        // Skip this state if no characters are saved
        if (_savedCharacters.Count == 0)
        {
            // Request a change to the next state
            _nextState = _toBeNextState;
        }

        Console.Write("Would you like to import one or many of the following saved characters?");
        Console.WriteLine();

        // Print saved characters
        for (int i = 0; i < _savedCharacters.Count; i++)
        {
            Console.Write(i == cursorPosition ? "> " : "  ");

            // Indicate if the character is selected
            Console.Write('[');
            if (_characters.Contains(_savedCharacters[i]))
            {
                Console.Write('X');
            }
            else
            {
                Console.Write(' ');
            }
            Console.Write("] ");

            Console.Write(_savedCharacters[i]);
            Console.Write(" ❤️ " + _savedCharacters[i].Health);

            // Print equipment slots
            Console.Write(" 🗡️ " + (_savedCharacters[i].GetEquippedWeapon()?.ToString() ?? "None"));
            Console.Write(" 🛡️ " + (_savedCharacters[i].GetEquippedDefensive()?.ToString() ?? "None"));
            Console.WriteLine(" 🧪 " + (_savedCharacters[i].GetEquippedUtility()?.ToString() ?? "None"));

            int amountOfItemsInInventory = _savedCharacters[i].GetInventory().GetItems().Count;
            Console.WriteLine($"      with {amountOfItemsInInventory} items in inventory");
        }

        Console.WriteLine();
        Console.WriteLine("--------------------");
        Console.WriteLine();

        Console.WriteLine("Actions:");
        Console.WriteLine(" ↕: Change selection");
        Console.WriteLine(" C: Confirm");
        Console.WriteLine(" Q: Quit");
    }

    public void HandleInput(ConsoleKey key)
    {
        // Update selection when arrow keys are pressed
        if (key == ConsoleKey.UpArrow)
        {
            cursorPosition = Math.Max(0, cursorPosition - 1);
        }
        else if (key == ConsoleKey.DownArrow)
        {
            cursorPosition = Math.Min(_savedCharacters.Count - 1, cursorPosition + 1);
        }

        // Confirm selection when 'C' is pressed
        else if (key == ConsoleKey.C)
        {
            if (_characters.Contains(_savedCharacters[cursorPosition]))
            {
                _characters.Remove(_savedCharacters[cursorPosition]);
            }
            else
            {
                _characters.Add(_savedCharacters[cursorPosition]);
            }
        }
    }

    public void Quit()
    {
        Controller controller = Controller.GetInstance();

        // Add all selected characters to the game world
        _characters.ForEach(character => controller.GetGameWorld().AddCharacter(character));

        // Request a change to the next state
        _nextState = _toBeNextState;
    }
}