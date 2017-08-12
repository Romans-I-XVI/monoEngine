using System;
using Microsoft.Xna.Framework.Input;

namespace Engine
{
    public class KeyboardEventArgs : EventArgs
    {
        public KeyboardEventArgs(Keys key)
        {
            Key = key;
        }

        public Keys Key { get; }
    }
}