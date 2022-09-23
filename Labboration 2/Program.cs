

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
        private static Customer _currentCustomer;

        static void Main(string[] args)
        {
            _customerCollection = new();
            

            while (_menuState != MenuState.Exit)
            {
                if (_menuState == MenuState.LoginMenu)
                {
                 
                    switch (WriteMenu(new string[] { "Logga in", "Registrera ny kund" },
                        "Välkommen!",
                        "\nAnvänd piltangenterna och ENTER för att välja.\nTryck ESC för att avsluta"))
                    {
                        case -1:
                            _menuState = MenuState.Exit;
                            break;
                        case 0:
                            _menuState = MenuState.Login;
                            break;
                        case 1:
                            ClearConsole();
                            _customerCollection.SaveCustomersToFile();
                            Thread.Sleep(3000);

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

                    if (_customerCollection.CustomerExists(inUsername) && !inPassword.Equals(String.Empty))
                    {
                        if (_customerCollection.TryGetCustomer(inUsername, inPassword, out _currentCustomer))
                        {
                            ClearConsole();
                            Console.WriteLine($"Inloggning lyckades. Välkommen {inUsername}!");
                            _menuState = MenuState.MainMenu;
                        }
                        else
                        {
                            Console.WriteLine($"Felaktigt lösenord. Försök igen");
                        }
                        Thread.Sleep(1500);
                    }
                    else if(!inUsername.Equals(String.Empty) && !inPassword.Equals(String.Empty))
                    {
                        ClearConsole();
                        Console.WriteLine($"Kunden existerar inte. Vill du registrera dig som ny kund?");

                        if (WriteMenu(new string[] { "Ja", "Nej" })==0)
                        {
                            ClearConsole();
                            int customerLevel = WriteMenu(new string[] { "Basic", "Bronze", "Silver", "Guld" }, "Var god välj kundnivå:", "\nTryck ESC för att avbryta");
                            switch (customerLevel)
                            {
                                case 0:
                                    _customerCollection.AddNewCustomer(new Customer(inUsername, inPassword));
                                    _menuState = MenuState.MainMenu;
                                    break;
                                case 1:
                                    _customerCollection.AddNewCustomer(new BronzeCustomer(inUsername, inPassword));
                                    _menuState = MenuState.MainMenu;
                                    break;
                                case 2:
                                    _customerCollection.AddNewCustomer(new SilverCustomer(inUsername, inPassword));
                                    _menuState = MenuState.MainMenu;
                                    break;
                                case 3:
                                    _customerCollection.AddNewCustomer(new GoldCustomer(inUsername, inPassword));
                                    _menuState = MenuState.MainMenu;
                                    break;
                            }
                            //_customerCollection.SaveCustomersToFile();
                        }

                    }
                    else
                    {
                        Console.WriteLine("Felaktig input. Försök igen.");
                        Thread.Sleep(1500);
                    }

                    ClearConsole();
                }
                else if (_menuState == MenuState.Register)
                {

                }
                else if (_menuState == MenuState.MainMenu)
                {
                    WriteMenu(new string[] { "Handla", "Visa Kundvagn","Gå till kassan", "Ändra valuta", "Logga ut"}, $"Välkommen {_currentCustomer.Name}!", "");
                }
                else if (_menuState == MenuState.Cart)
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
            ClearConsoleToRow(0);
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