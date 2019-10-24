using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Linq;

namespace MonoEngine
{
    public static class EngineInputState
    {
        private static readonly List<MouseEventArgs> mouse_pressed_events = new List<MouseEventArgs>();
        private static readonly List<MouseEventArgs> mouse_released_events = new List<MouseEventArgs>();
        private static readonly List<KeyboardEventArgs> keyboard_pressed_events = new List<KeyboardEventArgs>();
        private static readonly List<KeyboardEventArgs> keyboard_released_events = new List<KeyboardEventArgs>();
        private static readonly List<GamePadEventArgs> gamepad_pressed_events = new List<GamePadEventArgs>();
        private static readonly List<GamePadEventArgs> gamepad_released_events = new List<GamePadEventArgs>();
        private static List<MouseButtons> mouse_enum = Enum.GetValues(typeof(MouseButtons)).Cast<MouseButtons>().ToList();
        private static List<Keys> keys_enum = Enum.GetValues(typeof(Keys)).Cast<Keys>().ToList();
        private static List<PlayerIndex> player_index_enum = Enum.GetValues(typeof(PlayerIndex)).Cast<PlayerIndex>().ToList();
        private static List<Buttons> buttons_enum = Enum.GetValues(typeof(Buttons)).Cast<Buttons>().ToList();

        public static bool ButtonHasBeenPressed { get; private set; }
        public static GameTimeSpan LastButtonPressTimer { get; private set; } = new GameTimeSpan();
        public static MouseState MouseState { get; private set; }
        public static KeyboardState KeyboardState { get; private set; }
        public static Dictionary<PlayerIndex, GamePadState> GamepadStates { get; private set; } = new Dictionary<PlayerIndex, GamePadState>()
        {
            {PlayerIndex.One, GamePad.GetState(PlayerIndex.One)},
            {PlayerIndex.Two, GamePad.GetState(PlayerIndex.Two)},
            {PlayerIndex.Three, GamePad.GetState(PlayerIndex.Three)},
            {PlayerIndex.Four, GamePad.GetState(PlayerIndex.Four)}
        };
        public static TouchCollection TouchState { get; private set; }
        public static MouseEventArgs[] MousePresses { get; private set; }
        public static MouseEventArgs[] MouseReleases { get; private set; }
        public static KeyboardEventArgs[] KeyPresses { get; private set; }
        public static KeyboardEventArgs[] KeyReleases { get; private set; }
        public static GamePadEventArgs[] GamepadPresses { get; private set; }
        public static GamePadEventArgs[] GamepadReleases { get; private set; }
        public static TouchLocation[] TouchPresses { get; private set; }
        public static TouchLocation[] TouchReleases { get; private set; }

        public static void Update()
        {
            mouse_pressed_events.Clear();
            mouse_released_events.Clear();
            keyboard_pressed_events.Clear();
            keyboard_released_events.Clear();
            gamepad_pressed_events.Clear();
            gamepad_released_events.Clear();

            foreach (MouseButtons button in mouse_enum)
            {
                if (Input.Mouse.isPressed(button))
                    mouse_pressed_events.Add(new MouseEventArgs(button, Input.Mouse.CurrentPosition, Input.Mouse.CurrentScroll));
                else if (Input.Mouse.isReleased(button))
                    mouse_released_events.Add(new MouseEventArgs(button, Input.Mouse.CurrentPosition, Input.Mouse.CurrentScroll));
            }

            foreach (Keys key in keys_enum)
            {
                if (key == Keys.ChatPadOrange || key == Keys.ChatPadGreen)
                    continue;

                if (Input.Keyboard.isPressed(key))
                    keyboard_pressed_events.Add(new KeyboardEventArgs(key));
                else if (Input.Keyboard.isReleased(key))
                    keyboard_released_events.Add(new KeyboardEventArgs(key));
            }

            foreach (PlayerIndex player in player_index_enum)
            {
                foreach (Buttons button in buttons_enum)
                {
                    if (Input.Gamepad.isPressed(button, player))
                    {
                        ButtonHasBeenPressed = true;
                        LastButtonPressTimer.Mark();
                        gamepad_pressed_events.Add(new GamePadEventArgs(player, button));
                    }
                    else if (Input.Gamepad.isReleased(button, player))
                        gamepad_released_events.Add(new GamePadEventArgs(player, button));
                }
            }

            MouseState = Input.Mouse.GetNewTranslatedState();
            KeyboardState = Keyboard.GetState();
            GamepadStates[PlayerIndex.One] = GamePad.GetState(PlayerIndex.One);
            GamepadStates[PlayerIndex.Two] = GamePad.GetState(PlayerIndex.Two);
            GamepadStates[PlayerIndex.Three] = GamePad.GetState(PlayerIndex.Three);
            GamepadStates[PlayerIndex.Four] = GamePad.GetState(PlayerIndex.Four);
            TouchState = Input.Touch.CurrentTouches;
            MousePresses = mouse_pressed_events.ToArray();
            MouseReleases = mouse_released_events.ToArray();
            KeyPresses = keyboard_pressed_events.ToArray();
            KeyReleases = keyboard_released_events.ToArray();
            GamepadPresses = gamepad_pressed_events.ToArray();
            GamepadReleases = gamepad_released_events.ToArray();
            TouchPresses = Input.Touch.PressedTouches.ToArray();
            TouchReleases = Input.Touch.ReleasedTouches.ToArray();
        }

    }
}
