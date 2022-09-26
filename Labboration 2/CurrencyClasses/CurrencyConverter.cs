using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboration_2.CurrencyClasses
{
    public class CurrencyConverter
    {
        public static decimal ConvertTo(Currecies currecy, decimal price)
        {
            switch (currecy)
            {
                case Currecies.EUR:
                    return Math.Round(price * 0.092m, 2);
                case Currecies.GBP:
                    return Math.Round(price * 0.082m, 2);
                case Currecies.USD:
                    return Math.Round(price * 0.088m, 2);
                default:
                    return Math.Round(price, 2);
            }
        }
    }
}
