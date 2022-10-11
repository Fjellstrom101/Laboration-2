namespace Laboration_2
{
    public class BronzeCustomer : Customer
    {
        //Klassen för Bronskund. Ärver från grundklassen Customer
        public BronzeCustomer(string name, string password) : base(name, password)
        {
        }

        public override decimal GetTotalPrice()
        {
            //Hämtar det totala priset för kundvagnen. 5% rabatt ges och priset rundas av till två decimaler.
            return Math.Round(base.GetTotalPrice() * 0.95M, 2);
        }

        public override string GetCustomerLevel()
        {
            //En metod som används för att få ut kundnivån.
            return "Bronskund";
        }
    }
}
