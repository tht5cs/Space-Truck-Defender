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

        /* These point to the triggers on the board
         * so that they can be destroyed easily.
         */
        private AIState CurrentState;
        private List<AITrigger> ActiveTriggers;

        /* Data constructor
         */
        public AI(AIState ai)
        {
            this.CurrentState = ai;
            ActiveTriggers = ai.GetTriggers();
        }

        /* Board constructor. This Constructor
         * is used once per actor. One actor only ever
         * has one AI, though they may go through multiple states
         * and triggers.
         */
        public AI(AI ai, Actor _body)
        {//IMP
            CurrentState = ai.CurrentState.Copy();
            Attach(_body);
            ActiveTriggers = new List<AITrigger>();
            ResetActiveTriggers();
        }


        //point the AI unit at a given actor
        public void Attach (Actor a)
        {
            this.Body = a;
            this.CurrentState.Attach(a);
        }

        public override void Update(GameTime gt)
        {//IMP
            
            if (!Body.IsDestroyed())
            {

                //CurrentState.Update(gt);
            }
            else
                this.Destroy();
        }

        /* This method runs before updating the AI
         * in the list. 
         * 1: check if triggers are flipped
         * 2: if so, update current state = new copy of trigger.state
         * 3: Destroy any triggers already in place
         * 4: then, update ActiveTriggers (by making new copies of the ones in the currstate)
         * (The Actor Manager will handle adding the triggers to list)
         */
        public bool CheckTriggers()
        {
            bool ret = false;
            foreach (var t in ActiveTriggers)
            {
                if (t.IsTriggered())
                {
                    ret = true;
                    CurrentState = t.GetState().Copy();
                    ResetActiveTriggers();

                }
            }
            return ret;
        }

        /* This method first destroys the current triggers, then
         * takes the triggers behind the current state
         * and sets them as the new active triggers.
         */
        private void ResetActiveTriggers()
        {
            // clean out the old list, and destroy
            //the triggers so other lists will do the same
            int max = ActiveTriggers.Count;
            for (int i = max; i >= 0; i--)
            {
                ActiveTriggers[i].Destroy();
                ActiveTriggers.RemoveAt(i);
            }

            //then, we add copies of the new triggers to the list
            foreach(var t in CurrentState.GetTriggers())
            {
                ActiveTriggers.Add(t.Copy());
            }

        }

        /* ActorManager calls this so it knows what new triggers
         * to add to the board when an AI switches states or is
         * instantiated. Note this passes by reference, as the
         * AI has already made deep copies for use.
         */
        public List<AITrigger> GetActiveTriggers()
        {
            return this.ActiveTriggers;
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
