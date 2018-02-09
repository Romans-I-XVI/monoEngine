﻿using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Engine
{
	static public class Input
	{
		public static void Update() {
			Input.Keyboard.Update ();
			Input.Gamepad.Update ();
            Input.Touch.Update();
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

			public static bool isPressed(Buttons button){
				bool is_pressed = false;
				foreach (PlayerIndex key in Enum.GetValues(typeof(PlayerIndex))) {
					if (!previousGamepadState [key].IsButtonDown (button) && currentGamepadState [key].IsButtonDown (button)) {
						is_pressed = true;
						break;
					}
				}
				return is_pressed;
			}

			public static bool isReleased(Buttons button, PlayerIndex player_index) {
				return (previousGamepadState [player_index].IsButtonDown (button) && !currentGamepadState [player_index].IsButtonDown (button));
			}

			public static bool isReleased(Buttons button) {
				bool is_released = false;
				foreach (PlayerIndex key in Enum.GetValues(typeof(PlayerIndex))) {
					if (previousGamepadState [key].IsButtonDown (button) && !currentGamepadState [key].IsButtonDown (button)) {
						is_released = true;
						break;
					}
				}
				return is_released;
			}

			public static bool isHeld(Buttons button, PlayerIndex player_index) {
				return currentGamepadState [player_index].IsButtonDown (button);
			}

			public static bool isHeld(Buttons button) {
				bool is_held = false;
				foreach (PlayerIndex key in Enum.GetValues(typeof(PlayerIndex))) {
					if (currentGamepadState [key].IsButtonDown (button)) {
						is_held = true;
						break;
					}
				}
				return is_held;
			}

			public static Vector2 leftThumbstick(PlayerIndex player_index){
				return currentGamepadState [player_index].ThumbSticks.Left;
			}

			public static Vector2 rightThumbstick(PlayerIndex player_index){
				return currentGamepadState [player_index].ThumbSticks.Right;
			}
		}

        public static class Touch
        {
            public static List<TouchLocation> PressedTouches = new List<TouchLocation>();
            public static List<TouchLocation> ReleasedTouches = new List<TouchLocation>();

            private static TouchCollection currentTouchState = TouchPanel.GetState();

            public static void Update()
            {
                currentTouchState = TouchPanel.GetState();

                PressedTouches.Clear();
                ReleasedTouches.Clear();

                foreach (var touch in currentTouchState)
                {
                    var translated_point = GameRoot.BoxingViewport.PointToScreen((int)touch.Position.X, (int)touch.Position.Y);
                    var translated_touch = new TouchLocation(touch.Id, touch.State, new Vector2(translated_point.X, translated_point.Y));
                    if (touch.State == TouchLocationState.Pressed)
                        PressedTouches.Add(translated_touch);
                    else if (touch.State == TouchLocationState.Released)
                        ReleasedTouches.Add(translated_touch);
                }
            }
        }
    }
}

