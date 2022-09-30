

namespace Laboration_2
{
    public class Customer
    {
        public string Name { get; private set; }
        public string Password { get; set; }


        private List<Product> _cart;
        public List<Product> Cart { 
            get { return _cart; }
            set { _cart = value; }
        }

        public Currecies Currency { get; set; }
        public Customer(string name, string password)
        {
            Name = name;
            Password = password;
            _cart = new List<Product>();
            Currency = Currecies.SEK;
        }
        public virtual decimal GetTotalPrice()
        {
            return _cart.Sum(a => CurrencyConverter.ConvertTo(Currency,a.Price));
        }

        public string GetCartInfo()
        {
            string retString = string.Empty;

            if (_cart.Count()!=0)
            {
                retString += string.Format("{0,-20} {1,-10} {2,-10} {3, -20} \n",
                    "Namn", "Antal", "Pris", "Totalt");

                _cart.Sort();

                int counter = 1;
                decimal totalPrice = 0;

                for (int i = 0; i < _cart.Count; i++)
                {
                    if (i == _cart.Count - 1 || _cart[i] != _cart[i + 1])
                    {
                        decimal convertedPrice = CurrencyConverter.ConvertTo(Currency, _cart[i].Price);
                        totalPrice += convertedPrice*counter;

                        retString += string.Format("{0,-20} {1,-10} {2,-10} {3, -20} \n",
                            _cart[i].Name,
                            $"{counter} {_cart[i].Unit}",
                            convertedPrice.ToString("0.00") + " " + Currency.ToString(),
                            (counter * convertedPrice).ToString("0.00") + " " + Currency.ToString());
                        
                        counter = 1;
                    }
                    else
                    {
                        counter++;
                    }
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
