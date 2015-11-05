using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework;
namespace Space_Truck_Defender
{
    public class TargetActor:Target
    {
        private Actor body;

        public TargetActor(Actor a)
        {
            body = a;
        }

        public override Vector2 GetPosition()
        {
            if (body.IsDestroyed())
                this.Destroy();
            return body.GetPosition();
        }

    }
}
