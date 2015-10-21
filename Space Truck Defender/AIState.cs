using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Space_Truck_Defender
{
    public class AIState:Piece
    {
        private Actor Body;
        private Target MoveTarget,
                        ShootTarget;
        private bool Moving, Shooting;
        //a list of events that could change the AI state
        private List<AITrigger> TriggerList;

        public AIState()
        {
            TriggerList = new List<AITrigger>();
        }

        /* Use this constructor for instantiating "data"
         * versions of the AI. These objects will not be
         * added to the board.
         */

        /* Use this contructor for adding board copies of the
         * AI state. 
         */

        public override void Update(GameTime gt)
        {
            if (Moving)
                Body.Go();
                if (MoveTarget != null)
                {
                    Body.SetHeading(PointAt(MoveTarget));
                }
            else
                Body.Stop();

            if (Shooting)
                Body.Shoot();
                if (ShootTarget != null)
                {
                    Body.SetWeaponHeadings(PointAt(ShootTarget));
                }
            else
                Body.HoldFire();



        }

        /* This method returns the apropriate heading for the 
         * actor to "point" at the given target.
         */
        private double PointAt(Target t)
        {
            var tp = t.GetPosition();
            var bp = Body.GetPosition();
            double dir = Math.Atan2(tp.X - bp.X, tp.Y - bp.Y);
            return dir;
        }

        public List<AITrigger> GetTriggers()
        {
            return this.TriggerList;
        }

        public void AddTrigger(AITrigger t)
        {
            TriggerList.Add(t);
        }

    }
}
