using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Space_Truck_Defender
{
    /* For the time being, this class holds pre-initialized base data for testing use.
     * However, this class will eventually be used to read in data from file so that
     * the actor manager can use it. Lookup, etc. will be done here. The actor manager
     * will:
     * 1. Send files to be read in and initialized
     * 2. Query the Manager for data
     */
    public class ManagerData
    {
        //Defaults

        public Engine baseEngine1,
                    baseEngine2;
        public Actor baseFriendlyActor,
                    baseEnemyActor;
        public Actor baseProjectile1;
        public Weapon baseWeapon1;

        public Hull ProjectileHull,
                    ActorHull,
                    PlayerHull;

        public EffectDestroy baseDestroy;
        public EffectDamage baseDamage;

        public AI baseAI;

        //endDefaults

        public GraphicsDevice Device;


        public ManagerData(GraphicsDevice _device)
        {
            this.Device = _device;
            InitializeBases();
        }


        private void InitializeBases()
        {
            ActorHull = new Hull(75, 5, 2);
            PlayerHull = new Hull(100, 5, 2);
            ProjectileHull = new Hull(1, -1, 0);

            baseDestroy = new EffectDestroy();
            baseDamage = new EffectDamage(20);

            baseEngine1 = new Engine(5, 50);
            baseEngine2 = new Engine(10, 100);
            Vector2 v = new Vector2(0, 0);
            baseFriendlyActor = new Actor(v, 10, baseEngine1, PlayerHull, Device);
            //baseFriendlyActor.AddEffect(baseDestroy);
            baseFriendlyActor.SetCollision(0);
            baseEnemyActor = new Actor(v, 10, baseEngine1, ActorHull, Device);
            baseEnemyActor.SetCollision(2);
            //baseEnemyActor.AddEffect(baseDestroy);
            baseProjectile1 = new Actor(v, 5, baseEngine2, ProjectileHull, Device);
            baseProjectile1.AddEffect(baseDamage);
            baseProjectile1.SetCollision(1);
            //baseProjectile1.AddEffect(baseDestroy);
            List<double> d = new List<double>();
            for (int i = 0; i < 3; i++)
                d.Add(0);
            baseWeapon1 = new Weapon(baseProjectile1, d, 0.075f, 0.12);

            InitializeBaseAI();
        }

        /* AI that sets the actor to move and shoot
         */
        public void InitializeBaseAI()
        {
            AIState a = new AIState(true, false);
            baseAI = new AI(a);

        }

    }
}
