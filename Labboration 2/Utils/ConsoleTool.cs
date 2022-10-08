using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboration_2
{
    public static class ConsoleTool
    {
        public static void InvertConsoleColors()
        {
            //En metod som inverterar konsollens färger med hjälp av en tuple.
            (Console.BackgroundColor, Console.ForegroundColor) = (Console.ForegroundColor, Console.BackgroundColor);
        }

        public static void WriteInInvertedColors(string inputString)
        {
            //En metod för att skriva ut en sträng i konsollen med inverterade färger. Färgerna inverteras sedan tillbaks.
            InvertConsoleColors();
            Console.WriteLine(inputString);
            InvertConsoleColors();
        }

        public static void WriteMenuItem(string itemText, int thisItemIndex, int selectedIndex)
        {
            //En metod för att skriva ut texten från ett MenuItem. Ifall just det menyvalet är valt kommer det att skrivas ut med inverterade färger. Annars inte.
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
            //En metod som skriver ut en meny med hjälp listan inList. inList konverteras till en sträng array, och användaren får sedan välja ett alternativ i menyn.
            //Valet retuneras som en int till variabeln userChoice. Sist körs Action:n från det valda MenuItem:t

            int userChoice = WriteMenu(MenuItem.ToStringArray(inList), preMenuMessage, postMenuMessage);
            inList[userChoice].MenuMethod();
        }

        public static int WriteMenu(string[] menuStrings, string preMenuMessage = "", string postMenuMessage = "")
        {
            //En metod som skriver ut en meny med hjälp av en array av strängar.
            //Användaren får välja ifall dom vill ha ett meddelande på raden innan eller efter menyn med hjälp av parameterarna preMenuMessage och postMenuMessage. Parametrarna är valfria.


            //Det valda menyindexet sätts till 0
            int selectedIndex = 0;
            Console.CursorVisible = false;
            ClearKeyBuffer();

            //En loop som körs tills användaren gjort ett val i menyn.
            while (true)
            {
                //Vi håller koll på vilken rad menyn börjar på.
                int currentTop = Console.CursorTop;

                //Om användaren har matat in ett meddelande som ska visas innan menyn så skrivs det ut.
                if (!preMenuMessage.Equals(String.Empty))
                {
                    Console.WriteLine(preMenuMessage);
                }

                //Vi skriver ut alla menyalternativen. Om alternativet är valt inverteras färgerna.
                for (int i = 0; i < menuStrings.Length; i++)
                {
                    WriteMenuItem(menuStrings[i], i, selectedIndex);
                }

                //Om användaren har matat in ett meddelande som ska visas efter menyn så skrivs det ut.
                if (!postMenuMessage.Equals(String.Empty))
                {
                    Console.WriteLine(postMenuMessage);
                }

                //Användaren får använda piltangenterna och enter för att förflytta sig eller välja i menyn.
                //Om piltangenterna används justeras värdet på selectedIndex. Om enter trycks retuneras värdet på selectedIndex;
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
                //selectedIndex har justeras. Vi suddar ut menyn för att sedan måla upp den igen i nästa loopning.
                ClearConsoleToRow(currentTop);
            }

        }

        
        public static void ClearConsole()
        {
            //En funktion som clearar konsollen utan anropa Console.Clear(); Du slipper att konsollen flickrar till så irriterande. Clearar alla rader till och med rad 0
            ClearConsoleToRow(0);
        }
        public static void ClearNumberOfRows(int numberOfRows)
        {
            //Tar bort parametern numberOfRows antal rader från konsollen.
            ClearConsoleToRow(Console.CursorTop-numberOfRows);
        }
        public static void ClearConsoleToRow(int top)
        {
            //En metod som clearar konsollen till rad top. Den gör det genom att skriva ut en sträng med mellanslag som är lika lång som fönstrets bredd.
            //Den gör det från raden pekaren är på till raden i parametern top
            for (int i = Console.CursorTop; i >= top; i--)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(new string(' ', Console.WindowWidth));
            }
            Console.SetCursorPosition(0, top);
        }
        
        public static void ClearKeyBuffer()
        {
            //En metod som används för att rensa alla knapptryckningar som otåliga användare gör under Thread.Sleep().
            while (Console.KeyAvailable)
            {
                Console.ReadKey(false);
            }
        }
    }
}
