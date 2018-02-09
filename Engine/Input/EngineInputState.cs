using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Engine
{
    public struct EngineInputState
    {

        public MouseState MouseState { get; private set; }
        public KeyboardState KeyboardState { get; private set; }
        public Dictionary<PlayerIndex, GamePadState> GamepadStates { get; private set; }
        public TouchCollection TouchState { get; private set; }
        public List<KeyboardEventArgs> KeyPresses { get; private set; }
        public List<KeyboardEventArgs> KeyReleases { get; private set; }
        public List<GamePadEventArgs> GamepadPresses { get; private set; }
        public List<GamePadEventArgs> GamepadReleases { get; private set; }
        public TouchCollection TouchPresses { get; private set; }
        public TouchCollection TouchReleases { get; private set; }

        public EngineInputState(List<KeyboardEventArgs> key_presses, List<KeyboardEventArgs> key_releases, List<GamePadEventArgs> gamepad_presses, List<GamePadEventArgs> gamepad_releases, TouchCollection touch_presses, TouchCollection touch_releases)
        {
            MouseState = Mouse.GetState();
            KeyboardState = Keyboard.GetState();
            GamepadStates = new Dictionary<PlayerIndex, GamePadState>()
            {
                {PlayerIndex.One, GamePad.GetState(PlayerIndex.One)},
                {PlayerIndex.Two, GamePad.GetState(PlayerIndex.Two)},
                {PlayerIndex.Three, GamePad.GetState(PlayerIndex.Three)},
                {PlayerIndex.Four, GamePad.GetState(PlayerIndex.Four)}
            };
            TouchState = TouchPanel.GetState();
            KeyPresses = key_presses;
            KeyReleases = key_releases;
            GamepadPresses = gamepad_presses;
            GamepadReleases = gamepad_releases;
            TouchPresses = touch_presses;
            TouchReleases = touch_releases;

        }
            
    }
}
