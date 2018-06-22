﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine
{
    public class MouseEventArgs : EventArgs
    {
        public MouseButtons Button { get; private set; }
        public Point Position { get; private set; }
        public int ScrollWheelValue { get; private set; }

        public MouseEventArgs(MouseButtons button, Point position, int scroll_wheel_value)
        {
            Button = button;
            Position = position;
            ScrollWheelValue = scroll_wheel_value;
        }
    }
}