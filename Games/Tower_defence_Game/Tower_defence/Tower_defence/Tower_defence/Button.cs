using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Tower_defence
{
    class Button: Non_interactive
    {
 
      

        public Button(Texture2D sheet, Vector2 pos, Rectangle src_rect) :base(sheet, pos)
        {
            this.src_rect = src_rect;

           
        }

        public virtual void Update(MouseState mouse)
        {

        }

        public bool is_clicked(MouseState mouse)
        {
            return Hitbox().Contains(new Point(mouse.X, mouse.Y)) && mouse.LeftButton == ButtonState.Pressed;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sheet_tex, pos, src_rect, Color.White);
        }

        public virtual Rectangle Hitbox()
        {
            return new Rectangle((int)pos.X , (int)pos.Y, src_rect.Width, src_rect.Height);
        }
    }
}
