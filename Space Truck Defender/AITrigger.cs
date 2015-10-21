using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;


namespace Space_Truck_Defender
{
    /* AI triggers contain the conditions necessary
     * to move an actor to another state.
     */
    public abstract class AITrigger : Piece
    {
        private bool Triggered = false;
        private AIState State;

        private void Trigger()
        {
            Triggered = true;
        }

        public bool IsTriggered()
        {
            return Triggered;
        }

        public AIState GetState()
        {
            return this.State;
        }
    }
}
