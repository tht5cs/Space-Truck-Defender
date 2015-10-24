using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Space_Truck_Defender
{
    public class TriggerCollision : AITrigger
    {
        private CollisionCounter cCounter;
        private Actor body; // this is the thing we're tracking
        private int triggerCount; // the count that needs to be reached before triggering

        public TriggerCollision(AIState _state, CollisionCounter c, int count) : base(_state)
        {
            this.cCounter = c;
            this.triggerCount = count;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gt)
        {
 	         cCounter.SetPosition(body.GetPosition());
            if(cCounter.GetCount() > triggerCount)
                this.Trigger();
        }

        public void Attach(Actor a)
        {
            this.body = a;
            cCounter.SetPosition(a.GetPosition());
        }

        public override AITrigger Copy()
        {
            return new TriggerCollision(State, new CollisionCounter(cCounter, cCounter.GetPosition()), this.triggerCount);
        }

    }
}
