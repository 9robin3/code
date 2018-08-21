using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tower_defence
{
    class Tower: Interactive
    {


        public Bullet bullet1;
        public Bullet bullet2;
        public Bullet bullet3;
        
        public float bullet_interval;
        
        public List<Bullet> bullets;


        public enum Tower_state
        {
            tower1,
            tower2,
            tower3
        }
        public Tower_state tower_state;

        public Tower(Texture2D sheet, Vector2 pos, Rectangle src_rect) :base(sheet, pos)
        {
            this.src_rect = src_rect;
            bullet_interval = 100;
            bullets = new List<Bullet>();
            hit_box = Get_hit_box();

        }

        public void Create_bullet()
        {

            if (bullet_interval <= 0)
            {
                if (tower_state == Tower_state.tower1)
                {
                    bullet1 = new Bullet(sheet_tex, new Vector2(pos.X + 17, pos.Y), new Rectangle(140, 8, 10, 10));
                    bullet2 = new Bullet(sheet_tex, new Vector2(pos.X + 17, pos.Y), new Rectangle(140, 8, 10, 10));
                    bullet1.Speed = new Vector2(0, -1);
                    bullet2.Speed = new Vector2(0, 1);
                    bullets.Add(bullet1);
                    bullets.Add(bullet2);

                }
                else if (tower_state == Tower_state.tower2)
                {
                    bullet1 = new Bullet(sheet_tex, new Vector2(pos.X + 15, pos.Y), new Rectangle(135, 18, 20, 20));
                    bullet2 = new Bullet(sheet_tex, new Vector2(pos.X + 15, pos.Y), new Rectangle(135, 18, 20, 20));
                    bullet1.Speed = new Vector2(3, 3);
                    bullet2.Speed = new Vector2(-3, 3);
                    bullets.Add(bullet1);
                    bullets.Add(bullet2);
                }
                else if (tower_state == Tower_state.tower3)
                {
                    bullet1 = new Bullet(sheet_tex, new Vector2(pos.X + 15, pos.Y), new Rectangle(134, 42, 25, 25));
                    bullet2 = new Bullet(sheet_tex, new Vector2(pos.X + 15, pos.Y), new Rectangle(134, 42, 25, 25));
                    bullet3 = new Bullet(sheet_tex, new Vector2(pos.X + 15, pos.Y), new Rectangle(134, 18, 25, 25));
                    bullet1.Speed = new Vector2(5, 0);
                    bullet2.Speed = new Vector2(-5, 0);
                    bullet3.Speed = new Vector2(0, 4);
                    bullets.Add(bullet1);
                    bullets.Add(bullet2);
                    bullets.Add(bullet3);
                }
               
                bullet_interval = 100;
            }
               
        }

        public List<Bullet> Get_bullets()
        {
            return bullets;
        }

        public override void Update()
        {
            bullet_interval--;

            for (int i = 0; i < bullets.Count; i++)
            {
                if (bullets[i].bullet_life_length <= 0)
                {
                    bullets.RemoveAt(i);
                }
                
            }
            foreach (Bullet b in bullets)
            {
                if (bullets != null)
                {
                    b.Update();
                }
            }
           hit_box = Get_hit_box();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sheet_tex, pos, src_rect, Color.White);
            foreach (Bullet b in bullets)
            {
                if (b != null)
                {
                    b.Draw(spriteBatch);
                }
            }
       
        }
    }
}
