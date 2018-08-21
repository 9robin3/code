using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tower_defence
{
    class UI_Handle: Non_interactive
    {
        public int score;
        public int base_hp;
        public int level;
        public int gold;
        public int enemies_killed;
        public int tower_hp = 20;


        public UI_Handle(Texture2D sheet, Vector2 pos) : base(sheet, pos)
        {
            src_rect = new Rectangle(740, 0, 150, 720);
            //Default values when game start. Some are changed according to level in GO_Manager
            score = 0;
            base_hp = 20;
            level = 1;
            gold = 50;
            enemies_killed = 0;


        }

        public override void Update()
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sheet_tex, pos, src_rect, Color.White);
        }
    }
}
