using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboration_2
{
    public static class CurrencyConverter
    {
        //En statisk klass som innehåller en metod för att konvertera valuta. Valutorna och omvandlingskursen sparas i en Dictionary för att slippa använda massor med If satser. Beloppet avrundas till två decimaler
        public static decimal ConvertTo(Currencies currency, decimal price)
        {
            var d = new Dictionary<Currencies, decimal> //TODO Ändra namnet?
            {
                { Currencies.EUR, 0.092m },
                { Currencies.GBP, 0.082m },
                { Currencies.USD, 0.088m },
                { Currencies.SEK, 1m }
            };
            return Math.Round(price * d[currency], 2);
        }
    }
}
