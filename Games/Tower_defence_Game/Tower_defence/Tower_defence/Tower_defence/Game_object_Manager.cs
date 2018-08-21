using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Spline;

namespace Tower_defence
{

    class Game_object_Manager
    {

        GraphicsDevice graphics;
        public RenderTarget2D layer;

        //Textures
        Texture2D sheet_tex;
        Texture2D road_tex;

        //Floats
        float enemy_interval;
        float p_manager_life_length;

        //Ints 
        public int enemy_amount;
        public int tower_placed_amount;

        //Vectors
        Vector2 pos;

        //Streamreaders, writers and strings
        StreamReader sr;

        //Lists and arrays
        public List<Enemy> enemies;
        public List<Tower> menu_towers;
        public List<Tower> interactive_towers;
        List<Bullet> go_bullets;

        //Bools
        public bool tower_is_picked_up;
        public bool tower_dropped;
        public bool enemy_wave_start;
        public bool enemy_wave_full;

        //Mouse logic
        MouseState mouse;
        MouseState old_mouse;
        Point mouse_point;

        //Instances
        Tower menu_tower1;
        Tower menu_tower2;
        Tower menu_tower3;
        public Tower interactive_tower;
        Enemy enemy;
        SimplePath path;
        UI_Handle ui;
        Particle_Manager p_manager;

        public Game_object_Manager(Texture2D sheet_tex, Vector2 pos, GraphicsDevice graphics, Texture2D road_tex, UI_Handle ui)
        {

            this.graphics = graphics;
            this.sheet_tex = sheet_tex;
            this.pos = pos;
            this.road_tex = road_tex;
            this.ui = ui;

            //Calls method
            Create_level();

        }

        private void Create_level()
        {
            //Creates lists
            enemies = new List<Enemy>();
            menu_towers = new List<Tower>();
            interactive_towers = new List<Tower>();

            //Sets int and float values
            enemy_amount = 0;
            tower_placed_amount = 0;
            enemy_interval = 100;

            //Creates instances
            menu_tower1 = new Tower(sheet_tex, new Vector2(1080, 50), new Rectangle(0, 0, 45, 70));
            menu_tower2 = new Tower(sheet_tex, new Vector2(1123, 50), new Rectangle(45, 0, 45, 70));
            menu_tower3 = new Tower(sheet_tex, new Vector2(1167, 50), new Rectangle(90, 0, 45, 70));
            path = new SimplePath(graphics);
            layer = new RenderTarget2D(graphics, 1220, 720);
            menu_towers.Add(menu_tower1);
            menu_towers.Add(menu_tower2);
            menu_towers.Add(menu_tower3);

            //Creates steamreader
            sr = new StreamReader("Level1.txt");

            path.Clean();

            //Reads and creates spline points from text file
            while (!sr.EndOfStream)
            {
                string s = sr.ReadLine();
                int x = int.Parse(s.Split(',')[0]);
                int y = int.Parse(s.Split(',')[1]);
                path.AddPoint(new Vector2(x, y));
            }

            Draw_rendertarget();
        }


        private void Create_enemy()
        {

            enemy = new Enemy(sheet_tex, path.GetPos(path.beginT));
            enemy.pos.X -= enemy.origin.X;
            enemy.pos.Y -= enemy.origin.Y;
            enemy_amount++;
            enemies.Add(enemy);
            enemy_interval = 100;
            enemy.Set_enemy_type();

        }

        private void Create_tower()
        {
            mouse = Mouse.GetState();
            mouse_point = new Point(mouse.X, mouse.Y);

            //Checks when you can pick a tower from the menu
            if (Game1.current_state != Game1.Game_state.title || Game1.current_state != Game1.Game_state.help || Game1.current_state != Game1.Game_state.game_over)
            {

                //Creates tower type 1 
                if (menu_tower1.hit_box.Contains(mouse_point) && mouse.LeftButton == ButtonState.Pressed && old_mouse.LeftButton == ButtonState.Released
                    && ui.gold >= 10 && !tower_is_picked_up && !enemy_wave_start)
                {
                    interactive_tower = new Tower(sheet_tex, new Vector2(mouse.X, mouse.Y), new Rectangle(0, 0, 45, 70));
                    interactive_tower.tower_state = Tower.Tower_state.tower1;
                    interactive_towers.Add(interactive_tower);
                    tower_is_picked_up = true;
                    ui.gold -= 10;

                }

                //Creates tower type 2 
                else if (menu_tower2.hit_box.Contains(mouse_point) && mouse.LeftButton == ButtonState.Pressed && old_mouse.LeftButton == ButtonState.Released
                    && ui.gold >= 30 && !tower_is_picked_up && !enemy_wave_start)
                {
                    interactive_tower = new Tower(sheet_tex, new Vector2(mouse.X, mouse.Y), new Rectangle(45, 0, 45, 70));
                    interactive_tower.tower_state = Tower.Tower_state.tower2;
                    interactive_towers.Add(interactive_tower);
                    tower_is_picked_up = true;
                    ui.gold -= 30;
                }

                ////Creates tower type 3 
                else if (menu_tower3.hit_box.Contains(mouse_point) && mouse.LeftButton == ButtonState.Pressed && old_mouse.LeftButton == ButtonState.Released
                    && ui.gold >= 50 && !tower_is_picked_up && !enemy_wave_start)
                {
                    interactive_tower = new Tower(sheet_tex, new Vector2(mouse.X, mouse.Y), new Rectangle(90, 0, 45, 70));
                    interactive_tower.tower_state = Tower.Tower_state.tower3;
                    interactive_towers.Add(interactive_tower);
                    tower_is_picked_up = true;
                    ui.gold -= 50;
                }


               //Checks where you can place towers

                else if (mouse.LeftButton == ButtonState.Pressed && old_mouse.LeftButton == ButtonState.Released &&
                             mouse.X < 1025 && tower_is_picked_up && Can_place(layer, interactive_tower))
                {
                    if (mouse.X > 0 && mouse.X < 1180 && mouse.Y > 10 && mouse.Y < 650)
                    {

                        interactive_tower.Set_position(new Vector2(mouse.X, mouse.Y));
                        tower_is_picked_up = false;
                        tower_dropped = true;
                        tower_placed_amount++;
                        Draw_rendertarget();

                    }
                }

                //Updates picked up towers to the position of the cursor
                if (tower_is_picked_up)
                {
                    interactive_tower.Set_position(new Vector2(mouse.X, mouse.Y));
                }

            }
            old_mouse = mouse;
        }

        private void Collision_check()
        {

            //Collision: Enemy with base
            foreach (Enemy e in enemies)
            {
                if (e.curve_pos >= (2800))
                {
                    e.curve_pos = path.beginT;
                    p_manager = new Particle_Manager(Game1.p_textures, e.pos);
                    p_manager_life_length = 25;
                    ui.base_hp -= 1;
                }

            }

            //Collision: Bullet with enemy
            for (int i = 0; i < interactive_towers.Count; i++)
            {
                go_bullets = interactive_towers[i].Get_bullets();

                for (int j = 0; j < go_bullets.Count; j++)
                {
                    for (int k = 0; k < enemies.Count; k++)
                    {
                        if (go_bullets[j].hit_box.Intersects(enemies[k].hit_box))
                        {
                            enemies[k].enemy_health--;
                            go_bullets.RemoveAt(j);
                            break;
                        }

                    }
                }

            }
        }


        public void Update(RenderTarget2D layer)
        {
            enemy_interval--;

            p_manager_life_length--;

            //Calls methods
            if (!enemy_wave_start)
            {
                Create_tower();
            }
            Collision_check();

            //Makes enemies begin to walk along the path when enter button is hit and towers placed are >= 1:
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && tower_placed_amount >= 1 && !enemy_wave_start)
            {
                enemy_wave_start = true;
            }

            //Checks if towers are dropped/placed, if enemies are >= 1 and if bullet timer <= 0. If so create bullets:
            if (!tower_is_picked_up && tower_dropped && enemy_wave_start && enemy_amount >= 1)
            {
                foreach (Tower i in interactive_towers)
                {
                    i.Create_bullet();
                    i.Update();
                }
            }

            //Creates an enemy if enter was hit and enemy amount is less than 10
            if (enemy_interval <= 0 && enemy_wave_start && !enemy_wave_full)
            {
                Create_enemy();
            }
            if (enemy_amount == 10)
            {
                enemy_wave_full = true;
            }

            //Calls Enemy class's get_path, which makes it move along the curve
            foreach (Enemy e in enemies)
            {
                e.get_path(path);

            }

            //Checks if enemy health is <= 0 and remove it from the list if so
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].enemy_health <= 0)
                {
                    p_manager = new Particle_Manager(Game1.p_textures, enemies[i].pos);
                    p_manager_life_length = 25;
                    Game1.explosion.Play();
                    enemies.RemoveAt(i);
                    ui.enemies_killed++;
                    ui.score += 10;
                    ui.gold += 10;
                    enemy_amount--;
                    break;
                }
            }

            //Checks if 10 enemies are killed. If so, proceed to next level
            if (ui.enemies_killed == 10 && tower_placed_amount > 0 && Game1.current_state == Game1.Game_state.level1)
            {
                enemies.Clear();
                Game1.current_state = Game1.Game_state.level2;
                ui.level++;
                interactive_towers.Clear();
                enemy_wave_start = false;
                enemy_wave_full = false;
                ui.enemies_killed = 0;
                enemy_amount = 0;
                tower_placed_amount = 0;
                graphics.SetRenderTarget(null);
                Draw_rendertarget();

            }

            if (ui.enemies_killed >= 10 && tower_placed_amount > 0 && Game1.current_state == Game1.Game_state.level2)
            {
                enemies.Clear();
                Game1.current_state = Game1.Game_state.level3;
                ui.level++;
                interactive_towers.Clear();
                enemy_wave_start = false;
                enemy_wave_full = false;
                ui.enemies_killed = 0;
                enemy_amount = 0;
                tower_placed_amount = 0;
                graphics.SetRenderTarget(null);
                Draw_rendertarget();

            }

            if (ui.enemies_killed >= 10 && tower_placed_amount > 0 && Game1.current_state == Game1.Game_state.level3)
            {
                enemies.Clear();
                Game1.current_state = Game1.Game_state.winning;
                interactive_towers.Clear();
                enemy_wave_start = false;
                enemy_wave_full = false;
                ui.enemies_killed = 0;
                enemy_amount = 0;
                tower_placed_amount = 0;
            }


            //Change to Game Over state if base HP <= 0
            if (ui.base_hp <= 0)
            {
                Game1.current_state = Game1.Game_state.game_over;
                enemies.Clear();
                interactive_towers.Clear();
            }


            //Update instances
            foreach (Tower i in interactive_towers)
            {
                i.Update();
            }
            foreach (Tower mt in menu_towers)
            {
                mt.Update();
            }
            foreach (Enemy e in enemies)
            {
                e.Update();
            }
            if (p_manager != null)
            {
                p_manager.Update();
            }
            ui.Update();
            old_mouse = mouse;
        }


        public void Draw_rendertarget()
        {
            SpriteBatch spriteBatch = new SpriteBatch(graphics);
            graphics.SetRenderTarget(layer);
            graphics.Clear(Color.Transparent);
            spriteBatch.Begin();
            spriteBatch.Draw(road_tex, Vector2.Zero, Color.White);

            foreach (Tower i in interactive_towers)
            {
                i.Draw(spriteBatch);
            }
            spriteBatch.End();

            graphics.SetRenderTarget(null);
        }


        public virtual void Draw(SpriteBatch spriteBatch)
        {

            //Draws all objects in the game
            foreach (Tower mt in menu_towers)
            {
                mt.Draw(spriteBatch);
            }
            foreach (Enemy e in enemies)
            {
                e.Draw(spriteBatch);
            }
            foreach (Tower i in interactive_towers)
            {
                i.Draw(spriteBatch);
            }
            if (p_manager != null && p_manager_life_length > 0)
                p_manager.Draw(spriteBatch);

            //Draws level number image and other font textures
            if (Game1.current_state == Game1.Game_state.level1 && !enemy_wave_start && !tower_is_picked_up && tower_placed_amount == 0)
            {
                spriteBatch.Draw(sheet_tex, new Vector2(370, 250), new Rectangle(0, 222, 375, 115), Color.White);
                spriteBatch.Draw(sheet_tex, new Vector2(370, 400), new Rectangle(0, 622, 375, 115), Color.White);
            }
            else if (Game1.current_state == Game1.Game_state.level2 && !enemy_wave_start && !tower_is_picked_up && tower_placed_amount == 0)
            {
                spriteBatch.Draw(sheet_tex, new Vector2(370, 250), new Rectangle(0, 335, 375, 115), Color.White);
                spriteBatch.Draw(sheet_tex, new Vector2(370, 400), new Rectangle(0, 622, 375, 115), Color.White);
            }
            else if (Game1.current_state == Game1.Game_state.level3 && !enemy_wave_start && !tower_is_picked_up && tower_placed_amount == 0)
            {
                spriteBatch.Draw(sheet_tex, new Vector2(370, 250), new Rectangle(0, 444, 375, 115), Color.White);
                spriteBatch.Draw(sheet_tex, new Vector2(370, 400), new Rectangle(0, 622, 375, 115), Color.White);
            }
            if (tower_placed_amount >= 1 && !enemy_wave_start)
            {
                spriteBatch.Draw(sheet_tex, new Vector2(370, 0), new Rectangle(0, 560, 375, 60), Color.White);
            }



        }

        public bool Can_place(RenderTarget2D layer, Tower t)
        {

                Color[] towerPixels = new Color[t.src_rect.Width * t.src_rect.Height];
                Color[] layerPixels = new Color[t.src_rect.Width * t.src_rect.Height];
                t.sheet_tex.GetData(0, t.src_rect, towerPixels, 0, towerPixels.Length);

                if (mouse.X > 0 && mouse.X < 1220 && mouse.Y > 0 && mouse.Y < 650)
                {
                    layer.GetData(0, t.Get_hit_box(), layerPixels, 0, layerPixels.Length);
                }

                for (int i = 0; i < towerPixels.Length; ++i)
                {
                    if (towerPixels[i].A > 0.0 && layerPixels[i].A > 0.0)
                        return false;
                }
                return true;
            }
 
        }
    }



        
       
