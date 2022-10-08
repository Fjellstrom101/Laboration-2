

namespace Laboration_2
{
    public class Customer
    {
        //Basklassen kund. Innehåller namn, lösenord, en varukorgs-lista och en vald valuta för kunden.

        public string Name { get; private set; }
        public string Password { get; set; }


        public List<CartItem> Cart { get; set; }

        public Currencies Currency { get; set; }
        public Customer(string name, string password)
        {
            Name = name;
            Password = password;
            Cart = new List<CartItem>();
            Currency = Currencies.SEK;
        }
        public virtual decimal GetTotalPrice()
        {
            //Hämtar det totala priset för kundvagnen med hjälp av LINQ funktinen Sum. Varornas pris konverteras till vald valuta och gångras med antal.
            //Här räknas även eventuell rabatt med. Baskunden har 0% rabatt :)
            return Cart.Sum(a => CurrencyConverter.ConvertTo(Currency,a.Product.Price)*a.Amount);
        }

        public void AddToCart(Product product, int amount)
        {
            //Lägger till en produkt i kundvagnen. Om antalet är noll avbryts metoden.
            //Om den redan finns i kundvagnen så ökas antalet på just det CartItem:t. Annars läggs det till.

            if (amount <= 0) return;

            if (Cart.Any(a => a.Product == product))
            {
                Cart.Find(a => a.Product == product).Amount += amount;
            }
            else
            {
                Cart.Add(new CartItem(){Product = product, Amount = amount});
            }
        }

        public string GetCartInfo()
        {
            //En metod som retunerar en sträng med alla varor i kundvagnen. Första raden innehåller namn på alla kolumner. Om varukorgen är tom retuneras "Kundvagnen är tom!"
            //En variabel håller även koll på det orabatterade totalpriset. Ifall det skiljer sig från priset retunerat av metoden GetTotalPrice() skrivs även den totala rabatten ut.
            //Sist av allt skrivs totalpriset ut.
            string retString = string.Empty;

            if (Cart.Count()!=0)
            {
                retString += string.Format("{0,-20} {1,-10} {2,-10} {3, -20} \n",
                    "Namn", "Antal", "Pris", "Totalt");


                decimal totalPrice = 0;

                foreach (var item in Cart)
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
                        string.Empty, string.Empty, $"Rabatt:", $"{totalPrice-GetTotalPrice()} {Currency.ToString()}");
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
            //En metod för att verifiera lösenord enligt Niklas specifikation. Det hade ju gått att göra med LINQ :)
            return Password.Equals(password);
        }

        public virtual string GetCustomerLevel()
        {
            //En metod som används för att få ut kundnivån.
            return "Baskund";
        }
        public override string ToString()
        {
            //ToString implementerad enligt Niklas specifikation. Lite farligt att skriva ut användarens lösenord i ren text :)
            String retString = $"Användarnamn: {Name}, Lösenord: {Password}\n";
            retString += GetCartInfo();
            return retString;
        }

    }
}
