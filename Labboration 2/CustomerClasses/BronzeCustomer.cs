namespace Laboration_2
{
    public class BronzeCustomer : Customer
    {
        public BronzeCustomer(string name, string password) : base(name, password)
        {
        }

        public override decimal GetTotalPrice()
        {
            return Math.Round(base.GetTotalPrice() * 0.95M, 2);
        }
    }
}
