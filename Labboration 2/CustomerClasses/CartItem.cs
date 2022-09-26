using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboration_2.CustomerClasses
{
    public class CartItem
    {
        public Product CartProduct { get; set; }
        public int Amount { get; set; }

        public CartItem(Product product)
        {
            CartProduct = product;
            Amount = 1;
        }
    }
}
