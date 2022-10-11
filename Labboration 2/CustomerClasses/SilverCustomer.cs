namespace Laboration_2
{
    public class SilverCustomer : Customer
    {
        //Klassen för Silverkund. Ärver från grundklassen Customer
        public SilverCustomer(string name, string password) : base(name, password)
        {
        }
        public override decimal GetTotalPrice()
        {
            //Hämtar det totala priset för kundvagnen. 10% rabatt ges och priset rundas av till två decimaler.
            return Math.Round(base.GetTotalPrice() * 0.9M, 2);
        }

        public override string GetCustomerLevel()
        {
            //En metod som används för att få ut kundnivån.
            return "Silverkund";
        }
    }
}
