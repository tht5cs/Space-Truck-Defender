using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Space_Truck_Defender
{

    public class Piece
    {

        protected bool Delete;


        public virtual void Update(GameTime gt)
        {

        }

        public virtual void Destroy()
        {
            this.Delete = true;
        }

        public virtual bool IsDestroyed()
        {
            return Delete;
        }

        public virtual void Revive()
        {
            this.Delete = false;
        }
    }
}
