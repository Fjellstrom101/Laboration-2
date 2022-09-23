using Laboration_2.Customer;

namespace Laboration_2
{
    internal class Program
    {
        private enum MenuState
        {
            LoginMenu,
            Login,
            Register,
            MainMenu,
            Cart,
            Exit
        }

        private static MenuState _menuState;
        private static int _menuPosition = 0;
        private static CustomerCollection _customerCollection;

        static void Main(string[] args)
        {
            _customerCollection = new();

            while (_menuState != MenuState.Exit)
            {
                if (_menuState == MenuState.LoginMenu)
                {
                    /*Console.WriteLine("Välkommen!");
                    Console.CursorVisible = false;

                    WriteMenuItem("Logga in", 0, _menuPosition);

                    WriteMenuItem("Registrera ny kund", 1, _menuPosition);

                    Console.WriteLine("\nAnvänd piltangenterna och ENTER för att välja.\nTryck på ESC för att avsluta");

                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.Escape:
                            _menuState = MenuState.Exit;
                            break;
                        case ConsoleKey.UpArrow:
                            if (_menuPosition > 0)
                            {
                                --_menuPosition;
                            }

                            break;
                        case ConsoleKey.DownArrow:
                            if (_menuPosition < 1)
                            {
                                ++_menuPosition;
                            }

                            break;
                        case ConsoleKey.Enter:

                            switch (_menuPosition)
                            {
                                case 0:
                                    _menuState = MenuState.Login;
                                    break;
                                case 1:
                                    _menuState = MenuState.Register;
                                    break;
                            }

                            break;
                    }
                    ClearConsole();*/
                    switch (WriteMenu(new string[] { "Logga in", "Registrera ny kund" }, "Välkommen!", "test"))
                    {
                        case -1:
                            _menuState = MenuState.Exit;
                            break;
                        case 0:
                            _menuState = MenuState.Login;
                            break;
                        case 1:
                            _menuState = MenuState.Register;
                            break;
                    }
                    ClearConsole();




                }
                else if (_menuState == MenuState.Login)
                {
                    Console.WriteLine("Användarnamn:");
                    string inUsername = Console.ReadLine();

                    Console.WriteLine("Lösenord:");
                    string inPassword = Console.ReadLine();

                    if (_customerCollection.CustomerExists(inUsername))
                    {
                        if (_customerCollection.PasswordIsMatching(inUsername, inPassword))
                        {
                            Console.WriteLine($"Välkommen {inUsername}!");
                        }
                        else
                        {
                            Console.WriteLine($"Felaktigt lösenord");
                        }
                    }
                    else
                    {
                        ClearConsole();
                        Console.WriteLine($"Kunden existerar inte. Vill du registrera dig?");
                        if (WriteMenu(new string[] { "JA", "NEJ" })==0)
                        {

                        }

                    }

                    ClearConsole();
                }
                else if (_menuState == MenuState.MainMenu)
                {

                }
            }
        }

        private static void InvertConsoleColors()
        {
            (Console.BackgroundColor, Console.ForegroundColor) = (Console.ForegroundColor, Console.BackgroundColor);
        }

        private static void WriteInInvertedColors(string inputString)
        {
            InvertConsoleColors();
            Console.WriteLine(inputString);
            InvertConsoleColors();
        }

        private static void WriteMenuItem(string itemText, int thisItemIndex, int selectedIndex)
        {
            if (selectedIndex == thisItemIndex)
            {
                WriteInInvertedColors(itemText);
            }
            else
            {
                Console.WriteLine(itemText);

            }
        }

        private static int WriteMenu(string[] menuStrings)
        {
            return WriteMenu(menuStrings, string.Empty, string.Empty);
        }
        private static int WriteMenu(string[] menuStrings, string preMenuMessage, string postMenuMessage)
        {
            int selectedIndex = 0;
            Console.CursorVisible = false;

            while (true)
            {
                int currentTop = Console.CursorTop;
                int currentLeft = Console.CursorLeft;

                if (!preMenuMessage.Equals(String.Empty))
                {
                    Console.WriteLine(preMenuMessage);
                }


                for (int i = 0; i < menuStrings.Length; i++)
                {
                    WriteMenuItem(menuStrings[i], i, selectedIndex);
                }

                if (!postMenuMessage.Equals(String.Empty))
                {
                    Console.WriteLine(postMenuMessage);
                }

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Escape:
                        return -1;
                    case ConsoleKey.UpArrow:
                        if (selectedIndex > 0)
                        {
                            --selectedIndex;
                        }

                        break;
                    case ConsoleKey.DownArrow:
                        if (selectedIndex < menuStrings.Length-1)
                        {
                            ++selectedIndex;
                        }

                        break;
                    case ConsoleKey.Enter:
                        return selectedIndex;
                }
                ClearConsoleToRow(currentTop);
            }

        }

        //En funktion som clearar konsollen utan anropa Console.Clear(); Du slipper att konsollen flickrar till
        private static void ClearConsole()
        {
            for (int i = Console.CursorTop; i >= 0; i--)
            {
                Console.SetCursorPosition(0,i);
                Console.Write(new string(' ', Console.WindowWidth));
            }
            Console.SetCursorPosition(0, 0);
        }
        private static void ClearConsoleToRow(int top)
        {
            for (int i = Console.CursorTop; i >= top; i--)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(new string(' ', Console.WindowWidth));
            }
            Console.SetCursorPosition(0, top);
        }

    }
}