using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboration_2
{
    public class CartItem
    {
        //En liten klass som används i kundvagnen. Innehåller en produkt och antal.
        //Den finns till för att om användaren la till miljoner av en produkt, och sedan valde att logga ut så blev textfilen väldigt väldigt stor.
        public Product Product { get; set; }
        public int Amount { get; set; }

    }
}
