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
    class Matrixes
    {

        public bool mode = false;
        public bool ship_hit = false;
        public Vector2 position;

        Tile[,] tiles;

        Tile tile = new Tile(); 
        public Ship[] ships;

        public Rectangle player_bounds; 

        Point mouse_point;
        MouseState mouse; 
        MouseState old_mouse;

        public int counter;

        public bool winning; 

        public Matrixes()
        {
        }

        public Matrixes(int size, Texture2D texture, Vector2 position)
        {
            this.position = new Vector2(position.X, position.Y);

            tiles = new Tile[size, size];
            ships = new Ship[5];

            player_bounds = new Rectangle((int)position.X, (int)position.Y, 500, 500);

            mouse = Mouse.GetState();
            mouse_point = new Point(mouse.X, mouse.Y);

            counter = 0; 


            int i;
            int j;
            int k = ships.Length;

            //Tiles loop (creates the matrixes)
            for (i = 0; i < size; i++)
            {

                for (j = 0; j < size; j++)
                {
                    tiles[i, j] = new Tile(new Vector2(i * 50 + position.X, j * 50 + position.Y), texture);


                }

            }

            //Ships loop (creates the ships)

            ships[0] = new Ship(new Vector2(k + position.X, position.Y), texture, new Rectangle(0, 0, 250, 50), player_bounds);
            ships[1] = new Ship(new Vector2(k + position.X, 50 + position.Y), texture, new Rectangle(0, 50, 200, 50), player_bounds);
            ships[2] = new Ship(new Vector2(k + position.X, 100 + position.Y), texture, new Rectangle(0, 100, 150, 50),player_bounds);
            ships[3] = new Ship(new Vector2(k + position.X, 150 + position.Y), texture, new Rectangle(0, 100, 150, 50),player_bounds);
            ships[4] = new Ship(new Vector2(k + position.X, 200 + position.Y), texture, new Rectangle(0, 150, 100, 50),player_bounds);

        }


        public bool Update(MouseState mouse) //BOOL
        {
            mouse = Mouse.GetState();
            mouse_point = new Point(mouse.X, mouse.Y);
            mouse = Mouse.GetState();

            if (mode)
            {
                if (mouse.LeftButton == ButtonState.Pressed && old_mouse.LeftButton == ButtonState.Released)
                {
                    for (int i = 0; i < tiles.GetLength(0); i++)
                    {
                        for (int j = 0; j < tiles.GetLength(1); j++)
                        {
                            //ship_hit = false;
                                for (int k = 0; k < 5; k++)
                                {
                                  if (mouse.LeftButton == ButtonState.Pressed && old_mouse.LeftButton == ButtonState.Released)
                                  {
                                    {
                                        if (ships[k].ship_hitbox.Contains(mouse_point))
                                        {
                                            counter++; 
                                            
                                            mode = false; 
                                            return ships[k].fired(); // return
                                            //break;
                                        }

                                    }
                                  }

                                  if (tiles[i, j].hitbox.Contains(mouse_point))
                                  {
                                      mode = false; 
                                      tiles[i, j].fired(ship_hit);
                                      return true;
                                      //break;
                                  }

                            }
                        }

                    }
                }

            }
            foreach (Ship ship in ships)
            {
                ship.is_visible = false; 
                ship.Update();
            }
            if(counter == 17)
            {
                winning = true; 
            }
            old_mouse = mouse;

            return false;
           
        }
        

        public void Draw(SpriteBatch spriteBatch)
        {

            int i;
            int size = 10;
            int j;



            for (i = 0; i < size; i++)
            {
                for (j = 0; j < size; j++)
                {
                    tiles[i, j].Draw(spriteBatch);


                }

            }

            foreach (Ship ship in ships)
            {
                
                {
                    ship.Draw(spriteBatch);
                }
            }

                
        }


 
    }
        }
    

