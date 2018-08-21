using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tower_defence
{
    abstract class Game_object
    {
        public Texture2D sheet_tex;
        public Vector2 pos;
        public Rectangle src_rect;
        public Rectangle hit_box;


        public Game_object(Texture2D sheet_tex, Vector2 pos)
        {
            this.sheet_tex = sheet_tex;
            this.pos = pos;
        }

        public abstract void Update();

        public abstract void Draw(SpriteBatch spriteBatch);

    }
}
