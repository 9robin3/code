using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spline;

namespace Tower_defence
{
    class Enemy: Interactive
    {

        public Point origin;
        public float curve_pos = 0;
        public int enemy_health;

        private SimplePath path;



        public Enemy(Texture2D sheet_tex, Vector2 pos): base(sheet_tex,pos)
        {
            this.sheet_tex = sheet_tex;
            this.pos = pos;
            hit_box = Get_hit_box();
           
        }

        public void Set_enemy_type()
        {
            if (Game1.current_state == Game1.Game_state.level1)
            {
                src_rect = new Rectangle(333, 0, 64, 72);
                origin = new Point(src_rect.Width / 2, src_rect.Height / 2);
                enemy_health = 8;
            }
            if (Game1.current_state == Game1.Game_state.level2)
            {
                src_rect = new Rectangle(415, 0, 64, 72);
                origin = new Point(src_rect.Width / 2, src_rect.Height / 2);
                enemy_health = 12;
            }
            if (Game1.current_state == Game1.Game_state.level3)
            {
                src_rect = new Rectangle(490, 0, 74, 72);
                origin = new Point(src_rect.Width / 2, src_rect.Height / 2);
                enemy_health = 20;
            }
           
        }

        public void get_path(SimplePath path)
        {
            this.path = path;
            
        }
        public override void Update()
        {
            if(Game1.current_state == Game1.Game_state.level1)
            {
            curve_pos++;
            pos.X = path.GetPos(curve_pos).X - origin.X;
            pos.Y = path.GetPos(curve_pos).Y - origin.Y;
            }

            if (Game1.current_state == Game1.Game_state.level2)
            {
                curve_pos += 2;
                pos.X = path.GetPos(curve_pos).X - origin.X;
                pos.Y = path.GetPos(curve_pos).Y - origin.Y;
            }

            if (Game1.current_state == Game1.Game_state.level3)
            {
                curve_pos += 5;
                pos.X = path.GetPos(curve_pos).X - origin.X;
                pos.Y = path.GetPos(curve_pos).Y - origin.Y;
            }

            hit_box = Get_hit_box();

       
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sheet_tex, pos, src_rect, Color.White);
        }
    }
}
