
using Laboration_2.CurrencyClasses;
using Laboration_2.CustomerClasses;

namespace Laboration_2
{
    public class Customer
    {
        public string Name { get; set; } //Namn är unikt/key
        public string Password { get; set; }

        //private int Discount { get; set; }

        private List<CartItem> _cart;
        public List<CartItem> Cart { get { return _cart; } } //? Varför retunera hela listan?

        public Currecies Currency { get; set; }
        public Customer(string name, string password)
        {
            Name = name;
            Password = password;
            _cart = new List<CartItem>();
            Currency = Currecies.SEK;
            _cart.Add(new CartItem(new Product("Korv", 80m, "ST")));
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
                retString += string.Format("{0,-20} {1,-10} {2,-10} {3, -10} \n",
                    "Namn", "Antal", "Pris", "Totalt");


                foreach (var item in _cart)
                {
                    retString += string.Format("{0,-20} {1,-10} {2,-10} {3, -10} \n",
                        item.CartProduct.Name, item.Amount, CurrencyConverter.ConvertTo(Currency, item.CartProduct.Price) + " " + Currency.ToString(), "Totalt");
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
