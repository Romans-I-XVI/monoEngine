using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Linq;

namespace Engine
{
    public class EngineInputState
    {
        private readonly List<KeyboardEventArgs> keyboard_pressed_events = new List<KeyboardEventArgs>();
        private readonly List<KeyboardEventArgs> keyboard_released_events = new List<KeyboardEventArgs>();
        private readonly List<GamePadEventArgs> gamepad_pressed_events = new List<GamePadEventArgs>();
        private readonly List<GamePadEventArgs> gamepad_released_events = new List<GamePadEventArgs>();
        private List<Keys> keys_enum = Enum.GetValues(typeof(Keys)).Cast<Keys>().ToList();
        private List<PlayerIndex> player_index_enum = Enum.GetValues(typeof(PlayerIndex)).Cast<PlayerIndex>().ToList();
        private List<Buttons> buttons_enum = Enum.GetValues(typeof(Buttons)).Cast<Buttons>().ToList();

        public bool ButtonHasBeenPressed { get; private set; }
        public GameTimeSpan _last_button_press_timer { get; private set; }
        public MouseState MouseState { get; private set; }
        public KeyboardState KeyboardState { get; private set; }
        public Dictionary<PlayerIndex, GamePadState> GamepadStates { get; private set; }
        public TouchCollection TouchState { get; private set; }
        public List<KeyboardEventArgs> KeyPresses { get; private set; }
        public List<KeyboardEventArgs> KeyReleases { get; private set; }
        public List<GamePadEventArgs> GamepadPresses { get; private set; }
        public List<GamePadEventArgs> GamepadReleases { get; private set; }
        public List<TouchLocation> TouchPresses { get; private set; }
        public List<TouchLocation> TouchReleases { get; private set; }

        public EngineInputState()
        {
            _last_button_press_timer = new GameTimeSpan();
            GamepadStates = new Dictionary<PlayerIndex, GamePadState>()
            {
                {PlayerIndex.One, GamePad.GetState(PlayerIndex.One)},
                {PlayerIndex.Two, GamePad.GetState(PlayerIndex.Two)},
                {PlayerIndex.Three, GamePad.GetState(PlayerIndex.Three)},
                {PlayerIndex.Four, GamePad.GetState(PlayerIndex.Four)}
            };
            Update();
        }

        public void Update()
        {
            Input.Update();

            keyboard_pressed_events.Clear();
            keyboard_released_events.Clear();
            gamepad_pressed_events.Clear();
            gamepad_released_events.Clear();

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
                        _last_button_press_timer.Mark();
                        gamepad_pressed_events.Add(new GamePadEventArgs(player, button));
                    }
                    else if (Input.Gamepad.isReleased(button, player))
                        gamepad_released_events.Add(new GamePadEventArgs(player, button));
                }
            }

            MouseState = Mouse.GetState();
            KeyboardState = Keyboard.GetState();
            GamepadStates[PlayerIndex.One] = GamePad.GetState(PlayerIndex.One);
            GamepadStates[PlayerIndex.Two] = GamePad.GetState(PlayerIndex.Two);
            GamepadStates[PlayerIndex.Three] = GamePad.GetState(PlayerIndex.Three);
            GamepadStates[PlayerIndex.Four] = GamePad.GetState(PlayerIndex.Four);
            TouchState = Input.Touch.CurrentTouches;
            KeyPresses = keyboard_pressed_events;
            KeyReleases = keyboard_released_events;
            GamepadPresses = gamepad_pressed_events;
            GamepadReleases = gamepad_released_events;
            TouchPresses = Input.Touch.PressedTouches;
            TouchReleases = Input.Touch.ReleasedTouches;

        }

    }
}
