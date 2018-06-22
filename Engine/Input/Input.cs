using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Engine
{
	static public class Input
	{
		public static void Update() {
            Input.Mouse.Update();
            Input.Keyboard.Update();
            Input.Gamepad.Update();
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
            private static List<PlayerIndex> player_index_enum = new List<PlayerIndex>()
            {
                PlayerIndex.One,
                PlayerIndex.Two,
                PlayerIndex.Three,
                PlayerIndex.Four
            };
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
                foreach (var key in player_index_enum)
                {
                    previousGamepadState[key] = currentGamepadState[key];
                }
                foreach (var key in player_index_enum)
                {
                    currentGamepadState[key] = Microsoft.Xna.Framework.Input.GamePad.GetState(key);
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

        public static class Mouse
        {
            public static Point CurrentPosition { get; private set; }
            public static int CurrentScroll { get { return currentMouseState.ScrollWheelValue; } }
            private static MouseState currentMouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();
            private static MouseState previousMouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();

            public static void Update()
            {
                previousMouseState = currentMouseState;
                currentMouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();
                CurrentPosition = GameRoot.BoxingViewport.PointToScreen(currentMouseState.Position.X, currentMouseState.Position.Y);
            }

            public static MouseState GetNewTranslatedState()
            {
                return new MouseState(CurrentPosition.X, CurrentPosition.Y, currentMouseState.ScrollWheelValue, currentMouseState.LeftButton, currentMouseState.MiddleButton, currentMouseState.RightButton, currentMouseState.XButton1, currentMouseState.XButton2);
            }

            public static bool isPressed(MouseButtons button)
            {
                return (!isMouseButtonDown(previousMouseState, button) && isMouseButtonDown(currentMouseState, button));
            }

            public static bool isReleased(MouseButtons button)
            {
                return (isMouseButtonDown(previousMouseState, button) && !isMouseButtonDown(currentMouseState, button));
            }

            public static bool isHeld(MouseButtons button)
            {
                return isMouseButtonDown(currentMouseState, button);
            }

            private static bool isMouseButtonDown(MouseState state, MouseButtons button)
            {
                switch (button)
                {
                    case MouseButtons.LeftButton:
                        return state.LeftButton == ButtonState.Pressed;
                    case MouseButtons.RightButton:
                        return state.RightButton == ButtonState.Pressed;
                    case MouseButtons.MiddleButton:
                        return state.MiddleButton == ButtonState.Pressed;
                    case MouseButtons.XButton1:
                        return state.XButton1 == ButtonState.Pressed;
                    case MouseButtons.XButton2:
                        return state.XButton2 == ButtonState.Pressed;
                    default:
                        return state.LeftButton == ButtonState.Pressed;
                }
            }
        }

        public static class Touch
        {
            public static List<TouchLocation> PressedTouches = new List<TouchLocation>();
            public static List<TouchLocation> ReleasedTouches = new List<TouchLocation>();
            public static TouchCollection CurrentTouches = new TouchCollection();

            public static void Update()
            {
                var touch_state = TouchPanel.GetState();
                TouchLocation[] translated_touch_array = new TouchLocation[touch_state.Count];
                for (int i = 0; i < touch_state.Count; i++)
                {
                    TouchLocation touch = touch_state[i];
                    var translated_point = GameRoot.BoxingViewport.PointToScreen((int)touch.Position.X, (int)touch.Position.Y);
                    var translated_touch = new TouchLocation(touch.Id, touch.State, new Vector2(translated_point.X, translated_point.Y));
                    translated_touch_array[i] = translated_touch;
                }

                CurrentTouches = new TouchCollection(translated_touch_array);
                PressedTouches.Clear();
                ReleasedTouches.Clear();

                foreach (var touch in CurrentTouches)
                {
                    if (touch.State == TouchLocationState.Pressed)
                        PressedTouches.Add(touch);
                    else if (touch.State == TouchLocationState.Released)
                        ReleasedTouches.Add(touch);
                }
            }
        }
    }
}

