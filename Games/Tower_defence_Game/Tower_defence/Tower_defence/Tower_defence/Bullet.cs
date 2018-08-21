using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tower_defence
{
    class Bullet: Interactive
    {

        public int bullet_life_length;

        public Vector2 Speed
        {
            get 
            { 
                return speed; 
            }
            set
            {
                speed = value;
            }
        }

        public Bullet(Texture2D sheet_tex, Vector2 pos, Rectangle src_rect): base(sheet_tex, pos)
        {
            this.src_rect = src_rect;
            bullet_life_length = 350;
            hit_box = Get_hit_box();

        }

        public override void Update()
        {
            bullet_life_length--;
            pos += speed;
            hit_box = Get_hit_box();

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
  
            spriteBatch.Draw(sheet_tex, pos, src_rect, Color.White);
        }
    }
 }

