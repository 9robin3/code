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
    class Tile
    {

        public Vector2 position;
        public Texture2D texture;
        
        public Rectangle rect;
        public Rectangle hitbox;
        
        public bool is_fired_at;
        public bool ship_hit;
  
        Point mouse_point;
        MouseState mouse;

        public int ammo = 3; 
        public Tile()
        {

        }
  

        public Tile(Vector2 position, Texture2D texture)
        {
            this.position = position;
            this.texture = texture;
            rect = new Rectangle(200, 200, 50, 50);
            hitbox = new Rectangle((int)position.X, (int)position.Y, 50, 50);
            this.is_fired_at = false;
            ship_hit = false;

            mouse_point = new Point(mouse.X, mouse.Y);
   
        }

        public void fired(bool ship_hit)
        {   //Calls this method in Matrixes if tile is fired at
            ship_hit = true;
            is_fired_at = true;
     
        }
   


        public void Update()
        {
  
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
            spriteBatch.Draw(texture, position, rect, Color.Blue);

           

            if(is_fired_at == true && !ship_hit)
            {   //Draws miss if tile is fired at
                spriteBatch.Draw(texture, hitbox, new Rectangle(200,100,50,50) , Color.White); 
            }

      
        }

    }
}
