using System;
using System.Collections.Generic;
using System.Text;

namespace MonoEngine
{
    public struct Circle
    {
        public int X;
        public int Y;
        public float Radius;
        public float Diameter { get { return Radius * 2; } }
        public double Circumference { get { return 2 * Math.PI * Radius; } }

        public Circle(int x, int y, float radius)
        {
            X = x;
            Y = y;
            Radius = radius;
        }
    }
}
