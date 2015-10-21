using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Space_Truck_Defender
{
    /* This is an engine part
     * This class controls how an actor moves through space,
     * for example in a straight line, spiral, etc.
     * Essentially, this determines movement offset that happens
     * irrespective of heading.
     */

    //this base class represents the empty movement pattern.
    //it returns no offset
    public class MovementPattern
    {

        public MovementPattern()
        {

        }

        public virtual void Update(GameTime gt)
        {

        }

        public virtual Vector2 GetOffset(GameTime gt)
        {
            return new Vector2(0, 0);
        }
    }
}
