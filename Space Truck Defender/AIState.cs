﻿using System;
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
        private Target MoveTarget = null,
                        ShootTarget = null;
        private bool Moving, 
                    Shooting;
        //a list of events that could change the AI state
        private List<AITrigger> TriggerList;

        /* Use this constructor for instantiating "data"
         * versions of the AI. These objects will not be
         * added to the board.
         */
        public AIState(bool moving, bool shooting)
        {
            Moving = moving;
            Shooting = shooting;
            TriggerList = new List<AITrigger>();
        }



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
            {
                Body.Go();
                if (MoveTarget != null)
                {
                    if (!MoveTarget.IsDestroyed())
                        Body.SetHeading(PointAt(MoveTarget));
                    else
                        MoveTarget = null;
                }
            }
            else
                Body.Stop();

            if (Shooting)
            {
                Body.Shoot();
                if (ShootTarget != null)
                {
                    if (!ShootTarget.IsDestroyed())
                        Body.SetWeaponHeadings(PointAt(ShootTarget));
                    else
                        ShootTarget = null;
                }
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
            double dir = Math.Atan2(tp.Y - bp.Y, tp.X - bp.X);
            return dir;
        }

        public void SetMoveTarget(Target t)
        {
            if (MoveTarget == null)
                MoveTarget = t;
        }

        public void SetShootTarget(Target t)
        {
            if (ShootTarget == null)
                ShootTarget = t;
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
            AIState retstate = new AIState(this.Moving, this.Shooting);
            retstate.Body = this.Body; // this is fine, as Copy() is only used for board copies.
                                        // the data copy is guaranteed to have a body.
            retstate.MoveTarget = this.MoveTarget;
            retstate.ShootTarget = this.ShootTarget;
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
