
namespace Laboration_2
{
    public class Customer
    {
        public string Name { get; set; } //Namn är unikt/key
        public string Password { get; set; }

        //private int Discount { get; set; }

        private List<Product> _cart;
        public List<Product> Cart { get { return _cart; } } //? Varför retunera hela listan?

        public Currecies currency { get; set; }
        public Customer(string name, string password)
        {
            Name = name;
            Password = password;
            _cart = new List<Product>();
            currency = Currecies.SEK;
        }
        public decimal GetTotalPrice()
        {
            return 0;
        }

        public string GetCartInfo()
        {
            string retString = string.Empty;
            Console.WriteLine(string.Format("{0,-20} {1,-10} {2,-10} {3, -10} \n",
                "Namn", "Antal", "Pris", "Valuta"));

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
