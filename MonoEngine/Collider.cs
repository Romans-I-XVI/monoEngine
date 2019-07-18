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
        public ColliderLayer MemberFlags = ColliderLayer.One;
        public ColliderLayer CollidableFlags = ColliderLayer.One;
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

        public ColliderCircle(Entity owner, string name, float radius, int offsetX, int offsetY, bool enabled = true) : base(owner, name)
        {
            Radius = radius;
            Offset = new Point(offsetX, offsetY);
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

        public ColliderRectangle(Entity owner, string name, int offsetX, int offsetY, int width, int height, bool enabled = true) : base(owner, name)
        {
            Offset = new Point(offsetX, offsetY);
            Width = width;
            Height = height;
            Enabled = enabled;
        }
    }
}
