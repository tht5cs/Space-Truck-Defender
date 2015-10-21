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
        private Target AITarget;
        private bool Moving, Shooting;
        //a list of events that could change the AI state
        private List<AITrigger> TriggerList;

        public AIState()
        {
            TriggerList = new List<AITrigger>();
        }

        public override void Update(GameTime gt)
        {
            if (Moving)
                Body.Go();
            else
                Body.Stop();

            if (Shooting)
                Body.Shoot();
            else
                Body.HoldFire();

            if (AITarget != null)
            {
                var tp = AITarget.GetPosition();
                var bp = Body.GetPosition();
                double dir = Math.Atan2(tp.X - bp.X, tp.Y - bp.Y);
                Body.SetHeading(dir);
                Body.SetWeaponHeadings(dir);
            }

        }

        public List<AITrigger> GetTriggers()
        {
            return this.TriggerList;
        }
    }
}
