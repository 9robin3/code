using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tower_defence;

namespace Tower_defence
{
    public class Particle
    {
        public Texture2D texture { get; set; }        // The texture that will be drawn to represent the particle
        public Vector2 pos { get; set; }        // The current position of the particle        
        public Vector2 speed { get; set; }        // The speed of the particle at the current instance
        public float angle { get; set; }            // The current angle of rotation of the particle
        public float angular_speed { get; set; }    // The speed that the angle is changing
        public Color color { get; set; }            // The color of the particle
        public float size { get; set; }                // The size of the particle
        public int life_length { get; set; }                // The 'time to live' of the particle

        public Particle(Texture2D texture, Vector2 pos, Vector2 speed,
            float angle, float angular_speed, Color color, float size, int life_length)
        {
            this.texture = texture;
            this.pos = pos;
            this.speed = speed;
            this.angle = angle;
            this.angular_speed = angular_speed;
            this.color = color;
            this.size = size;
            this.life_length = life_length;
        }
        public void Update()
        {
            life_length--;
            pos += speed;
            angle += angular_speed;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle src_rect = new Rectangle(0, 0, texture.Width, texture.Height);
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);

            spriteBatch.Draw(texture, pos, src_rect, color,
                angle, origin, size, SpriteEffects.None, 0f);
        }



    }
}

