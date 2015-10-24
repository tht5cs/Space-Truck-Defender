using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Space_Truck_Defender
{
    public class Hitbox:IDrawable
    {
        private int HitRadius;
        private Vector2 Position;
        private Texture2D Sprite;


        public Hitbox(int _HitRadius, int _colsetting, Vector2 _Position, GraphicsDevice graphicsDevice)
        {
            this.HitRadius = _HitRadius;
            this.Position = _Position;
            this.Sprite = CreateCircle(_HitRadius, _colsetting, graphicsDevice);
        }

        //copy constructor with new location
        public Hitbox(Hitbox h, Vector2 _position)
        {
            this.HitRadius = h.GetHitRadius();
            this.Position = _position;
            this.Sprite = h.Sprite;
        }

        public int GetHitRadius()
        {
            return this.HitRadius;
        }

        //changes the position of the hitbox
        public void SetPosition(Vector2 _Position)
        {
            this.Position = _Position;
        }

        public void Draw(SpriteBatch sb)
        {
            int width = Sprite.Width;
            int height = Sprite.Height;

            //need to adjust draw location by sprite size
            int xpos = (int)Position.X - width / 2;
            int ypos = (int)Position.Y - height / 2;

            Rectangle sourceRectangle = new Rectangle(0, 0, width, height);
            Rectangle destinationRectangle = new Rectangle(xpos, ypos, width, height);
            sb.Draw(Sprite, destinationRectangle, sourceRectangle, Color.White);
        }

        public Color ChooseColor(int colSetting)
        {
            Color c = Color.White;
            switch(colSetting)
            {
               //friendly actor
                case 0:
                    c = Color.White;
                    break;
                // friendly projectile
                case 1:
                    c = Color.Green;
                    break;
                case 2:
                    c = Color.Yellow;
                    break;
                case 3:
                    c = Color.Red;
                    break;
            }
            return c;

        }

        public Texture2D CreateCircle(int radius, int colsetting, GraphicsDevice graphicsDevice)
        {
            int outerRadius = radius * 2 + 2; // So circle doesn't go out of bounds
            Texture2D texture = new Texture2D(graphicsDevice, outerRadius, outerRadius);
            Color cColor = ChooseColor(colsetting);


            Color[] data = new Color[outerRadius * outerRadius];

            // Colour the entire texture transparent first.
            for (int i = 0; i < data.Length; i++)
                data[i] = Color.Transparent;

            // Work out the minimum step necessary using trigonometry + sine approximation.
            double angleStep = 1f / radius;

            for (double angle = 0; angle < Math.PI * 2; angle += angleStep)
            {
                // Use the parametric definition of a circle: http://en.wikipedia.org/wiki/Circle#Cartesian_coordinates
                int x = (int)Math.Round(radius + radius * Math.Cos(angle));
                int y = (int)Math.Round(radius + radius * Math.Sin(angle));

                data[y * outerRadius + x + 1] = cColor * 0.5f;
            }

            texture.SetData(data);
            return texture;
        }


        public void LoadSprite(ContentManager content, string spriteName)
        {
            //
        }

        public void LoadSprite(Texture2D sprite)
        {
            //
        }
    }
}
