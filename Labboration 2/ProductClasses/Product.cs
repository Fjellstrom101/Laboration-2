namespace Laboration_2
{
    public class Product : IComparable
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

        public override string ToString()
        {
            return $"Namn: \"{Name}\", Pris: {Price} SEK / {Unit}";
        }

        public int CompareTo(object? obj)
        {
            if (obj == null) return 1;
            if (obj is not Product) return 1;
            
            Product product = (Product)obj;
            return product.Name.CompareTo(Name);
        }
    }
}
