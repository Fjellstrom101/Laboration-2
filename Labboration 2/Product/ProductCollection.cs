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
                _productList.Add(new Product("Banan", 26.95m, "KG"));
                _productList.Add(new Product("Äpple, Jonagold", 19.90m, "KG"));
                _productList.Add(new Product("Coca Cola, 1,5L", 17.90m, "ST"));
                _productList.Add(new Product("Salami á 80g", 15.90m, "ST"));
                _productList.Add(new Product("Marabou Mjölkchoklad", 19.50m, "ST"));
                _productList.Add(new Product("Lök, Gul", 12.90m, "KG"));
                _productList.Add(new Product("Pågenlimpan", 35.90m, "ST"));
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
    }
}
