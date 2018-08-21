using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tower_defence
{
    abstract class Interactive: Game_object
    {
        
        public Vector2 speed;


        public Interactive(Texture2D sheet, Vector2 pos): base(sheet, pos)
        {
            hit_box = Get_hit_box();
        }

        public override void Update()
        {
            
        }

        public void Set_position(Vector2 pos)
        {
            this.pos = pos;
            hit_box = Get_hit_box();
        }

        public Rectangle Get_hit_box()
        {
           return new Rectangle((int)pos.X, (int)pos.Y, src_rect.Width, src_rect.Height);
        }
    }
}
