
using Laboration_2.CurrencyClasses;
using Laboration_2.MenuClasses;

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
            Shop,
            Cart,
            Checkout,
            Exit
        }

        private static MenuState _menuState;
        private static CustomerCollection _customerCollection;
        private static Customer _currentCustomer;
        private static ProductCollection _productCollection;

        static void Main(string[] args)
        {
            _customerCollection = new();
            _productCollection = new();
            Menu menu = new Menu();
            menu.ShowLoginMenu();
            Console.ReadKey();

            while (_menuState != MenuState.Exit && false)
            {
                if (_menuState == MenuState.LoginMenu)
                {
                 
                    switch (WriteMenu(new string[] { "Logga in", "Registrera ny kund"},
                        "Välkommen!",
                        "\nAnvänd piltangenterna och ENTER för att välja.\nTryck ESC för att avsluta", true))
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

                        if (WriteMenu(new string[] { "Ja", "Nej" }, false)==0)
                        {
                            ClearConsole();
                            int customerLevel = WriteMenu(new string[] { "Basic", "Bronze", "Silver", "Guld" }, "Var god välj kundnivå:", "\nTryck ESC för att avbryta", true);
                            switch (customerLevel)
                            {
                                case 0:
                                    _currentCustomer = new Customer(inUsername, inPassword);
                                    _customerCollection.AddNewCustomer(_currentCustomer);
                                    _menuState = MenuState.MainMenu;
                                    break;
                                case 1:
                                    _currentCustomer = new Customer(inUsername, inPassword);
                                    _customerCollection.AddNewCustomer(_currentCustomer);
                                    _menuState = MenuState.MainMenu;
                                    break;
                                case 2:
                                    _currentCustomer = new Customer(inUsername, inPassword);
                                    _customerCollection.AddNewCustomer(_currentCustomer);
                                    _menuState = MenuState.MainMenu;
                                    break;
                                case 3:
                                    _currentCustomer = new Customer(inUsername, inPassword);
                                    _customerCollection.AddNewCustomer(_currentCustomer);
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
                    int menuChoice = WriteMenu(new string[] { "Handla", "Visa Kundvagn", "Gå till kassan", "Ändra valuta", "Logga ut" }, $"Välkommen {_currentCustomer.Name}!", "", false);
                    switch (menuChoice)
                    {
                        case 0:
                            _menuState = MenuState.Shop;
                            break;
                        case 1:
                            _menuState = MenuState.Cart;
                            break;
                        case 2:
                            _menuState = MenuState.Checkout;
                            break;
                        case 3:

                            ClearConsole();
                            _currentCustomer.Currency = (Currecies) WriteMenu(new string[] { "SEK", "EUR", "GBP", "USD" }, false);
                            break;

                        case 4://Logga ut

                            _currentCustomer = null; // TODO: ÄNDRA kanske? Måste jag nollställa. Kontrollera
                            _customerCollection.SaveCustomersToFile();
                            _menuState = MenuState.LoginMenu;
                            break;

                    }
                    ClearConsole();

                }
                else if (_menuState == MenuState.Shop)
                {
                }
                else if (_menuState == MenuState.Cart)
                {
                    _currentCustomer.GetCartInfo();
                    Thread.Sleep(1000);
                    _menuState = MenuState.MainMenu;
                }
                else if (_menuState == MenuState.Checkout)
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

        private static int WriteMenu(string[] menuStrings, bool useESC)
        {
            return WriteMenu(menuStrings, string.Empty, string.Empty, useESC);
        }
        private static int WriteMenu(string[] menuStrings, string preMenuMessage, string postMenuMessage, bool useESC)
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
                        if (useESC)
                        {
                            return -1;
                        }

                        break;
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