using System;
using System.Collections.Generic;
using System.Text;

namespace WebShopRauf.Helpers
{
    class HelpFunction
    {
        /// <summary>
        /// Metoden pausar programmet för bättre interraktion med användaren
        /// </summary>
        public static void Pause()
        {
            Console.WriteLine();
            Console.WriteLine("[Press any key to continue]");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
