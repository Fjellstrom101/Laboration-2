namespace Laboration_2
{
    public class Product
    {
        //En klass som representerar en produkt. Innehåller namn, pris och enhet.
        public string Name { get; set; }
        public decimal Price { get; set; }

        public string Unit { get; set; }

        public Product(string name, decimal price, string unit)
        {
            Name = name;
            Price = price;
            Unit = unit;
        }

        //ToString är implementerad enligt Niklas specifikationer :)
        public override string ToString()
        {
            return $"Namn: \"{Name}\", Pris: {Price} SEK / {Unit}";
        }
    }
}
