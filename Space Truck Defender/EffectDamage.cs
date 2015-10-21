using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Space_Truck_Defender
{
    public class EffectDamage : Effect
    {
        private float damage;

        public EffectDamage(float _damage) : base()
        {
            EffectName = "DAMAGE";
            SetID();
            damage = _damage;
        }

        public override void OnEnd(Actor a)
        {
            a.Damage(damage);
        }

        public override Effect ResolveDuplicate(Effect e)
        {
            EffectDamage temp = (EffectDamage)e;
            return new EffectDamage(this.damage + temp.damage);
        }
    }
}
