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
                    baseEngine2,
                    baseEngine3;
        public Actor baseFriendlyActor,
                    baseEnemyActor;
        public Actor baseProjectile1;
        public Weapon baseWeapon1;

        public Hull ProjectileHull,
                    ActorHull,
                    PlayerHull;

        public EffectDestroy baseDestroy;
        public EffectDamage baseDamage;

        public AI AIStartStop;
        public AI AICloseStop;

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
            baseEngine3 = new Engine(2, 20);
            Vector2 v = new Vector2(0, 0);
            baseFriendlyActor = new Actor(v, 10, baseEngine1, PlayerHull, Device);
            //baseFriendlyActor.AddEffect(baseDestroy);
            baseFriendlyActor.SetCollision(0);
            baseEnemyActor = new Actor(v, 10, baseEngine3, ActorHull, Device);
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

            InitializeAIStartStop();
            InitializeAICloseStop();
        }

        /* AI that sets the actor to move and shoot
         */
        public void InitializeAIStartStop()
        {
            //first, the two basic states: move or stop
            AIState move = new AIState(true, false);
            AIState stop = new AIState(false, false);
            //now, we connect them with the appropriate triggers
            var t1 = new TriggerTime(stop, 0.25f);
            var t2 = new TriggerTime(move, 0.25f);
            move.AddTrigger(t1);
            stop.AddTrigger(t2);
            AIStartStop = new AI(move);

        }

        /* AI For enemy actors. When a friendly (player)
         * actor comes close, the actor will stop moving.
         */
        public void InitializeAICloseStop()
        {
            AIState move = new AIState(true, false);
            AIState stop = new AIState(false, false);
            CollisionCounter c = new CollisionCounter(30, 3, Device);//radius of 15, collides with Friendly Actors
            var t1 = new TriggerCollision(stop, c, 0, false); // trips when more than 1 friendly actor in radius
            var t2 = new TriggerCollision(move, c, 1, true); // trips when there are no friendly actors in radius
            move.AddTrigger(t1);
            stop.AddTrigger(t2);
            AICloseStop = new AI(move);
            
        }

    }
}
