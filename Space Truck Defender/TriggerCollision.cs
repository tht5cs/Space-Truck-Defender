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
        private bool LessThan; //if this is true, trigger will flip if count is less than trigger count. Otherwise, greater than.

        public TriggerCollision(AIState _state, CollisionCounter c, int count, bool lt) : base(_state)
        {
            this.cCounter = c;
            this.triggerCount = count;
            this.LessThan = lt;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gt)
        {
 	         cCounter.SetPosition(body.GetPosition());
             if (!LessThan)
             {
                 if (cCounter.GetCount() > triggerCount)
                     this.Trigger();
             }
             else
             {
                 if (cCounter.GetCount() < triggerCount)
                     this.Trigger();
             }
             cCounter.ResetCount();
             if (this.IsDestroyed())
                 cCounter.Destroy();

        }

        protected override void Trigger()
        {
            base.Trigger();
            cCounter.Destroy();
        }

        public override void Attach(Actor a)
        {
            this.body = a;
            cCounter.SetPosition(a.GetPosition());
        }

        public override AITrigger Copy()
        {
            return new TriggerCollision(State, new CollisionCounter(cCounter, cCounter.GetPosition()), this.triggerCount, this.LessThan);
        }

        public CollisionCounter GetCollisionCounter()
        {
            return this.cCounter;
        }

    }
}
