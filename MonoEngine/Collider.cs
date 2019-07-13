using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoEngine
{
    public abstract class Collider
    {
        public Entity Owner { get; }
        public string Name { get; }
        public bool Enabled = true;
        protected Collider(Entity owner, string name)
        {
            Owner = owner;
            Name = name;
        }
    }

    public class ColliderCircle : Collider
    {
        public Point Position => new Point((int)Owner.Position.X + Offset.X, (int)Owner.Position.Y + Offset.Y);
        public Circle Circle => new Circle(Position.X, Position.Y, Radius);
        public float Radius;
        public Point Offset;

        public ColliderCircle(Entity owner, string name, float radius, int offset_x, int offset_y, bool enabled = true) : base(owner, name)
        {
            Radius = radius;
            Offset = new Point(offset_x, offset_y);
            Enabled = enabled;
        }
    }

    public class ColliderRectangle : Collider
    {
        public Point Position => new Point((int)Owner.Position.X + Offset.X, (int)Owner.Position.Y + Offset.Y);
        public Rectangle Rectangle => new Rectangle(Position.X, Position.Y, Width, Height);
        public Point Offset;
        public int Width;
        public int Height;

        public ColliderRectangle(Entity owner, string name, int offset_x, int offset_y, int width, int height, bool enabled = true) : base(owner, name)
        {
            Offset = new Point(offset_x, offset_y);
            Width = width;
            Height = height;
            Enabled = enabled;
        }
    }
}
