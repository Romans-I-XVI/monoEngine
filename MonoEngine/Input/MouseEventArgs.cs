using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoEngine
{
    public class MouseEventArgs : EventArgs
    {
        public MouseButtons Button { get; private set; }
        public Point Position { get; private set; }
        public int ScrollWheelValue { get; private set; }

        public MouseEventArgs(MouseButtons button, Point position, int scrollWheelValue)
        {
            Button = button;
            Position = position;
            ScrollWheelValue = scrollWheelValue;
        }
    }
}