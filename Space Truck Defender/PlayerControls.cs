using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Space_Truck_Defender
{
    /* This class connects the Input manager to individual player instances
     * it allows each player to have their own input and control settings
     */
    public class PlayerControls : Piece
    {
        //the player to be controlled
        private Actor Player;
        //input settings
        public Keys Up, Down, Left, Right;
        public Keys Shoot, Special;

        private float RespawnTimeMax,
                        RespawnTimeCurr;

        //default constructor with default input settings
        public PlayerControls(Actor _player)
        {
            Player = _player;

            Up = Keys.Up;
            Down = Keys.Down;
            Left = Keys.Left;
            Right = Keys.Right;

            Shoot = Keys.Z;
            Special = Keys.X;

            RespawnTimeMax = 3f;
            RespawnTimeCurr = RespawnTimeMax;

        }

        public void UpdatePlayer(InputManager input)
        {
            UpdateMovementInput(input);
            UpdateShootingInput(input);
        }

        public override void Update(GameTime gt)
        {
            if (Player.IsDestroyed())
                RespawnTimeCurr -= (float)gt.ElapsedGameTime.TotalSeconds;
        }



        public double GetRespawnTimeCurr()
        {
            return this.RespawnTimeCurr;
        }
        public void ResetRespawnTime()
        {
            RespawnTimeCurr = RespawnTimeMax;
        }

        private void UpdateMovementInput(InputManager input)
        {
            float dY = 0;
            float dX = 0;
            if (input.isPressed(Up))
                dY -= 1;
            if (input.isPressed(Down))
                dY += 1;
            if (input.isPressed(Left))
                dX -= 1;
            if (input.isPressed(Right))
                dX += 1;
            if (dX == 0 && dY == 0)
                Player.Stop();
            else
                Player.Go();
            Player.SetHeading(Math.Atan2(dY, dX));
        }

        private void UpdateShootingInput(InputManager input)
        {
            if (input.isPressed(Shoot))
                Player.Shoot();
            else
                Player.HoldFire();
        }


        public Actor GetPlayer()
        {
            return this.Player;
        }

    }
}
