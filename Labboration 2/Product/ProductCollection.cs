using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Laboration_2.Product
{

    internal class ProductCollection
    {
        private List<Product> _productList;
        public List<Product> ProductList { get { return _productList; }

        private string _fileName = "Products.json";
        
        public ProductCollection()
        {
            _productList = new List<Product>();

            if (!File.Exists(_fileName))
            {

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
