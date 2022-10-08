using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

namespace Laboration_2
{

    public static class ProductCollection
    {
        //En statisk klass som innehåller alla produkter. Klassen innehåller alla metoder för att spara ner och hämta produkter från fil. 
        private static List<Product>? _productList;

        public static List<Product> ProductList
        {
            get
            {
                //Kontrollera så att listan är initsierad och produkterna har hämtats från fil innan vi retunerar listan.
                CheckIfListInitialized();
                return _productList;
            }
        }

        private const string FileName = "Products.json";

        private static void FetchProductsFromFile()
        {
            //En metod som initiserar listan _productList. Sen kontrollerar den ifall filen "Products.json" existerar.
            //Ifall den gör det hämtas alla produkter från filen och läggs in i listan. Annars skapas "grund produkterna", läggs in i listan och sparas ner till fil.

            _productList = new List<Product>();

            if (!File.Exists(FileName))
            {
                //Första gången programmet körs, eller om filen tagits bort

                _productList.Add(new Product("Bananer", 26.95m, "KG"));
                _productList.Add(new Product("Äpplen, Jonagold", 19.90m, "KG"));
                _productList.Add(new Product("Coca Cola, 1,5L", 17.90m, "ST"));
                _productList.Add(new Product("Salami á 80g", 15.90m, "ST"));
                _productList.Add(new Product("Marabou Mjölkchoklad", 19.50m, "ST"));
                _productList.Add(new Product("Lökar, Gul", 12.90m, "KG"));
                _productList.Add(new Product("Pågenlimpor", 35.90m, "ST"));

                SaveProductsToFile(); //Skapar en fil med alla varor
            }
            else
            {
                string fileInput = File.ReadAllText(FileName);
                _productList = JsonSerializer.Deserialize<List<Product>>(fileInput)!;
            }
        }

        public static void SaveProductsToFile()
        {
            //En metod som konverterar om listan _productList till JSON och sparar ner till filen "Products.json"
            CheckIfListInitialized();
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var jsonString = JsonSerializer.Serialize(_productList, options);
            File.WriteAllText(FileName, jsonString);
        }

        public static Product GetProductByReference(string name, string unit, decimal price)
        {
            //En metod som retunerar en produkt. Används när kundernas kundvagnar laddas från fil för att produkterna ska referera till samma objekt som kunderna handlar i shoppen.
            //Annars skapas dubbletter av objekten, och == går inte att använda även om alla värden i objekten är samma.

            CheckIfListInitialized();
            return _productList.Find(a => a.Name.Equals(name) && 
                                          a.Unit.Equals(unit) && 
                                          a.Price == price);
        }
        private static void CheckIfListInitialized()
        {
            //Kontrollerar ifall listan är initsierad. Annars körs metoden FetchProductsFromFile()
            if (_productList == null)
            {
                FetchProductsFromFile();
            }
        }
    }
}
