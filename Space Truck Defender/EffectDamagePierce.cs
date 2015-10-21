using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Space_Truck_Defender
{
    public class EffectDamagePierce : Effect
    {
        private float damage;

        public EffectDamagePierce(float _damage)
            : base()
        {
            EffectName = "DAMAGE-PIERCE";
            SetID();
            damage = _damage;
        }

        public override void OnEnd(Actor a)
        {
            a.DamagePierce(damage);
        }

        public override Effect ResolveDuplicate(Effect e)
        {
            EffectDamagePierce temp = (EffectDamagePierce)e;
            return new EffectDamagePierce(this.damage + temp.damage);
        }
    }
}
