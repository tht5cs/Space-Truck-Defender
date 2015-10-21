using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Space_Truck_Defender
{
    //these are things that attach to actors. They
    // can be instant effects or long term.
    public class Effect
    {
        // how long the effect lasts. 0 means instantaneous.
        protected float Duration;
        protected float CurrTime;

        
        protected String EffectName = "";
        protected int ID;


        //DON'T EVER ACTUALLY USE THIS
        public Effect()
        {
            Duration = 0;
            CurrTime = 0;
        }

        protected void SetID()
        {
            ID = EffectName.GetHashCode();
        }

        //what happens when the effect is added
        public virtual void OnAttach(Actor a)
        {

        }

        //ongoing status effects of the effect
        public void UpdateEffect(Actor a, GameTime gt)
        {
            CurrTime -= (float)gt.ElapsedGameTime.TotalSeconds;
            OnTick(a, gt);
        }

        //what happens during the tick
        protected virtual void OnTick(Actor a, GameTime gt)
        {

        }

        public bool HasTime()
        {
            if (this.CurrTime > 0)
                return true;
            return false;
        }

        //what happens when the effect is removed
        // (use this for removing status effects)
        public virtual void OnEnd(Actor a)
        {

        }

        /* Duplicate lasting effects don't stack, so this method
         * allows O(1) time checking if an actor already has said effect
         */
        public int GetID()
        {
            return this.ID;
        }

        public float GetDuration()
        {
            return this.Duration;
        }

        public float GetCurrTime()
        {
            return this.CurrTime;
        }
        public virtual Effect Copy()
        {
            return this;
        }

        /* Super important.
         * When an actor is trying to gain a new effect,
         * if it is already affected by an effect of the same type
         * the duplication must be resolved. This behavior is different
         * for each effect (i.e. does rebuffing extend the buff?)
         * so it has to be handled here
         * 
         * Note that THIS is the effect that might be added and e 
         * is the effect already in place
         */
        public virtual Effect ResolveDuplicate(Effect e)
        {
            return e;
        }
    }
}
