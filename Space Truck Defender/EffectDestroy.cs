using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Space_Truck_Defender
{
    public class EffectDestroy : Effect
    {

        /* Destruction effects
         * 
         */

        //NAME  = "DESTROY";

        public EffectDestroy() : base()
        {
            this.EffectName = "DESTROY";
            this.SetID();
        }
        public EffectDestroy(float  _duration) : this()
        {
            this.Duration = _duration;
        }

        public override void OnAttach(Actor a)
        {
            
        }

        protected override void OnTick(Actor a, Microsoft.Xna.Framework.GameTime gt)
        {
            
        }

        public override void OnEnd(Actor a)
        {
            a.Destroy();
        }

        public override Effect Copy()
        {
            return new EffectDestroy();
        }

        public override Effect ResolveDuplicate(Effect e)
        {
            return this;
        }
    }
}
