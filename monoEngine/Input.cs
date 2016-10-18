using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;


namespace monogame
{
	static public class Input
	{
		public static void Update() {
			Input.Keyboard.Update ();
			Input.Gamepad.Update ();
		}

		static public class Keyboard {
			private static KeyboardState currentKeyboardState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
			private static KeyboardState previousKeyboardState = Microsoft.Xna.Framework.Input.Keyboard.GetState();

			public static void Update() {
				previousKeyboardState = currentKeyboardState;
				currentKeyboardState = Microsoft.Xna.Framework.Input.Keyboard.GetState ();
			}


			public static bool isPressed(Keys key){
				return (!previousKeyboardState.IsKeyDown (key) && currentKeyboardState.IsKeyDown (key));
			}

			public static bool isReleased(Keys key) {
				return (previousKeyboardState.IsKeyDown (key) && !currentKeyboardState.IsKeyDown (key));
			}

			public static bool isHeld(Keys key) {
				return currentKeyboardState.IsKeyDown (key);
			}
		}

		static public class Gamepad
		{
			public static Dictionary<PlayerIndex, GamePadState> currentGamepadState = new Dictionary<PlayerIndex, GamePadState>() {
				{PlayerIndex.One, Microsoft.Xna.Framework.Input.GamePad.GetState(PlayerIndex.One)},
				{PlayerIndex.Two, Microsoft.Xna.Framework.Input.GamePad.GetState(PlayerIndex.Two)},
				{PlayerIndex.Three, Microsoft.Xna.Framework.Input.GamePad.GetState(PlayerIndex.Three)},
				{PlayerIndex.Four, Microsoft.Xna.Framework.Input.GamePad.GetState(PlayerIndex.Four)}
			};
			private static Dictionary<PlayerIndex, GamePadState> previousGamepadState = new Dictionary<PlayerIndex, GamePadState>() {
				{PlayerIndex.One, Microsoft.Xna.Framework.Input.GamePad.GetState(PlayerIndex.One)},
				{PlayerIndex.Two, Microsoft.Xna.Framework.Input.GamePad.GetState(PlayerIndex.Two)},
				{PlayerIndex.Three, Microsoft.Xna.Framework.Input.GamePad.GetState(PlayerIndex.Three)},
				{PlayerIndex.Four, Microsoft.Xna.Framework.Input.GamePad.GetState(PlayerIndex.Four)}
			};

			public static void Update() {
				foreach (KeyValuePair<PlayerIndex, GamePadState> entry in currentGamepadState) {
					previousGamepadState [entry.Key] = entry.Value;
				}
				foreach (PlayerIndex key in Enum.GetValues(typeof(PlayerIndex))) {
					currentGamepadState [key] = Microsoft.Xna.Framework.Input.GamePad.GetState (key);
				}
			}


			public static bool isPressed(Buttons button, PlayerIndex player_index){
				return (!previousGamepadState [player_index].IsButtonDown (button) && currentGamepadState [player_index].IsButtonDown (button));
			}

			public static bool isReleased(Buttons button, PlayerIndex player_index) {
				return (previousGamepadState [player_index].IsButtonDown (button) && !currentGamepadState [player_index].IsButtonDown (button));
			}

			public static bool isHeld(Buttons button, PlayerIndex player_index) {
				return currentGamepadState [player_index].IsButtonDown (button);
			}

			public static Vector2 leftThumbstick(PlayerIndex player_index){
				return currentGamepadState [player_index].ThumbSticks.Left;
			}

			public static Vector2 rightThumbstick(PlayerIndex player_index){
				return currentGamepadState [player_index].ThumbSticks.Right;
			}
		}
	}
}

