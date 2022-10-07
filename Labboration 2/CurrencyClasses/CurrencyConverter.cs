using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboration_2
{
    public static class CurrencyConverter
    {
        public static decimal ConvertTo(Currecies currecy, decimal price)
        {
            var d = new Dictionary<Currecies, decimal> //TODO Ändra namnet?
            {
                { Currecies.EUR, 0.092m },
                { Currecies.GBP, 0.082m },
                { Currecies.USD, 0.088m },
                { Currecies.SEK, 1m }
            };
            return Math.Round(price * d[currecy], 2);
        }
    }
}
