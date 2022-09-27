
using Laboration_2.CurrencyClasses;
using Laboration_2.CustomerClasses;

namespace Laboration_2
{
    public class Customer
    {
        public string Name { get; set; } //Namn är unikt/key
        public string Password { get; set; }

        //private int Discount { get; set; }

        private List<Product> _cart;
        public List<Product> Cart { get { return _cart; } } //? Varför retunera hela listan?

        public Currecies Currency { get; set; }
        public Customer(string name, string password)
        {
            Name = name;
            Password = password;
            _cart = new List<Product>();
            Currency = Currecies.SEK;
            _cart.Add(new Product("Korv", 80m, "ST"));
        }
        public decimal GetTotalPrice()
        {
            return 0;
        }

        public string GetCartInfo()
        {
            string retString = string.Empty;

            if (_cart.Count()!=0)
            {
                retString += string.Format("{0,-20} {1,-10} {2,-10} {3, -20} \n",
                    "Namn", "Antal", "Pris", "Totalt");


                int counter = 1;
                for (int i = 0; i < _cart.Count; i++)
                {
                    if (i == _cart.Count - 1 || _cart[i] != _cart[i + 1])
                    {
                        decimal convertedPrice = CurrencyConverter.ConvertTo(Currency, _cart[i].Price);
                        retString += string.Format("{0,-20} {1,-10} {2,-10} {3, -20} \n",
                            _cart[i].Name, counter, convertedPrice.ToString("0.00") + " " + Currency.ToString(), (counter * convertedPrice).ToString("0.00"));
                        
                        counter = 1;
                    }
                    else
                    {
                        counter++;
                    }
                }


            }
            else
            {
                retString += "Här var det tomt!";
            }

            return retString;
        }
        public bool VerifyPassword(string password)
        {
            return Password.Equals(password);
        }
        public override string ToString()
        {
            String retString = $"Användarnamn: {Name} Lösenord: {Password}\n";
            retString += GetCartInfo();
            return retString;
        }

    }
}
