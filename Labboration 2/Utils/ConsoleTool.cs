using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboration_2
{
    internal class ConsoleTool
    {
        public static void InvertConsoleColors()
        {
            (Console.BackgroundColor, Console.ForegroundColor) = (Console.ForegroundColor, Console.BackgroundColor);
        }

        public static void WriteInInvertedColors(string inputString)
        {
            InvertConsoleColors();
            Console.WriteLine(inputString);
            InvertConsoleColors();
        }

        public static void WriteMenuItem(string itemText, int thisItemIndex, int selectedIndex)
        {
            if (selectedIndex == thisItemIndex)
            {
                WriteInInvertedColors(itemText);
            }
            else
            {
                Console.WriteLine(itemText);

            }
        }
        public static void WriteMenu(List<MenuItem> inList, string preMenuMessage, string postMenuMessage)
        {
            int userChoice = WriteMenu(MenuItem.ToStringArray(inList), preMenuMessage, postMenuMessage);
            inList[userChoice].MenuMethod();
        }

        public static int WriteMenu(string[] menuStrings, string preMenuMessage = "", string postMenuMessage = "")
        {
            int selectedIndex = 0;
            Console.CursorVisible = false;
            ClearKeyBuffer();

            while (true)
            {
                int currentTop = Console.CursorTop;
                int currentLeft = Console.CursorLeft;

                if (!preMenuMessage.Equals(String.Empty))
                {
                    Console.WriteLine(preMenuMessage);
                }


                for (int i = 0; i < menuStrings.Length; i++)
                {
                    WriteMenuItem(menuStrings[i], i, selectedIndex);
                }

                if (!postMenuMessage.Equals(String.Empty))
                {
                    Console.WriteLine(postMenuMessage);
                }

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        if (selectedIndex > 0)
                        {
                            --selectedIndex;
                        }

                        break;
                    case ConsoleKey.DownArrow:
                        if (selectedIndex < menuStrings.Length - 1)
                        {
                            ++selectedIndex;
                        }

                        break;
                    case ConsoleKey.Enter:
                        return selectedIndex;
                }
                ClearConsoleToRow(currentTop);
            }

        }

        //En funktion som clearar konsollen utan anropa Console.Clear(); Du slipper att konsollen flickrar till
        public static void ClearConsole()
        {
            ClearConsoleToRow(0);
        }
        public static void ClearNumberOfRows(int numberOfRows)
        {
            ClearConsoleToRow(Console.CursorTop-numberOfRows);
        }
        public static void ClearConsoleToRow(int top)
        {
            for (int i = Console.CursorTop; i >= top; i--)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(new string(' ', Console.WindowWidth));
            }
            Console.SetCursorPosition(0, top);
        }
        public static void ClearKeyBuffer()
        {
            while (Console.KeyAvailable)
            {
                Console.ReadKey(false);
            }
        }
    }
}
