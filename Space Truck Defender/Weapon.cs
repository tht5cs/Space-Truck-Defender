using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Space_Truck_Defender
{
    public class Weapon
    {
        private Actor Projectile; // the things it shoots
        private float MaxCooldown, //weapon cooldown, in seconds
                        CurrCooldown; // the amount of time until the weapon can fire again
        private List<double> HeadingOffsets; // the heading offset of each shot
        private double Heading, // The centered path of the projectiles
                        AccOffset; // projectile deviation from stated path, in radians

        public Weapon(Actor _projectile, List<double> _headings, float CDownMax, double _AccOffset)
        {
            this.Projectile = _projectile;
            this.HeadingOffsets = _headings;
            MaxCooldown = CDownMax;           
            AccOffset = _AccOffset;
            CurrCooldown = 0;
            Heading = Math.PI * 3 / 2; //this means straight "up" (no x vector, negative y vector)
        }

        //Copy constructor
        public Weapon(Weapon w)
        {
            Projectile = w.Projectile;
            this.HeadingOffsets = new List<double>();
            foreach (var d in w.HeadingOffsets)
                HeadingOffsets.Add(d);
            MaxCooldown = w.MaxCooldown;
            AccOffset = w.AccOffset;
            CurrCooldown = 0;
            Heading = w.Heading;
        }

        public void SetCollision(int i)
        {
            Projectile.SetCollision(i);
        }

        public void Update(GameTime gt)
        {
            CurrCooldown -= (float)gt.ElapsedGameTime.TotalSeconds;
            CurrCooldown = Math.Max(CurrCooldown, 0);
        }

        public double GetAccOffset()
        {
            return AccOffset;
        }

        public double GetHeading()
        {
            return this.Heading;
        }
        public void SetHeading(double d)
        {
            this.Heading = d;
        }

        public float GetCurrentCooldown()
        {
            return CurrCooldown;
        }

        public List<double> GetHeadingOffsets()
        {
            return this.HeadingOffsets;
        }

        public Actor GetProjectile()
        {
            return Projectile;
        }

        public void Reload()
        {
            CurrCooldown = MaxCooldown;
        }

    }
}
