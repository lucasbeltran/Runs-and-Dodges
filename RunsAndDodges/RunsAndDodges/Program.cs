#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;



#endregion

namespace RunsAndDodges
{
   public static class Program
    {
        public static Inicio game;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
       	static void Main()
        {
            game = new Inicio();
            game.Run();
        }
    }
}
