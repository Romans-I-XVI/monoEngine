using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;


namespace monogame
{
	static class InputHandler
	{
		private static KeyboardState currentKeyboardState = Keyboard.GetState();
		private static KeyboardState previousKeyboardState = Keyboard.GetState();

		public static void Update() {
			previousKeyboardState = currentKeyboardState;
			currentKeyboardState = Keyboard.GetState ();
		}

		public static bool isKeyPressed(Keys key){
			if (!previousKeyboardState.IsKeyDown (key) && currentKeyboardState.IsKeyDown (key)) {
				return true;
			} else {
				return false;
			}
		}

		public static bool isKeyReleased(Keys key) {
			if (previousKeyboardState.IsKeyDown (key) && !currentKeyboardState.IsKeyDown (key)) {
				return true;
			} else {
				return false;
			}
		}

		public static bool isKeyHeld(Keys key) {
			if (currentKeyboardState.IsKeyDown (key)) {
				return true;
			} else {
				return false;
			}
		}


	}
}

