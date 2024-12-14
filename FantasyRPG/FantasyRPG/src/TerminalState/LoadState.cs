using System;

public class LoadState : ITerminalState
{
    private readonly Controller _controller;

    private readonly List<Character> _savedCharacters;
    private List<Character> _characters;

    private int cursorPosition = 0;

    public LoadState(Controller controller)
    {
        _controller = controller;

        _savedCharacters = _controller.GetDatabaseManager().GetCharacters();
        _characters = new List<Character>();
    }

    public void PrintTerminal()
    {
        // Change TerminalState to DefaultState when no characters are saved
        if (_savedCharacters.Count == 0)
        {
            _controller.SetTerminalState(new DefaultState());
            _controller.ForceUpdate();
        }

        Console.WriteLine("Before opening the game, you can decide whether to import your progress.");
        Console.WriteLine();
        Console.WriteLine("--------------------");
        Console.WriteLine();
        Console.Write("Would you like to import one or many of the following saved characters?");
        Console.WriteLine();

        for (int i = 0; i < _savedCharacters.Count; i++)
        {
            Console.Write(i == cursorPosition ? "> " : "  ");

            Console.Write('[');

            if (_characters.Contains(_savedCharacters[i]))
            {
                Console.Write('X');
            }
            else
            {
                Console.Write(' ');
            }

            Console.Write(']');

            Console.WriteLine(_savedCharacters[i]);
        }

        Console.WriteLine();
        Console.WriteLine("--------------------");
        Console.WriteLine();

        Console.WriteLine("Actions:");
        Console.WriteLine(" ⬆️⬇️: Change selection");
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

        // Change to DefaultState when 'Q' is pressed
        else if (key == ConsoleKey.Q)
        {
            // Add all selected characters to the game world
            _characters.ForEach(character => _controller.GetGameWorld().AddCharacter(character));

            _controller.SetTerminalState(new DefaultState());
        }
    }
}