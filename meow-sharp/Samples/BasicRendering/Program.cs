using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicRendering
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
             * In Main function, you only need to create a window and run it.
             * Don't forget to dispose of it afterwards to cleanup all
             * unmanaged resources.
             */ 
            Game game = new Game(800, 600);
            game.Run();
            game.Dispose();
        }
    }
}