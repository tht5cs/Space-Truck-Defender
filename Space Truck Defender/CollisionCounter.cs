using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;


namespace Space_Truck_Defender
{
    /* This class is a collidable that sits on the board
     * and gives a readout of the current number of 
     * other collidables it is colliding with. Collidables.
     * 
     * Trigger logic will be handled by the appropriate trigger, 
     * not here. all this does is count.
     */
    public class CollisionCounter:Collidable
    {
        //THE PURPOSE OF THIS CLASS
        private int count = 0;
        private Actor lastActor = null;
        //Data constructor
        public CollisionCounter(int radius, int collisionSetting, Vector2 pos, GraphicsDevice device)
        {
            this.Box = new Hitbox(radius, collisionSetting, pos, device);
            this.SetPosition(pos);
            CollisionEffects = new Dictionary<int, Effect>();
        }

        //board constructor
        public CollisionCounter(CollisionCounter c, Vector2 pos)
        {
            this.Box = new Hitbox(c.Box, pos);
            this.SetPosition(pos);
            CollisionEffects = new Dictionary<int, Effect>();
        }


        public override void Update(GameTime gt)
        {
            Box.SetPosition(this.GetPosition());
            this.count = 0;
        }

        public int GetCount()
        {
            return this.count;
        }

        public override void Collide(Collidable c)
        {
            this.count++;
            if (c is Actor)
                lastActor = (Actor)c;
        }


        public Actor GetLastActor()
        {
            return this.lastActor;
        }

    }
}
