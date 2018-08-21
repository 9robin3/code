using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tower_defence
{
    abstract class Non_interactive: Game_object
    {
        public Non_interactive(Texture2D sheet, Vector2 pos): base(sheet,pos)
        {

        }

        public override void Update()
        {
            
        }


    }
}
