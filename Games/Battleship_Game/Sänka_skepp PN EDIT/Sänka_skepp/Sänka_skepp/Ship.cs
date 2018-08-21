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
    class Ship
    {

        public Vector2 position;
        private Vector2 origin;
        
        Texture2D texture;

        public Rectangle spriteSourceRect;
        public Rectangle ship_hitbox;
        private Rectangle game_board;

        //Controls lists with explosions/hit and if positions are hit or not
        List<Rectangle> hit_boxes;
        List<Rectangle> fired_hit_boxes = new List<Rectangle>();
        
        public float rotation; 

        public bool picked_up;
        public bool is_fired_at = false;
        public bool is_visible = false; 

        Point mouse_point;
        MouseState old_mouse;

        public int hp; 

        public Ship()
        {

        }

        public Ship(Vector2 position, Texture2D texture, Rectangle shipRec, Rectangle game_board)
        {
            this.position = new Vector2(position.X, position.Y);
            this.texture = texture;
            this.game_board = game_board;

            spriteSourceRect = shipRec;

            ship_hitbox = new Rectangle((int)position.X, (int)position.Y, shipRec.Width, shipRec.Height);
            hit_boxes = new List<Rectangle>();
    
            //Create the hitboxes for the explosion on the ships (IF ship is hit)
            for(int i = 0; i < spriteSourceRect.Width / 50; i++)
            {
                hit_boxes.Add(new Rectangle((int)(position.X + i * 50), (int)position.Y,50, 50));

            }
            this.hp = ship_hitbox.Width / ship_hitbox.Height; 

        }

        public void Update()
        {

            MouseState mouse = Mouse.GetState();
            mouse_point = new Point(mouse.X, mouse.Y);

            if (Game1.current_state == Game1.GameState.placing_ships1 || Game1.current_state == Game1.GameState.placing_ships2)
            {
                if (ship_hitbox.Contains(mouse_point) && (mouse.LeftButton == ButtonState.Pressed && old_mouse.LeftButton == ButtonState.Released))
                {
                    //Pick up / grab the ship
                    picked_up = true;


                }

                else if (mouse.LeftButton == ButtonState.Pressed && old_mouse.LeftButton == ButtonState.Released && picked_up)
                {
                    //If left button is clicked, picked up is true AND ship is within the grid(s), drop / place the ship    
                    picked_up = false;

                    //Set ships position X and Y to mouse position X and Y and snap them to a grid 50 x 50 (within a tile)
                    position.X = (mouse.X / 50) * 50;
                    position.Y = (mouse.Y / 50) * 50;

                    //Update all hitboxes and move them to mouse positionX and Y
                    ship_hitbox = new Rectangle((int)position.X, (int)position.Y, ship_hitbox.Width, ship_hitbox.Height);

                    if (game_board.X > ship_hitbox.X || game_board.X + game_board.Width < ship_hitbox.X + ship_hitbox.Width)
                        picked_up = true;
                    else if (game_board.Y > ship_hitbox.Y || game_board.Y + game_board.Height < ship_hitbox.Y + ship_hitbox.Height)
                        picked_up = true;
                }

                if (rotation == 0)
                {
                    for (int i = 0; i < hit_boxes.Count; i++)
                    {
                        hit_boxes[i] = new Rectangle((int)(position.X + i * 50), (int)position.Y, 50, 50);
                    }
                }
                else
                {
                    for (int i = 0; i < hit_boxes.Count; i++)
                    {
                        hit_boxes[i] = new Rectangle((int)(position.X), (int)position.Y + i * 50, 50, 50);

                    }
                }
                if (ship_hitbox.Contains(mouse_point) && (mouse.RightButton == ButtonState.Pressed && old_mouse.RightButton == ButtonState.Released))
                {

                    Rotate();

                }

                if (picked_up == true)
                {

                    if (mouse.RightButton == ButtonState.Pressed && old_mouse.RightButton == ButtonState.Released && picked_up)
                    {
                        Rotate();

                    }

                    position.X = (mouse.X / 50) * 50;
                    position.Y = (mouse.Y / 50) * 50;

                   
                }
                
            }
            old_mouse = mouse;
        }

      public bool fired() //BOOL
        {    
            //Call this method in Matrixes if ship is fired at. Sets bool to is_fired_at to true; 
      
            foreach(Rectangle r in hit_boxes)
            {
               
               if(r.Contains(mouse_point))
               {
                   hp--;
                   is_fired_at = true;
                   fired_hit_boxes.Add(r);
      
                   if (hp == 0)
                       is_visible = true;

                    return true;
               }
              
            }
            return false;
        }


        public void Rotate()
        {   
            //If rotate is 0 degrees, rotate the ship +90 degrees
            if (rotation == 0)
            {
                rotation = MathHelper.ToRadians(90);
                int priv_height = ship_hitbox.Height;
                int priv_width = ship_hitbox.Width;

                //Set the height as width and vice-versa
                ship_hitbox.Width = priv_height;
                ship_hitbox.Height = priv_width;

                ship_hitbox = new Rectangle(mouse_point.X, mouse_point.Y, ship_hitbox.Width, ship_hitbox.Height);
                origin.Y = 50;

                for (int i = 0; i < hit_boxes.Count; i++)
                {
                    hit_boxes[i] = new Rectangle((int)(position.X), (int)position.Y + i * 50, 50, 50);
                }
            }

        
            //if the ship is 90 degrees or else, rotate the ship 0 degrees (back)
                else
            {
                rotation = MathHelper.ToRadians(0);
                int priv_height = ship_hitbox.Height;
                int priv_width = ship_hitbox.Width;

                //Set the height as width and vice-versa
                ship_hitbox.Width = priv_height;
                ship_hitbox.Height = priv_width;

                ship_hitbox = new Rectangle(mouse_point.X, mouse_point.Y, ship_hitbox.Width, ship_hitbox.Height);
                origin.Y = 0;

                for (int i = 0; i < hit_boxes.Count; i++)
                {
                    hit_boxes[i] = new Rectangle((int)(position.X + i * 50), (int)position.Y, 50, 50);
                }
         
                
            }

        }
      
        
        public void Draw(SpriteBatch spriteBatch)
        {
            if (Game1.current_state == Game1.GameState.placing_ships1 || Game1.current_state == Game1.GameState.placing_ships2 || is_visible == true)
            {
                spriteBatch.Draw(texture, position, spriteSourceRect, Color.White, rotation, origin, 1.0f, SpriteEffects.None, 1.0f);
               
            }

            foreach(Rectangle r in fired_hit_boxes)
            {
                spriteBatch.Draw(texture, r, new Rectangle(150, 100, 50, 50), Color.White);
            }

           
        }
    }


}
