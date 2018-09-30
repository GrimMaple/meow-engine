using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meow.Framework;
using Meow.Framework.Graphics;
using Meow.Framework.Util;
using Meow.Framework.Audio;

namespace BasicRendering
{
    /*
     *  In order to create a new game window, you have to subclass a MeowGame
     */ 
    class Game : MeowGame
    {
        /*
         * Constructor of MeowGame takes two arguments  
         *      w -- window width
         *      h -- window height
         */
        public Game(uint w, uint h) : base(w, h)
        {
            // You can do initialization stuff here, set title for example
            Title = "Rendering sample";
        }

        /*
         * MeowGame's Draw method is abstract, so we are forced to implement it.
         * This method is called when RedrawInterval is reached.
         * 
         * You can insert any drawing code you want here, MeowGame will also
         * create a spriteBatch and primitiveBatch for you, so use them.
         */
        protected override void Draw(int timePassed)
        {
		primitiveBatch.DrawTriangle(new Point(200, 100), new Point(100, 200), new Point(300, 200), Color.Red255);
		primitiveBatch.DrawRectangle(400, 100, 100, 100, Color.Blue255);
        }

        /*
         * MeowGame's Update method is abstract, so we are forced to
         * implement it in subclass.
         * This method is called when RefreshInterval is reached
         */ 
        protected override void Update(int timePassed)
        {
            
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}