using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
//using Tao.Sdl;

namespace Space_Truck_Defender
{
	public class InputManager
	{
		private KeyboardState kb;
		private KeyboardState kbo;
		private GamePadState gp;
		private GamePadState gpo;

        private List<PlayerControls> PlayerControlsList;

		public InputManager()
		{
			this.kb = Keyboard.GetState();
			this.kbo = Keyboard.GetState();
			this.gp = GamePad.GetState(PlayerIndex.One);
			this.gpo = GamePad.GetState(PlayerIndex.One);
			//Console.WriteLine (Sdl.SDL_JoystickName (0));
            PlayerControlsList = new List<PlayerControls>();
		}

        public void AddPlayer(PlayerControls p)
        {
            PlayerControlsList.Add(p);
        }

		public void Update()
		{
			kbo = kb;
			gpo = gp;
			kb = Keyboard.GetState();
			this.gp = GamePad.GetState(PlayerIndex.One);

            //UpdatePlayers();
		}


		public bool isPressed(Keys key, Buttons button)
		{
			//Console.WriteLine (button);
			return kb.IsKeyDown(key) || gp.IsButtonDown(button);
		}

        public bool isPressed(Keys key)
        {
            //Console.WriteLine (button);
            return kb.IsKeyDown(key);
        }

		public bool onPress(Keys key, Buttons button)
		{
			if ((gp.IsButtonDown (button) && gpo.IsButtonUp (button))) {
				Console.WriteLine (button);
			}
			return (kb.IsKeyDown(key) && kbo.IsKeyUp(key)) ||
				(gp.IsButtonDown(button) && gpo.IsButtonUp(button));
		}

		public bool onRelease(Keys key, Buttons button)
		{
			//Console.WriteLine (button);
			return (kb.IsKeyUp(key) && kbo.IsKeyDown(key)) ||
				(gp.IsButtonUp(button) && gpo.IsButtonDown(button));
		}

		public bool isHeld(Keys key, Buttons button)
		{
			//Console.WriteLine (button);
			return (kb.IsKeyDown(key) && kbo.IsKeyDown(key)) ||
				(gp.IsButtonDown(button) && gpo.IsButtonDown(button));
		}
	
	}
}

