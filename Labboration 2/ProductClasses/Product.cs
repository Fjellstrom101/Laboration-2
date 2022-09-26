namespace Laboration_2
{
    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        public string Unit { get; set; }

        public Product(string name, decimal price, string unit)
        {
            Name = name;
            Price = price;
            Unit = unit;
        }
    }
}
