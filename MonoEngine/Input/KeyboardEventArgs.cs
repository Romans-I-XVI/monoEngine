using System;
using Microsoft.Xna.Framework.Input;

namespace MonoEngine
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