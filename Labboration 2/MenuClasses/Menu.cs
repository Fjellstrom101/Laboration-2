using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Laboration_2.CurrencyClasses;

namespace Laboration_2.MenuClasses
{
    public class Menu
    {
        private List<MenuItem> _loginMenu;
        private List<MenuItem> _mainMenu;

        private CustomerCollection _customerCollection;
        private Customer _currentCustomer;
        private ProductCollection _productCollection;

        public Menu()
        {
            _customerCollection = new CustomerCollection();
            _productCollection = new ProductCollection();

            InitMenus();
            
        }

        public void InitMenus()
        {
            _loginMenu = new List<MenuItem>()
            {
                new MenuItem("Logga in", new Action(ShowLoginPage)),
                new MenuItem("Registrera ny kund", null)
            };
            _mainMenu = new List<MenuItem>()
            {
                new MenuItem("Handla", new Action(ShowShop)),
                new MenuItem("Visa Kundvagn", new Action(ShowCart)),
                new MenuItem("Gå till kassan", null),
                new MenuItem("Ändra valuta", new Action(ShowChangeCurrencyPage)),
                new MenuItem("Logga ut", new Action(ShowLogoutPage))
            };


        }

        public void ShowLoginMenu()
        {
            ConsoleTool.ClearConsole();
            ConsoleTool.WriteMenu(_loginMenu, "Välkommen!",
                "\nAnvänd piltangenterna och ENTER för att välja.\nTryck ESC för att avsluta");
        }
        public void ShowMainMenu()
        {
            ConsoleTool.ClearConsole();
            ConsoleTool.WriteMenu(_mainMenu, "Välkommen!",
                "\nAnvänd piltangenterna och ENTER för att välja.\nTryck ESC för att avsluta");
        }
        public void ShowLoginPage()
        {
            ConsoleTool.ClearConsole();

            Console.WriteLine("Användarnamn:");
            string inUsername = Console.ReadLine();

            Console.WriteLine("Lösenord:");
            string inPassword = Console.ReadLine();

            if (_customerCollection.CustomerExists(inUsername) && !inPassword.Equals(String.Empty))
            {
                if (_customerCollection.TryGetCustomer(inUsername, inPassword, out _currentCustomer))
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
                    ConsoleTool.ClearConsole();
                    int customerLevel = ConsoleTool.WriteMenu(new string[] { "Basic", "Bronze", "Silver", "Guld" },
                        "Var god välj kundnivå:", String.Empty);
                    switch (customerLevel)
                    {
                        case 0:
                            _currentCustomer = new Customer(inUsername, inPassword);
                            _customerCollection.AddNewCustomer(_currentCustomer);
                            break;
                        case 1:
                            _currentCustomer = new Customer(inUsername, inPassword);
                            _customerCollection.AddNewCustomer(_currentCustomer);
                            break;
                        case 2:
                            _currentCustomer = new Customer(inUsername, inPassword);
                            _customerCollection.AddNewCustomer(_currentCustomer);
                            break;
                        case 3:
                            _currentCustomer = new Customer(inUsername, inPassword);
                            _customerCollection.AddNewCustomer(_currentCustomer);
                            break;
                    }
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
            Console.WriteLine(_currentCustomer.GetCartInfo());
            Console.WriteLine("\nTryck på valfri tangent för att fortsätta");
            Console.ReadKey();
            ShowMainMenu();
        }

        public void ShowLogoutPage()
        {
            ConsoleTool.ClearConsole();
            _customerCollection.SaveCustomersToFile();
            Console.WriteLine("Du har loggats ut. Välkommen åter!");
            Thread.Sleep(1500);
            ShowLoginMenu();
        }
        public void ShowChangeCurrencyPage()
        {
            ConsoleTool.ClearConsole();
            _currentCustomer.Currency = (Currecies) ConsoleTool.WriteMenu(new string[] { "SEK", "EUR", "GBP", "USD" });
            
            ConsoleTool.ClearConsole();
            Console.WriteLine($"Valutan ändrades till {_currentCustomer.Currency}");
            Thread.Sleep(1500);
            ShowMainMenu();
        }

        public void ShowShop()
        {
            string[] productArray = new string[_productCollection.ProductList.Count];

            for (int i = 0; i < productArray.Length; i++)
            {
                Product temp = _productCollection.ProductList[i];
                productArray[i] =  string.Format("{0,-20} {1,-10}",
                    temp.Name, $"{CurrencyConverter.ConvertTo(_currentCustomer.Currency, temp.Price)} {_currentCustomer.Currency.ToString()}/{temp.Unit}");
            }

            ConsoleTool.ClearConsole();
            int userChoice = ConsoleTool.WriteMenu(productArray);

            Console.WriteLine($"\nAnge antal {_productCollection.ProductList[userChoice].Unit}");

            Console.ReadLine();
            
            ShowMainMenu();
        }

    }
}
