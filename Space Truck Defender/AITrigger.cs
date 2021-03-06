﻿using System;
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
        protected bool Triggered = false;
        protected AIState State;
        protected Actor Body;

        protected AITrigger(AIState _state)
        {
            this.State = _state;
        }

        protected virtual void Trigger()
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

        public virtual void Attach(Actor a)
        {
            this.Body = a;
        }

        //
        public virtual Target GetMoveTarget()
        {
            return null;
        }
        public virtual Target GetShootTarget()
        {
            return null;
        }

        /* This is similar to a method in Effect. It returns
         * a new, deep copy of the AITrigger. For use in making
         * copies for the board.
         */
        public abstract AITrigger Copy();
    }
}
