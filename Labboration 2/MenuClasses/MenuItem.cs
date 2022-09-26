using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboration_2.MenuClasses
{
    public class MenuItem
    {
        private Action _menuMethod;
        public Action MenuMethod { get => _menuMethod; }

        private string _menuText;
        public string MenuText { get => _menuText;}


        public MenuItem(string menuText, Action menuMethod)
        {
            _menuMethod = menuMethod;
            _menuText = menuText;
        }

        public static string[] ToStringArray(List<MenuItem> inList)
        {
            string[] retArray = new string[inList.Count];

            for (int i = 0; i < retArray.Length; i++)
            {
                retArray[i] = inList[i].ToString();
            }

            return retArray;
        }

        public override string ToString()
        {
            return _menuText;
        }
    }
}
