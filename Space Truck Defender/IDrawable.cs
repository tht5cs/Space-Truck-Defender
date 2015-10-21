using System;
namespace Space_Truck_Defender
{
    interface IDrawable
    {
        void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb);
        void LoadSprite(Microsoft.Xna.Framework.Content.ContentManager content, string spriteName);
        void LoadSprite(Microsoft.Xna.Framework.Graphics.Texture2D sprite);
    }
}
