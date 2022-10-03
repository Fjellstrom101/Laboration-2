using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Laboration_2
{

    public abstract class ProductCollection
    {
        private static List<Product> _productList;

        public static List<Product> ProductList
        {
            get
            {
                CheckIfListInitialized();
                return _productList;
            }
        }

        private static string _fileName = "Products.json";

        private static void FetchProductsFromFile()
        {
            _productList = new List<Product>();

            if (!File.Exists(_fileName))
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
                string fileInput = File.ReadAllText(_fileName);
                _productList = JsonSerializer.Deserialize<List<Product>>(fileInput)!;
            }
        }

        public static void SaveProductsToFile()
        {
            var jsonString = JsonSerializer.Serialize(_productList);
            File.WriteAllText(_fileName, jsonString);
        }

        public static string ToString() //TODO Döpa om till något annat eftersom den inte overridar när den är statisk?
        {
            string retString = string.Empty;

            if (_productList.Count != 0)
            {
                retString += string.Format("{0,-20} {1,-10}\n",
                    "Namn", "Pris");

                foreach (var product in _productList)
                {
                    retString += string.Format("{0,-20} {1,-10}\n",
                        product.Name, $"{product.Price} SEK/{product.Unit}");
                }
            }
            return retString;
        }

        public static Product GetProductByReference(string name, string unit, decimal price)
        {
            CheckIfListInitialized();

            if (_productList.Any(a => a.Name.Equals(name) && a.Unit.Equals(unit) && a.Price == price))
            {
                return _productList.Find(a => a.Name.Equals(name) && a.Unit.Equals(unit) && a.Price == price);
            }

            return null; //TODO Returnera exception?
        }
        private static void CheckIfListInitialized()
        {
            if (_productList == null)
            {
                FetchProductsFromFile();
            }
        }
    }
}
