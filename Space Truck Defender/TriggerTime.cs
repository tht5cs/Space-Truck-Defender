using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Space_Truck_Defender
{
    //this class triggers a change in AI state when its timer reaches 0
    public class TriggerTime : AITrigger
    {
        /* here, we don't differentiate max/curr time because 
         * this trigger can only count down.
         * 
         * Also, as always, time is in seconds.
         */
        private float time;

        public TriggerTime(AIState _state, float _time):base(_state)
        {
            this.time = _time;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gt)
        {
            time -= (float)gt.ElapsedGameTime.TotalSeconds;
            if (time <= 0)
                this.Trigger();
        }


        /* remember, this copies data copies, so the original time 
         * value will always be left untouched.
         */
        public override AITrigger Copy()
        {
            return new TriggerTime(State, time);
        }
    }
}
