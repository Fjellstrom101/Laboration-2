using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboration_2
{
    public class MenuItem
    {
        //En klass som innehåller en string och en Action. Strängens innehåll visas på menyraden och Action:n körs om menynraden väljs av användaren.
        public Action MenuMethod { get; }

        private readonly string _menuText;
        public string MenuText { get; }


        public MenuItem(string menuText, Action menuMethod)
        {
            MenuMethod = menuMethod;
            _menuText = menuText;
        }

        //En statisk metod som konverterar en lista med MenuItems till en array med strings. Används för att skriva ut menyer.
        public static string[] ToStringArray(List<MenuItem> inList)
        {
            return Array.ConvertAll(inList.ToArray(), a => a.ToString());
        }

        //ToString(). Retunerar _menuText, alltså texten som ska stå på menyraden.
        public override string ToString()
        {
            return _menuText;
        }
    }
}
