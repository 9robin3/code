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

namespace Sänka_skepp
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {

         public enum GameState
        {
            title_screen,
            placing_ships1,
            placing_ships2,
            player1,
            player2,
            game_over
        }
        public static GameState current_state;
        GraphicsDeviceManager graphics;
        

        //Define sprites
        SpriteBatch spriteBatch;
        Texture2D bkg;
        Texture2D bkg_game_over;
        Texture2D bkg_in_game;
        Texture2D title_font;
        Texture2D spritesheet;
    
        Texture2D arrow1;
        Texture2D arrow2;
        Texture2D place_txt;
        Texture2D shoot_txt;
        Texture2D pl1_turn;
        Texture2D pl2_turn;
        Texture2D enter_to;

        Texture2D pl1_wins;
        Texture2D pl2_wins; 

        SpriteFont font1;

        //Define player and and 2's grid and their bounding box for each grid
        Matrixes player1;
        Rectangle player_bounds1;
        Matrixes player2;
        Rectangle player_bounds2;

        KeyboardState keyboard, old_keyboard;
        Tile tile = new Tile();

        Ship ship1 = new Ship();

        Matrixes hit = new Matrixes(); 

        //Define sounds and music
        Song music;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1220;
            graphics.ApplyChanges();

            IsMouseVisible = true;

            base.Initialize();
        }


        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            bkg = Content.Load<Texture2D>(@"Bkg_title");
            bkg_game_over = Content.Load<Texture2D>(@"game_over1");
            bkg_in_game = Content.Load<Texture2D>(@"Bkg");
            title_font = Content.Load<Texture2D>(@"title_font");
            spritesheet = Content.Load<Texture2D>(@"spritesheet1");
           
            arrow1 = Content.Load<Texture2D>(@"arrow1");
            arrow2 = Content.Load<Texture2D>(@"arrow2");
            
            place_txt = Content.Load<Texture2D>(@"place_text");
            shoot_txt = Content.Load<Texture2D>(@"shoot_text");

            pl1_turn = Content.Load<Texture2D>(@"player1turn");
            pl2_turn = Content.Load<Texture2D>(@"player2turn");
            
            enter_to = Content.Load<Texture2D>(@"enter_to");

            pl1_wins = Content.Load<Texture2D>(@"pl1_wins");
            pl2_wins = Content.Load<Texture2D>(@"pl2_wins");


            font1 = Content.Load<SpriteFont>(@"SpriteFont1"); 

            music = Content.Load<Song>(@"calm");

            MediaPlayer.Play(music);
            MediaPlayer.IsRepeating = true;

            player1 = new Matrixes(10, spritesheet, new Vector2(50, 150));
            player_bounds1 = new Rectangle(50, 150, 500, 500);
            player2 = new Matrixes(10, spritesheet, new Vector2(650, 150));
            player_bounds2 = new Rectangle(650, 150, 500, 500);

            //Bestäm start-tillstånd
            current_state = GameState.title_screen;
            

        }

        protected override void Update(GameTime gameTime)
        {

            keyboard = Keyboard.GetState(); 

            switch (current_state)
            {
                case GameState.title_screen:
                    if (keyboard.IsKeyDown(Keys.Enter))
                    {
                        current_state = GameState.placing_ships1;
                    }
                    break;
                case GameState.placing_ships1:
                    {
                        
                        player1.Update(Mouse.GetState());

                        if (keyboard.IsKeyDown(Keys.Enter) && !old_keyboard.IsKeyDown(Keys.Enter))
                        {
                            bool ship_held = false;
                            
                            foreach (Ship s in player1.ships)
                                if (s.picked_up)
                                    ship_held = true; ;
                            if (!ship_held)
                                current_state = GameState.placing_ships2;
                        }
                    }
                   break;
                case GameState.placing_ships2:
                   {
                       player2.Update(Mouse.GetState());
                       if (keyboard.IsKeyDown(Keys.Enter) && !old_keyboard.IsKeyDown(Keys.Enter))
                       {
                           bool ship_held = false;

                           foreach (Ship s in player2.ships)
                               if (s.picked_up)
                                   ship_held = true; ;
                           if (!ship_held)
                           {
                               player1.mode = true;
                               player2.Update(Mouse.GetState());
                               current_state = GameState.player1;
                           }
                       }
                   }
                    break;
                case GameState.player1:
                    {
     
                            player2.mode = true;
                            if (player2.Update(Mouse.GetState()))
                            {  
                                current_state = GameState.player2;
                            }
                            if (player1.winning == true)
                            {
                                current_state = GameState.game_over;
                            }
                    }
                                      
                    break;
                case GameState.player2:
                    {
                        player1.mode = true;
                        if (player1.Update(Mouse.GetState()))
                        {
                            current_state = GameState.player1;
                        }
                        if (player2.winning == true)
                        {
                            current_state = GameState.game_over;
                        }
                    }
                    break;
                case GameState.game_over:
                    {

                    }
                    break; 
                    

            }
            old_keyboard = keyboard; 

            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

                switch (current_state)
                {

                    case GameState.title_screen:
                        {
                            spriteBatch.Draw(bkg, Vector2.Zero, Color.White);
                      
                        }
                        break;
                    case GameState.placing_ships1:
                        {

                            spriteBatch.Draw(bkg_in_game, Vector2.Zero, Color.White); 
                            spriteBatch.Draw(title_font, new Vector2(435, 50), Color.White);
                            spriteBatch.Draw(pl1_turn, new Vector2(675, 300), Color.White);
                            spriteBatch.Draw(place_txt, new Vector2(670, 350), Color.White);
                            spriteBatch.Draw(arrow1, new Vector2(600, 350), Color.White);
                            spriteBatch.Draw(enter_to, new Vector2(670, 500), Color.White);
                            player1.Draw(spriteBatch);

                        }
                        break;
                    case GameState.placing_ships2:
                        {
                            spriteBatch.Draw(bkg_in_game, Vector2.Zero, Color.White); 
                            spriteBatch.Draw(title_font, new Vector2(435, 50), Color.White);
                            spriteBatch.Draw(pl2_turn, new Vector2(105, 300), Color.White);
                            spriteBatch.Draw(place_txt, new Vector2(100, 350), Color.White);
                            spriteBatch.Draw(arrow2, new Vector2(550, 350), Color.White);
                            spriteBatch.Draw(enter_to, new Vector2(100, 500), Color.White);
                            player2.Draw(spriteBatch);

                        }
                        break;
                    case GameState.player1:
                        {
                           
                            spriteBatch.Draw(bkg_in_game, Vector2.Zero, Color.White);
                            spriteBatch.Draw(title_font, new Vector2(435, 50), Color.White);
                            spriteBatch.Draw(pl1_turn, new Vector2(105, 300), Color.White);
                            spriteBatch.Draw(shoot_txt, new Vector2(100, 350), Color.White);
                            spriteBatch.Draw(arrow2, new Vector2(550, 350), Color.White);
                            player2.Draw(spriteBatch);

                            //if (player1.winning == true)
                            //{
                            //    spriteBatch.Draw(pl1_wins, new Vector2(300, 300), Color.White);
                            //}
                   
                        }
                        break;
                    case GameState.player2:
                        {
                            spriteBatch.Draw(bkg_in_game, Vector2.Zero, Color.White);
                            spriteBatch.Draw(title_font, new Vector2(435, 50), Color.White);
                            spriteBatch.Draw(pl2_turn, new Vector2(675, 300), Color.White);
                            spriteBatch.Draw(shoot_txt, new Vector2(670, 350), Color.White);
                            spriteBatch.Draw(arrow1, new Vector2(600, 350), Color.White);
                            player1.Draw(spriteBatch);

                            //if (player2.winning == true)
                            //{
                            //    spriteBatch.Draw(pl2_wins, new Vector2(300, 300), Color.White);
                            //    spriteBatch.Draw(title_font, new Vector2(435, 50), Color.White);
                            //}
                        }
                        break; 
                    case GameState.game_over:
                        {
                            spriteBatch.Draw(bkg_in_game, Vector2.Zero, Color.White);
                            if(player1.winning == true)
                            {
                                spriteBatch.Draw(pl2_wins, new Vector2(300, 300), Color.White);
                            }
                            if (player2.winning == true)
                            {
                                spriteBatch.Draw(pl1_wins, new Vector2(300, 300), Color.White);
                            }
                        }
                        break;
                          default:
                             break;

               
                }

            spriteBatch.End();
            base.Draw(gameTime);

            }

        }

    }


