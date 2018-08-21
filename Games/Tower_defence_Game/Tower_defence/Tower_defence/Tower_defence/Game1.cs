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

    public class Game1 : Microsoft.Xna.Framework.Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        //Gamestates
        public enum Game_state
        {
            title,
            help,
            level1,
            level2,
            level3,
            winning,
            game_over
        }
        public static Game_state current_state;
        

        //Textures
        Texture2D title_bkg;
        Texture2D help_bkg;
        Texture2D gameover_bkg;
        Texture2D winning_bkg;
        Texture2D grass_tex;
        Texture2D sheet_tex;
        public Texture2D road_tex;

        //Sounds and music
        Song title_song;
        Song in_game;
        public static SoundEffect explosion;
        
        //Spritefonts
        public SpriteFont Font;

        //Class instances
        Button play_btn;
        Button help_btn;
        Button back_btn;
        Button exit_btn;
        Button restart_btn;
        UI_Handle ui;
        Game_object_Manager go_manager;

        //Mouse logics
        public static MouseState mouse;
        MouseState old_mouse;
        Point mouse_point;

        //Bools
        bool in_game_help = false;

        //Lists
        public static List<Texture2D> p_textures;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            //Window size
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1220;
            graphics.ApplyChanges();

            IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Loads textures
            title_bkg = Content.Load<Texture2D>(@"Textures\bkg2");
            help_bkg = Content.Load<Texture2D>(@"Textures\help_bkg");
            gameover_bkg = Content.Load<Texture2D>(@"Textures\go1");
            winning_bkg = Content.Load<Texture2D>(@"Textures\fullscreen_bkg2");
            sheet_tex = Content.Load<Texture2D>(@"Textures\Spritesheet");
            road_tex = Content.Load<Texture2D>(@"Textures\road");
            grass_tex = Content.Load<Texture2D>(@"Textures\grass");

            //Particle Manager textures
            p_textures = new List<Texture2D>();
            p_textures.Add(Content.Load<Texture2D>(@"Textures\exp0"));
            p_textures.Add(Content.Load<Texture2D>(@"Textures\exp1"));
            p_textures.Add(Content.Load<Texture2D>(@"Textures\exp3"));

            //Loads sounds and music
            title_song = Content.Load<Song>(@"Sounds\td");
            in_game = Content.Load<Song>(@"Sounds\td_ingame");
            explosion = Content.Load<SoundEffect>(@"Sounds\118-Fire02");

            //Create instances of objects
            play_btn = new Button(sheet_tex, new Vector2(220, 250), new Rectangle(0,79, 328, 46));
            help_btn = new Button(sheet_tex, new Vector2(215, 350), new Rectangle(0, 124, 350, 48));
            back_btn = new Button(sheet_tex, new Vector2(1050, 660), new Rectangle(295, 174, 200, 48));
            exit_btn = new Button(sheet_tex, new Vector2(250, 450), new Rectangle(0, 171, 288, 48));
            restart_btn = new Button(sheet_tex, new Vector2(250, 560), new Rectangle(0, 780, 288, 150));
            ui = new UI_Handle(sheet_tex, new Vector2(1070, 0));
            go_manager = new Game_object_Manager(sheet_tex, Vector2.Zero, GraphicsDevice, road_tex, ui);

            //Loads fonts
            Font = Content.Load<SpriteFont>("font");

            //Starts playing music and sets different bools for it
            MediaPlayer.Play(title_song);
            MediaPlayer.IsRepeating = true;

            current_state = Game_state.title;
 
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            mouse = Mouse.GetState();
            mouse_point = new Point(mouse.X, mouse.Y);

            switch (current_state)
            {
                case Game_state.title:
                    {
 
                        if (play_btn.is_clicked(mouse))
                        {
                            current_state = Game_state.level1;
                            MediaPlayer.Play(in_game);
                        }
                        if (help_btn.is_clicked(mouse))
                        {
                            current_state = Game_state.help;
                        }
                        if (exit_btn.is_clicked(mouse))
                        {
                            this.Exit();
                        }
                    }
                    break;
                case Game_state.help:
                    {
                        if (back_btn.is_clicked(mouse))
                        {
                            current_state = Game_state.title;
                        }
                    }
                    break;
                case Game_state.level1:
                    {
                        if(Keyboard.GetState().IsKeyDown(Keys.F1) && !in_game_help)
                        {
                            in_game_help = true;
                        }
                        if (in_game_help && back_btn.is_clicked(mouse))
                        {
                            in_game_help = false;
                        }

                    }
                    break;
                case Game_state.level2:
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.F1) && !in_game_help)
                        {
                            in_game_help = true;
                        }
                        if (in_game_help && back_btn.is_clicked(mouse))
                        {
                            in_game_help = false;
                        }
                    }
                    break;
                case Game_state.level3:
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.F1) && !in_game_help)
                        {
                            in_game_help = true;
                        }
                        if (in_game_help && back_btn.is_clicked(mouse))
                        {
                            in_game_help = false;
                        }
                    }
                    break;
                case Game_state.winning:
                    {
                        if (restart_btn.is_clicked(mouse))
                        {
                            go_manager.enemies.Clear();
                            Game1.current_state = Game1.Game_state.level1;
                            ui.level = 1;
                            go_manager.interactive_towers.Clear();
                            go_manager.enemy_wave_start = false;
                            go_manager.enemy_wave_full = false;
                            ui.score = 0;
                            ui.gold = 50;
                            ui.base_hp = 20;
                            ui.enemies_killed = 0;
                            go_manager.enemy_amount = 0;
                            go_manager.tower_placed_amount = 0;
                        }
                    }
                    break;
                case Game_state.game_over:
                    {
                        if (restart_btn.is_clicked(mouse))
                        {
                            go_manager.enemies.Clear();
                            Game1.current_state = Game1.Game_state.level1;
                            ui.level = 1;
                            go_manager.interactive_towers.Clear();
                            go_manager.enemy_wave_start = false;
                            go_manager.enemy_wave_full = false;
                            ui.score = 0;
                            ui.gold = 50;
                            ui.base_hp = 20;
                            ui.enemies_killed = 0;
                            go_manager.enemy_amount = 0;
                            go_manager.tower_placed_amount = 0;
                        }
                    }
                    break;
            }
            
            //Updates all instances
            play_btn.Update(mouse);
            help_btn.Update(mouse);
            exit_btn.Update(mouse);
            back_btn.Update(mouse);
            restart_btn.Update(mouse);
            go_manager.Update(go_manager.layer);
            old_mouse = mouse;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            
            switch (current_state)
            {
                case Game_state.title:
                    {
                        //Draw title screen and its buttons
                        spriteBatch.Draw(title_bkg, Vector2.Zero, Color.White);
                        play_btn.Draw(spriteBatch);
                        help_btn.Draw(spriteBatch);
                        exit_btn.Draw(spriteBatch);
                        
                    }
                    break;
                case Game_state.help:
                    {
                        spriteBatch.Draw(help_bkg, Vector2.Zero, Color.White);
                        back_btn.Draw(spriteBatch);

                    }
                    break;
                case Game_state.level1:
                    {
                        //Draw objects
                        spriteBatch.Draw(grass_tex, Vector2.Zero, Color.White);
                        spriteBatch.Draw(road_tex, Vector2.Zero, Color.White);
                        ui.Draw(spriteBatch);
                        go_manager.Draw(spriteBatch);
                     
                        
                        //UI Parameters:
                        spriteBatch.DrawString(Font, "" + ui.score, new Vector2(ui.pos.X + 59, ui.pos.Y + 285), Color.Red);
                        spriteBatch.DrawString(Font, "" + ui.base_hp, new Vector2(ui.pos.X + 55, ui.pos.Y + 385), Color.Red);
                        spriteBatch.DrawString(Font, "" + ui.level, new Vector2(ui.pos.X + 60, ui.pos.Y + 475), Color.Red);
                        spriteBatch.DrawString(Font, "" + ui.gold, new Vector2(ui.pos.X + 60, ui.pos.Y + 540), Color.Red);
                        spriteBatch.DrawString(Font, "" + ui.enemies_killed, new Vector2(ui.pos.X + 60, ui.pos.Y + 600), Color.Red);

                        
                        //Draw help texture if bool in_game_help is true (by clicking F1)
                        if(in_game_help == true)
                        {
                            spriteBatch.Draw(help_bkg, Vector2.Zero, Color.White);
                            back_btn.Draw(spriteBatch);
                        }

                    }
                    break;
                case Game_state.level2:
                    {
                        //Draw objects
                        spriteBatch.Draw(grass_tex, Vector2.Zero, Color.White);
                        spriteBatch.Draw(road_tex, Vector2.Zero, Color.White);
                        ui.Draw(spriteBatch);
                        go_manager.Draw(spriteBatch);


                        //UI Parameters:
                        spriteBatch.DrawString(Font, "" + ui.score, new Vector2(ui.pos.X + 59, ui.pos.Y + 285), Color.Red);
                        spriteBatch.DrawString(Font, "" + ui.base_hp, new Vector2(ui.pos.X + 55, ui.pos.Y + 385), Color.Red);
                        spriteBatch.DrawString(Font, "" + ui.level, new Vector2(ui.pos.X + 60, ui.pos.Y + 475), Color.Red);
                        spriteBatch.DrawString(Font, "" + ui.gold, new Vector2(ui.pos.X + 60, ui.pos.Y + 540), Color.Red);
                        spriteBatch.DrawString(Font, "" + ui.enemies_killed, new Vector2(ui.pos.X + 60, ui.pos.Y + 600), Color.Red);

                        //Draw help texture if bool in_game_help is true (by clicking F1)
                        if (in_game_help == true)
                        {
                            spriteBatch.Draw(help_bkg, Vector2.Zero, Color.White);
                            back_btn.Draw(spriteBatch);
                        }

                    }
                    break;
                case Game_state.level3:
                    {
                        //Draw objects
                        spriteBatch.Draw(grass_tex, Vector2.Zero, Color.White);
                        spriteBatch.Draw(road_tex, Vector2.Zero, Color.White);
                        ui.Draw(spriteBatch);
                        go_manager.Draw(spriteBatch);


                        //UI Parameters:
                        spriteBatch.DrawString(Font, "" + ui.score, new Vector2(ui.pos.X + 59, ui.pos.Y + 285), Color.Red);
                        spriteBatch.DrawString(Font, "" + ui.base_hp, new Vector2(ui.pos.X + 55, ui.pos.Y + 385), Color.Red);
                        spriteBatch.DrawString(Font, "" + ui.level, new Vector2(ui.pos.X + 60, ui.pos.Y + 475), Color.Red);
                        spriteBatch.DrawString(Font, "" + ui.gold, new Vector2(ui.pos.X + 60, ui.pos.Y + 540), Color.Red);
                        spriteBatch.DrawString(Font, "" + ui.enemies_killed, new Vector2(ui.pos.X + 60, ui.pos.Y + 600), Color.Red);

                        //Draw help texture if bool in_game_help is true (by clicking F1)
                        if (in_game_help == true)
                        {
                            spriteBatch.Draw(help_bkg, Vector2.Zero, Color.White);
                            back_btn.Draw(spriteBatch);
                        }

                    }
                    break;
                case Game_state.game_over:
                    {
                        spriteBatch.Draw(gameover_bkg, Vector2.Zero, Color.White);
                        restart_btn.Draw(spriteBatch);

                    }
                    break;
                case Game_state.winning:
                    {
                        spriteBatch.Draw(winning_bkg, Vector2.Zero, Color.White);
                        restart_btn.Draw(spriteBatch);

                    }
                    break;
            }

           
            spriteBatch.End();

            base.Draw(gameTime);
        }
        
    }
}
