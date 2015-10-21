using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Space_Truck_Defender
{
    public class AIState:Piece
    {
        private Actor Body;
        private Target MoveTarget,
                        ShootTarget;
        private bool Moving, Shooting;
        //a list of events that could change the AI state
        private List<AITrigger> TriggerList;

        public AIState()
        {
            TriggerList = new List<AITrigger>();
        }

        /* Use this constructor for instantiating "data"
         * versions of the AI. These objects will not be
         * added to the board.
         */

        /* Use this contructor for adding board copies of the
         * AI state. 
         */


        public void Attach(Actor a)
        {
            Body = a;
        }

        //Note that AISTATES are never directly part of lists,
        //and exist as fields of an AI. Thus, this will only
        // ever be called by its AI parent.
        public override void Update(GameTime gt)
        {
            if (Moving)
                Body.Go();
                if (MoveTarget != null)
                {
                    Body.SetHeading(PointAt(MoveTarget));
                }
            else
                Body.Stop();

            if (Shooting)
                Body.Shoot();
                if (ShootTarget != null)
                {
                    Body.SetWeaponHeadings(PointAt(ShootTarget));
                }
            else
                Body.HoldFire();



        }

        /* This method returns the apropriate heading for the 
         * actor to "point" at the given target.
         */
        private double PointAt(Target t)
        {
            var tp = t.GetPosition();
            var bp = Body.GetPosition();
            double dir = Math.Atan2(tp.X - bp.X, tp.Y - bp.Y);
            return dir;
        }

        public List<AITrigger> GetTriggers()
        {
            return this.TriggerList;
        }

        public void AddTrigger(AITrigger t)
        {
            TriggerList.Add(t);
        }


        /* Used for generating board copies of AIStates
         */
        public AIState Copy()
        {
            var retstate = new AIState();
            retstate.Body = this.Body; // this is fine, as Copy() is only used for board copies.
                                        // the data copy is guaranteed to have a body.
            retstate.MoveTarget = this.MoveTarget;
            retstate.ShootTarget = this.ShootTarget;
            retstate.Shooting = this.Shooting;
            retstate.Moving = this.Moving;
            retstate.TriggerList = this.TriggerList; 
            /* pass by reference ok for this as the AI will have 
             * to make its own deep copies of the AITriggers to add
             * to the board. This TriggerList is just so the AI can copy
             * the correct triggers.
             */

            return retstate;
        }

    }
}
