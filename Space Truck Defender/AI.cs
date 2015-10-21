using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Space_Truck_Defender
{
    /* The AI units of the actors. Technically, the actor
     * belongs to the AI
     * 
     * So what i'm going for is essentially a finite state machine.
     * the details of the state will be held in the AI components,
     * while the details of switching states will be held here.
     */
    public class AI: Piece
    {
        private Actor Body; // this is what the AI controls
        private AIState CurrentState;

        public AI(Actor _body)
        {
            this.Body = _body;
        }

        //point the AI unit at a given actor
        public void Attach (Actor a)
        {
            this.Body = a;
        }

        public override void Update(GameTime gt)
        {
            
            if (!Body.IsDestroyed())
            {

                //CurrentState.Update(gt);
            }
            else
                this.Destroy();
        }

        public bool CheckTriggers()
        {
            foreach (var t in CurrentState.GetTriggers())
            {
                if (t.IsTriggered())
                    return true;
            }
            return false;
        }


        /* Basic instantiation Algorithm:
         * If Updating reveals a trigger:
         *      1. Pick first trigger activated. if multiple go off, pick the first
         *      2. Remove the current state
         *      3. Copy the AISTATE behind the active trigger, set it to current state, and add it to the board
         *      4. Clear Active triggers
         *      5. Copy triggers and add them to the board
         */

    }
}
