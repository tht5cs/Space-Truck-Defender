using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;


/* This class has all of the data necessary for actor movement
 * and guidance, as well as movement AI
 * 
 * Note that location is a part of the actor.
 */
namespace Space_Truck_Defender
{
    public class Engine
    {
        private float Speed; // MAX speed in pixels/second
        private float Acceleration; // acceleration in pixels/second^2
        private double Heading; // The direction the actor is to accelerate in
        private Vector2 Velocity; // how quickly the actor is currently moving in the x and y direction

        private bool IsGoing; // whether to stop or go

        //pattern stuff
        //private MovementPattern Pattern;

        /* Types of Engine:
         * 1: Fixed Heading
         *      points in a direction and just keeps going
         * 2: Homing
         *      Attatched to an actor and accelerates in the direction of the actor
         * 3: Pattern
         *      Flies according to its own set of rules (zig zag, looping, etc.)
         */

        public Engine(float _speed, float _accel)
        {
            Speed = _speed;
            Acceleration = _accel;
            Heading = 0;
            IsGoing = false;
            Velocity = new Vector2(0, 0);
            //Pattern = new MovementPattern();
        }

        //copy constructor
        public Engine(Engine e)
        {
            Speed = e.GetSpeed();
            Acceleration = e.GetAcceleration();
            Heading = 0;
            IsGoing = false;
            Velocity = new Vector2(0, 0);
            //Pattern = new MovementPattern();
        }

        public float GetSpeed()
        {
            return Speed;
        }

        public float GetAcceleration()
        {
            return Acceleration;
        }

        public void SetHeading(Double v)
        {
            Heading = v;
        }

        public double GetHeading()
        {
            return this.Heading;
        }

        public void Go()
        {
            IsGoing = true;
        }

        public void Stop()
        {
            IsGoing = false;
        }

        public void Freeze()
        {
            Velocity = new Vector2(0, 0);
            Stop();
        }

        public void Update(GameTime gt)
        {
            UpdateVelocity(gt);
            //Pattern.Update(gt);
        }

        //moves towards the direction determined by Heading
        public void UpdateVelocity(GameTime gt)
        {            
            //if the actor is going, accelerate as normal
            if (IsGoing)
                Accelerate(gt);
                //if actor is not going (stopping) deccelerate/stop
            else
                Decelerate(gt);
        }

        private void Decelerate(GameTime gt)
        {
            if ((Velocity.Y * Velocity.Y) + (Velocity.X * Velocity.X) > 0.2)
            {
                double dir = Math.Atan2(Velocity.Y, Velocity.X) + Math.PI;
                double a = Acceleration * gt.ElapsedGameTime.TotalSeconds;
                //tangent of a triangle = Opposite / Adjacent
                double Yaccel = a * Math.Sin(dir);
                double Xaccel = a * Math.Cos(dir);
                Velocity.X += (float)Xaccel;
                Velocity.Y += (float)Yaccel;
                double newDir = Math.Atan2(Velocity.Y, Velocity.X);
                if (dir == newDir)
                    Velocity = new Vector2(0, 0);
            }
            else
                Velocity = new Vector2(0, 0);
        }

        private void Accelerate(GameTime gt)
        {
            //a is the velocity step for the given gametime
            double a = Acceleration * gt.ElapsedGameTime.TotalSeconds;
            //tangent of a triangle = Opposite / Adjacent
            double Yaccel = a * Math.Sin(Heading);
            double Xaccel = a * Math.Cos(Heading);
            Velocity.X += (float)Xaccel;
            Velocity.Y += (float)Yaccel;
            //if speed breaches max, reduce to normal
            if ((Velocity.Y * Velocity.Y) + (Velocity.X * Velocity.X) > (Speed * Speed))
            {
                double dir = Math.Atan2(Velocity.Y, Velocity.X);
                Velocity.X = Speed * (float)Math.Cos(dir);
                Velocity.Y = Speed * (float)Math.Sin(dir);
            }
        }

        //this method returns the amount an actor has to move for the given timestep
        public Vector2 GetOffset()
        {
            //Vector2 patternStep = Pattern.GetOffset(gt);
            return this.Velocity;
        }
    }
}
