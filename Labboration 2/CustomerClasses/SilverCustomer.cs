namespace Laboration_2
{
    internal class SilverCustomer : Customer
    {
        public SilverCustomer(string name, string password) : base(name, password)
        {
        }
        public override decimal GetTotalPrice()
        {
            return Math.Round(base.GetTotalPrice() * 0.9M, 2);
        }
    }
}
