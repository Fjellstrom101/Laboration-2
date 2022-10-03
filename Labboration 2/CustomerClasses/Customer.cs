

namespace Laboration_2
{
    public class Customer
    {
        public string Name { get; private set; }
        public string Password { get; set; }


        private List<CartItem> _cart;
        public List<CartItem> Cart { 
            get { return _cart; }
            set { _cart = value; }
        }

        public Currecies Currency { get; set; }
        public Customer(string name, string password)
        {
            Name = name;
            Password = password;
            _cart = new List<CartItem>();
            Currency = Currecies.SEK;
        }
        public virtual decimal GetTotalPrice()
        {
            return _cart.Sum(a => CurrencyConverter.ConvertTo(Currency,a.Product.Price)*a.Amount);
        }

        public void AddToCart(Product product, int amount)
        {
            if (amount <= 0) return;

            if (_cart.Any(a => a.Product == product))
            {
                _cart.Find(a => a.Product == product).Amount += amount;
            }
            else
            {
                _cart.Add(new CartItem(){Product = product, Amount = amount});
            }
        }

        public string GetCartInfo()
        {
            string retString = string.Empty;

            if (_cart.Count()!=0)
            {
                retString += string.Format("{0,-20} {1,-10} {2,-10} {3, -20} \n",
                    "Namn", "Antal", "Pris", "Totalt");


                decimal totalPrice = 0;

                foreach (var item in _cart)
                {

                    decimal convertedPrice = CurrencyConverter.ConvertTo(Currency, item.Product.Price);
                    totalPrice += convertedPrice * item.Amount;

                    retString += string.Format("{0,-20} {1,-10} {2,-10} {3, -20} \n",
                        item.Product.Name,
                        $"{item.Amount} {item.Product.Unit}",
                        convertedPrice.ToString("0.00") + " " + Currency.ToString(),
                        (item.Amount * convertedPrice).ToString("0.00") + " " + Currency.ToString());
                    
 
                }

                if (GetTotalPrice() != totalPrice)
                {
                    retString += string.Format("\n{0,-20} {1,-10} {2,-10} {3, -20} ",
                        string.Empty, string.Empty, "Rabatt:", $"{totalPrice-GetTotalPrice()} {Currency.ToString()}");
                }

                retString += string.Format("\n{0,-20} {1,-10} {2,-10} {3, -20} ",
                    string.Empty, string.Empty, "Totalt:", $"{GetTotalPrice()} {Currency.ToString()}");


            }
            else
            {
                retString += "Kundvagnen är tom!";
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
