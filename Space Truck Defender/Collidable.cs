using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Space_Truck_Defender
{
    public class Collidable : Piece
    {
        protected Hitbox Box;
        protected Vector2 Position;
        protected int CollisionSetting;
        protected Dictionary<int, Effect> CollisionEffects;


        public Vector2 GetPosition()
        {
            return this.Position;
        }

        public void SetPosition(Vector2 v)
        {
            this.Position = v;
        }

        public int GetHitRadius()
        {
            return Box.GetHitRadius();
        }

        public void DrawHitbox(SpriteBatch sb)
        {
            this.Box.Draw(sb);
        }

        public int GetCollisionSetting()
        {
            return this.CollisionSetting;
        }

        public static void ResolveCollision(Collidable a, Collidable b)
        {
            a.Collide(b);
            b.Collide(a);
        }

        public void AddEffect(Effect e)
        {
            this.CollisionEffects[e.GetID()] = e;
        }

        /* When THIS collides with collidable c, only resolve
         * collision effects for THIS. c will call Collide(THIS)
         * so the collsion effects will be handled on that side.
         */
        public virtual void Collide(Collidable c)
        {

        }

        public Dictionary<int, Effect> GetCollisionEffects()
        {
            return this.CollisionEffects;
        }
    }
}
