using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Laboration_2
{

    internal class ProductCollection
    {
        private List<Product> _productList;

        public List<Product> ProductList
        {
            get { return _productList; }
        }

        private string _fileName = "Products.json";
        
        public ProductCollection()
        {
            _productList = new List<Product>();

            if (!File.Exists(_fileName))
            {
                _productList.Add(new Product("Bananer", 26.95m, "KG"));
                _productList.Add(new Product("Äpplen, Jonagold", 19.90m, "KG"));
                _productList.Add(new Product("Coca Cola, 1,5L", 17.90m, "ST"));
                _productList.Add(new Product("Salami á 80g", 15.90m, "ST"));
                _productList.Add(new Product("Marabou Mjölkchoklad", 19.50m, "ST"));
                _productList.Add(new Product("Lökar, Gul", 12.90m, "KG"));
                _productList.Add(new Product("Pågenlimpor", 35.90m, "ST"));
            }
            else
            {
                FetchProductsFromFile();
            }
        }
        private void FetchProductsFromFile()
        {
            string fileInput = File.ReadAllText(_fileName);
            _productList = JsonSerializer.Deserialize<List<Product>>(fileInput)!;
        }

        public override string ToString()
        {
            string retString = string.Empty;

            if (_productList.Count!=0)
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
    }
}
