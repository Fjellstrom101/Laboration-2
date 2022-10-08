namespace Laboration_2
{
    public class GoldCustomer : Customer
    {
        public GoldCustomer(string name, string password) : base(name, password)
        {
        }
        public override decimal GetTotalPrice()
        {
            //Hämtar det totala priset för kundvagnen. 15% rabatt ges och priset rundas av till två decimaler.
            return Math.Round(base.GetTotalPrice() * 0.85M, 2);
        }

        public override string GetCustomerLevel()
        {
            //En metod som används för att få ut kundnivån.
            return "Guldkund";
        }
    }
}
