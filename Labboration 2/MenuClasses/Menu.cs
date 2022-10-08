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

        //En metod som initierar alla menyer. Varje meny har en lista med MenuItems. Varje MenuItem innehåller en text och en action som körs när det väljs. Skalbart :)
        public void InitMenus()
        {
            _loginMenu = new List<MenuItem>()
            {
                new MenuItem("Logga in", new Action(ShowLoginPage)),
                new MenuItem("Registrera ny kund", new Action(ShowRegisterCustomerPage)),
                new MenuItem("Avsluta", new Action(ShowExitPage)) 
            };
            _mainMenu = new List<MenuItem>()
            {
                new MenuItem("Handla", new Action(ShowShopPage)),
                new MenuItem("Visa Kundvagn", new Action(ShowCartPage)),
                new MenuItem("Gå till kassan", new Action(ShowCheckoutPage)),
                new MenuItem("Ändra valuta", new Action(ShowChangeCurrencyPage)),
                new MenuItem("Logga ut", new Action(ShowLogoutPage))
            };


        }
        //En metod som visar Login menyn genom att skicka in listan _loginMenu i WriteMenu() metoden
        public void ShowLoginMenu()
        {
            ConsoleTool.ClearConsole();
            ConsoleTool.WriteMenu(_loginMenu, "Välkommen!",
                "\nAnvänd piltangenterna och ENTER för att välja.");
        }
        //En metod som visar Main menyn genom att skicka in listan _mainMenu i WriteMenu() metoden
        public void ShowMainMenu()
        {
            ConsoleTool.ClearConsole();
            ConsoleTool.WriteMenu(_mainMenu, $"Välkommen!\nInloggad som {_currentCustomer.Name} ({_currentCustomer.GetCustomerLevel()})\n",
                "\nAnvänd piltangenterna och ENTER för att välja.");
        }

        //En metod som visar login sidan. Här får användaren skriva in användarnamn och lösenord (Måste innehålla minst 1 tecken vardera). Om användaren inte finns registrerad får man möjlighet att registrera sig
        public void ShowLoginPage()
        {
            ConsoleTool.ClearConsole();
            //Användaren matar in användarnamn och lösenord

            Console.WriteLine("Användarnamn:");
            string inUsername = Console.ReadLine();

            Console.WriteLine("Lösenord:");
            string inPassword = Console.ReadLine();

            if (CustomerCollection.CustomerExists(inUsername) && !inPassword.Equals(String.Empty))
            {
                //Om kunden existerar och användaren har matat in ett lösenord.
                if (CustomerCollection.TryGetCustomer(inUsername, inPassword, out _currentCustomer))
                {
                    //Om lösenordet stämmer läggs den inloggade kunden in i _currentCustomer
                    ConsoleTool.ClearConsole();
                    Console.WriteLine($"Inloggning lyckades. Välkommen {inUsername}!");
                    Thread.Sleep(1500);
                    ShowMainMenu();
                }
                else
                {
                    //Om lösenordet är felaktigt får man försöka igen
                    Console.WriteLine($"Felaktigt lösenord. Försök igen");
                    Thread.Sleep(1500);
                    ShowLoginPage();
                }

                
            }
            else if (!inUsername.Equals(String.Empty) && !inPassword.Equals(String.Empty))
            {
                //Kunden existerar inte, men lösenordet och användarnamnet är inte tomma. Vill du registrera dig?
                ConsoleTool.ClearConsole();
                Console.WriteLine($"Kunden existerar inte. Vill du registrera dig som ny kund?");

                if (ConsoleTool.WriteMenu(new string[] { "Ja", "Nej" }) == 0)
                {
                    //Om ja
                    ShowRegisterCustomerPage(inUsername, inPassword);
                }
                else
                {
                    //Om nej så återvänder vi till login menyn
                    ShowLoginMenu();
                }
            }
            else
            {
                //Användarnamnet eller lösenordet är tomt. Försök igen!
                Console.WriteLine("Felaktig input. Försök igen.");
                Thread.Sleep(1500);
                ShowLoginPage();
            }
        }
        public void ShowCartPage()
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
                        //Första raden innehåller ingen vara, utan den innehåller namn på kolumner. Vi skriver ut den med inverterade färger
                        ConsoleTool.WriteInInvertedColors(cartArr[i]);
                    }
                    else
                    {
                        //Alla varor skrivs ut med vanliga färger i konsollen.
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
            Console.ReadKey(true);
            ShowMainMenu();
        }
        
        public void ShowCheckoutPage()
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
                        //Första raden innehåller ingen vara, utan den innehåller namn på kolumner. Vi skriver ut den med inverterade färger
                        ConsoleTool.WriteInInvertedColors(cartArr[i]);
                    }
                    else
                    {
                        //Alla varor skrivs ut med vanliga färger i konsollen.
                        Console.WriteLine(cartArr[i]);
                    }
                }

                Console.WriteLine("Vill du skapa order?");
                if (ConsoleTool.WriteMenu(new string[]{"Ja", "Nej"}) == 0)
                {
                    //Om användaren vill skapa ordern töms kundvagnen.
                    ConsoleTool.ClearConsole();
                    Console.WriteLine("Tack! Din order är nu skapad. Vi packar din order så snart som möjligt!");
                    _currentCustomer.Cart.Clear();
                    Thread.Sleep(2500);
                }
                //Annars retunera till menyn
                
            }
            //Kundvagnen är tom, och arrayen innehåller bara texten "Kundvagnen är tom"
            else
            {
                Console.WriteLine(cartArr[0]);
                Console.WriteLine("\nTryck på valfri tangent för att fortsätta");
                Console.ReadKey(true);
            }

            ShowMainMenu();
        }
        //En metod som används vid utloggning. Alla ändringar som har gjorts sparas och kunden skickas tillbaka till huvudmenyn
        public void ShowLogoutPage()
        {
            ConsoleTool.ClearConsole();
            CustomerCollection.SaveCustomersToFile();
            ProductCollection.SaveProductsToFile();
            Console.WriteLine("Du har loggats ut. Välkommen åter!");
            Thread.Sleep(1500);
            ShowLoginMenu();
        }
        
        //Visar en meny med alla tillgängliga valutor. Om fler valutor läggs till i Currencies så behöver inte metoden skrivas om. Skalbart :)
        public void ShowChangeCurrencyPage()
        {
            ConsoleTool.ClearConsole();
            _currentCustomer.Currency = (Currencies) ConsoleTool.WriteMenu(System.Enum.GetNames(typeof(Currencies)), "Välj valuta:", string.Empty);
            
            ConsoleTool.ClearConsole();
            Console.WriteLine($"Valutan ändrades till {_currentCustomer.Currency}");
            Thread.Sleep(1500);
            ShowMainMenu();
        }

        //Visar själva shop sidan. Skriver ut en lista med alla produkter och användaren får välja vilken produkt han vill köpa och hur många.'
        public void ShowShopPage()
        {
            string[] productMenuArray = new string[ProductCollection.ProductList.Count];

            //Skapar en string lista med produkter. Priset visas i den valda valutan.
            for (int i = 0; i < productMenuArray.Length; i++)
            {
                Product temp = ProductCollection.ProductList[i];
                productMenuArray[i] =  string.Format("{0,-20} {1,-20}",
                    temp.Name, $"{CurrencyConverter.ConvertTo(_currentCustomer.Currency, temp.Price)} {_currentCustomer.Currency.ToString()}/{temp.Unit}");
            }

            ConsoleTool.ClearConsole();
            //Vi skriver ut columnnamnen i inverterade färger och skriver sedan ut produktmenyn
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
                    //Om användaren matar in ett giltligt antal så läggs produkterna till i varukorgen och loopen breakas.
                    ConsoleTool.ClearConsole();
                    Console.WriteLine($"{amount} {ProductCollection.ProductList[userChoice].Unit} {ProductCollection.ProductList[userChoice].Name} har lagts till i din kundvagn!");


                    _currentCustomer.AddToCart(ProductCollection.ProductList[userChoice], amount);


                    Thread.Sleep(1500);
                    break;
                }
                else
                {
                    //Om användaren skriver in ett tal som är mindre än 0, eller något som inte kan parsas till en int så får han försöka igen.
                    Console.WriteLine("Felaktigt antal. Försök igen.");
                    Thread.Sleep(1500);
                    ConsoleTool.ClearNumberOfRows(2);
                }
            }
            
            
            ShowMainMenu();
        }

        //En metod som används i login menyn för att skapa en kund. MenuItem kräver en Action som inte kräver några parameterar
        public void ShowRegisterCustomerPage()
        {
            ShowRegisterCustomerPage(string.Empty, string.Empty);
        }

        //En metod för att registrera en kund. Om användarnamnet eller lösenordet som skickas in i metoden är tomt får användaren mata in dom på nytt.
        public void ShowRegisterCustomerPage(string inUsername, string inPassword)
        {
            string username = inUsername;
            string password = inPassword;

            ConsoleTool.ClearConsole();

            if (inUsername.Equals(string.Empty) || inPassword.Equals(string.Empty))
            {
                //Användarnamnet eller lösenordet är tomt. Användaren får mata in dom på nytt. Loopar till giltligt användarnamn har fåtts
                do
                {
                    Console.WriteLine("Användarnamn:");
                    username = Console.ReadLine();

                    if (CustomerCollection.CustomerExists(username))
                    {
                        //Användarnamnet är redan upptaget. Försök igen
                        Console.WriteLine("Användarnamnet är upptaget. Försök igen.");
                        Thread.Sleep(1000);
                        ConsoleTool.ClearNumberOfRows(3);
                    }
                    else if (username.Equals(string.Empty))
                    {
                        //Användarnamnet är tomt. Försök igen.
                        Console.WriteLine("Användarnamnet måste innehålla tecken. Försök igen.");
                        Thread.Sleep(1000);
                        ConsoleTool.ClearNumberOfRows(3);
                    }

                } while (CustomerCollection.CustomerExists(username) || username.Equals(string.Empty));

                //Loopar tills ett giltligt lösenord har fåtts.
                do
                {
                    Console.WriteLine("Lösenord:");
                    password = Console.ReadLine();

                    if (password.Equals(string.Empty))
                    {
                        //Lösenordet är tomt. Försök igen.
                        Console.WriteLine("Lösenordet måste innehålla tecken. Försök igen.");
                        Thread.Sleep(1000);
                        ConsoleTool.ClearNumberOfRows(3);
                    }

                } while (password.Equals(string.Empty));
            }

            ConsoleTool.ClearConsole();

            //Användaren får välja en nivå på sitt medlemsskap. Kunden läggs sedan till i CustomerCollection och i _currentCustomer
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

        //En metod för att visa en liten "Välkommen åter" sida när användaren väljer att avsluta programmet
        public void ShowExitPage()
        {
            ConsoleTool.ClearConsole();
            Console.WriteLine("Programmet avslutas. Välkommen åter!");
            Environment.Exit(0);
        }


    }
}
