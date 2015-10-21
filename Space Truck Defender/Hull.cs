using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Space_Truck_Defender
{
    /* This class is the "defensive" stats of 
     * an actor. it details how it responds to 
     * damage and stuff.
     * 
     * Also, this class doesn't manage any real time stuff. 
     * it is purely event based.
     */
    public class Hull
    {
        private float MaxHP,
                        CurrHP; // when this is 0, ship is dead
        private float Armor; // reduces damage caused by instant hits (not DOTS)
        private int Type; // collision resolution effects

        /* TYPE LIST:
         * 0: Projectile
         *      self destructs on any actor
         * 1: Missile
         *      self destructs on TYPE 2 actors
         * 2: Solid
         *      Resolves destruction by HP comparison
         */ 

        /* we need to be able to trigger conditional instant death.
         * for example, a missile should automatically detonate when contacting
         * an actor, but not a projectile.
         */

        public Hull(float _maxHP, float _armor, int _type)
        {
            this.MaxHP = _maxHP;
            this.CurrHP = MaxHP;
            this.Armor = _armor;
            this.Type = _type;
        }

        public Hull Copy()
        {
            return new Hull(MaxHP, Armor, Type);
        }


        public float GetMaxHP()
        {
            return MaxHP;
        }

        public float GetCurrHP()
        {
            return CurrHP;
        }

        public float GetArmor()
        {
            return Armor;
        }

        public int GetHullType()
        {
            return Type;
        }


        /* basic damage function. For each hit,
         * reduces health by (Damage-Armor). Negative
         * armor values increase damage.
         */

        public void Damage(float d)
        {
            CurrHP = CurrHP - (d - Armor);
        }

        public void DamagePierce(float d)
        {
            CurrHP = CurrHP - d;
        }

        public void ResetHP()
        {
            CurrHP = MaxHP;
        }

    }
}
