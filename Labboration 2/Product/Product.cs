namespace Laboration_2
{
    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        public string Enhet { get; set; }

        public Product(string name, decimal price, string enhet)
        {
            Name = name;
            Price = price;
            Enhet = enhet;
        }
    }
}
