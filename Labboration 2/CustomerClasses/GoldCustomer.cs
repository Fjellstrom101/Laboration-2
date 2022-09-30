namespace Laboration_2
{
    public class GoldCustomer : Customer
    {
        public GoldCustomer(string name, string password) : base(name, password)
        {
        }
        public override decimal GetTotalPrice()
        {
            return Math.Round(base.GetTotalPrice() * 0.85M, 2);
        }
    }
}
