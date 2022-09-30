using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboration_2
{
    public class Menu
    {
        private List<MenuItem> _loginMenu;
        private List<MenuItem> _mainMenu;

        private Customer _currentCustomer;

        public Menu()
        {
            InitMenus();
            ShowLoginMenu();
        }

        public void InitMenus()
        {
            _loginMenu = new List<MenuItem>()
            {
                new MenuItem("Logga in", new Action(ShowLoginPage)),
                new MenuItem("Registrera ny kund", new Action(ShowRegisterCustomerPage)),
                new MenuItem("Avsluta", () => { }) // Tom lambda funktion TODO Ändra till en funktion med ett meddelande?
            };
            _mainMenu = new List<MenuItem>()
            {
                new MenuItem("Handla", new Action(ShowShop)),
                new MenuItem("Visa Kundvagn", new Action(ShowCart)),
                new MenuItem("Gå till kassan", new Action(ShowCheckout)),
                new MenuItem("Ändra valuta", new Action(ShowChangeCurrencyPage)),
                new MenuItem("Logga ut", new Action(ShowLogoutPage))
            };


        }

        public void ShowLoginMenu()
        {
            ConsoleTool.ClearConsole();
            ConsoleTool.WriteMenu(_loginMenu, "Välkommen!",
                "\nAnvänd piltangenterna och ENTER för att välja.");
        }
        public void ShowMainMenu()
        {
            ConsoleTool.ClearConsole();
            ConsoleTool.WriteMenu(_mainMenu, "Välkommen!",
                "\nAnvänd piltangenterna och ENTER för att välja.");
        }
        public void ShowLoginPage()
        {
            ConsoleTool.ClearConsole();

            Console.WriteLine("Användarnamn:");
            string inUsername = Console.ReadLine();

            Console.WriteLine("Lösenord:");
            string inPassword = Console.ReadLine();

            if (CustomerCollection.CustomerExists(inUsername) && !inPassword.Equals(String.Empty))
            {
                if (CustomerCollection.TryGetCustomer(inUsername, inPassword, out _currentCustomer))
                {
                    ConsoleTool.ClearConsole();
                    Console.WriteLine($"Inloggning lyckades. Välkommen {inUsername}!");
                    Thread.Sleep(1500);
                    ShowMainMenu();
                }
                else
                {
                    Console.WriteLine($"Felaktigt lösenord. Försök igen");
                    Thread.Sleep(1500);
                    ShowLoginPage();
                }

                
            }
            else if (!inUsername.Equals(String.Empty) && !inPassword.Equals(String.Empty))
            {
                ConsoleTool.ClearConsole();
                Console.WriteLine($"Kunden existerar inte. Vill du registrera dig som ny kund?");

                if (ConsoleTool.WriteMenu(new string[] { "Ja", "Nej" }) == 0)
                {
                    ShowRegisterCustomerPage(inUsername, inPassword);
                }
                else
                {
                    ShowLoginMenu();
                }
            }
            else
            {
                Console.WriteLine("Felaktig input. Försök igen.");
                Thread.Sleep(1500);
                ShowLoginPage();
            }
        }
        public void ShowCart()
        {
            ConsoleTool.ClearConsole();
            string[] cartArr = _currentCustomer.GetCartInfo().Split('\n');

            //Kundvagnen är inte tom
            if (cartArr.Length>1)
            {
                for (int i = 0; i < cartArr.Length; i++)
                {
                    if (i==0)
                    {
                        ConsoleTool.WriteInInvertedColors(cartArr[i]);
                    }
                    else
                    {
                        Console.WriteLine(cartArr[i]);
                    }
                }
            }
            //Kundvagnen är tom, och arrayen innehåller bara texten "Din kundvagn är tom"
            else
            {
                Console.WriteLine(cartArr[0]);
            }

            Console.WriteLine("\nTryck på valfri tangent för att fortsätta");
            Console.ReadKey();
            ShowMainMenu();
        }
        public void ShowCheckout()
        {
            ConsoleTool.ClearConsole();
            string[] cartArr = _currentCustomer.GetCartInfo().Split('\n');

            //Kundvagnen är inte tom
            if (cartArr.Length > 1)
            {
                for (int i = 0; i < cartArr.Length; i++)
                {
                    if (i == 0)
                    {
                        ConsoleTool.WriteInInvertedColors(cartArr[i]);
                    }
                    else
                    {
                        Console.WriteLine(cartArr[i]);
                    }
                }

                Console.WriteLine("Vill du skapa order?");
                if (ConsoleTool.WriteMenu(new string[]{"Ja", "Nej"}) == 0)
                {
                    ConsoleTool.ClearConsole();
                    Console.WriteLine("Tack! Din order är nu skapad. Vi packar din order så snart som möjligt!");
                    _currentCustomer.Cart.Clear();
                    Thread.Sleep(2500);
                }

                
            }
            //Kundvagnen är tom, och arrayen innehåller bara texten "Kundvagnen är tom"
            else
            {
                Console.WriteLine(cartArr[0]);
                Console.WriteLine("\nTryck på valfri tangent för att fortsätta");
                Console.ReadKey();
            }

            ShowMainMenu();
        }
        public void ShowLogoutPage()
        {
            ConsoleTool.ClearConsole();
            CustomerCollection.SaveCustomersToFile();
            Console.WriteLine("Du har loggats ut. Välkommen åter!");
            Thread.Sleep(1500);
            ShowLoginMenu();
        }
        public void ShowChangeCurrencyPage()
        {
            ConsoleTool.ClearConsole();
            _currentCustomer.Currency = (Currecies) ConsoleTool.WriteMenu(new string[] { "SEK", "EUR", "GBP", "USD" }, "Välj valuta:", string.Empty);
            
            ConsoleTool.ClearConsole();
            Console.WriteLine($"Valutan ändrades till {_currentCustomer.Currency}");
            Thread.Sleep(1500);
            ShowMainMenu();
        }
        public void ShowShop()
        {
            string[] productMenuArray = new string[ProductCollection.ProductList.Count];

            for (int i = 0; i < productMenuArray.Length; i++)
            {
                Product temp = ProductCollection.ProductList[i];
                productMenuArray[i] =  string.Format("{0,-20} {1,-20}",
                    temp.Name, $"{CurrencyConverter.ConvertTo(_currentCustomer.Currency, temp.Price)} {_currentCustomer.Currency.ToString()}/{temp.Unit}");
            }

            ConsoleTool.ClearConsole();
            ConsoleTool.WriteInInvertedColors(string.Format("{0,-20} {1,-20}",
                "Namn", "Pris"));
            int userChoice = ConsoleTool.WriteMenu(productMenuArray, String.Empty, "\nEventuella rabatter dras av i kassan");


            //Loopar tills användaren har matat in ett giltligt antal. Om användaren ångrar sig kan 0 antal matas in.
            Console.WriteLine($"\nAnge antal {ProductCollection.ProductList[userChoice].Unit}");
            string inputAmount;
            
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int amount) && amount >= 0)
                {
                    ConsoleTool.ClearConsole();
                    Console.WriteLine($"{amount} {ProductCollection.ProductList[userChoice].Unit} {ProductCollection.ProductList[userChoice].Name} har lagts till i din kundvagn!");

                    for (int j = 0; j < amount; j++)
                    {
                        _currentCustomer.Cart.Add(ProductCollection.ProductList[userChoice]);
                    }


                    Thread.Sleep(1500);
                    break;
                }
                else
                {
                    Console.WriteLine("Felaktigt antal. Försök igen.");
                    Thread.Sleep(1500);
                    ConsoleTool.ClearNumberOfRows(2);
                }
            }
            
            
            ShowMainMenu();
        }

        public void ShowRegisterCustomerPage()
        {
            ShowRegisterCustomerPage(string.Empty, string.Empty);
        }

        public void ShowRegisterCustomerPage(string inUsername, string inPassword)
        {
            string username = inUsername;
            string password = inPassword;

            ConsoleTool.ClearConsole();

            if (inUsername.Equals(string.Empty) || inPassword.Equals(string.Empty))
            {
                do
                {
                    Console.WriteLine("Användarnamn:");
                    username = Console.ReadLine();

                    if (CustomerCollection.CustomerExists(username))
                    {
                        Console.WriteLine("Användarnamnet är upptaget. Försök igen.");
                        Thread.Sleep(1000);
                        ConsoleTool.ClearNumberOfRows(3);
                    }
                    else if (username.Equals(string.Empty))
                    {
                        Console.WriteLine("Användarnamnet måste innehålla tecken. Försök igen.");
                        Thread.Sleep(1000);
                        ConsoleTool.ClearNumberOfRows(3);
                    }

                } while (CustomerCollection.CustomerExists(username) || username.Equals(string.Empty));

                do
                {
                    Console.WriteLine("Lösenord:");
                    password = Console.ReadLine();

                    if (password.Equals(string.Empty))
                    {
                        Console.WriteLine("Lösenordet måste innehålla tecken. Försök igen.");
                        Thread.Sleep(1000);
                        ConsoleTool.ClearNumberOfRows(3);
                    }

                } while (password.Equals(string.Empty));
            }

            ConsoleTool.ClearConsole();

            int customerLevel = ConsoleTool.WriteMenu(new string[] { "Basic", "Bronze", "Silver", "Guld" },
                "Var god välj kundnivå:", String.Empty);

            _currentCustomer = customerLevel switch
            {
                0 => new Customer(username, password),
                1 => new BronzeCustomer(username, password),
                2 => new SilverCustomer(username, password),
                3 => new GoldCustomer(username, password),
            };

            CustomerCollection.AddNewCustomer(_currentCustomer);


            ConsoleTool.ClearConsole();
            Console.WriteLine($"Kontot är skapat. Välkommen {username}!");
            Thread.Sleep(1500);

            ShowMainMenu();



            Thread.Sleep(2000);
        }


    }
}
