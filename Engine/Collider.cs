using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public abstract class Collider
    {
        public Entity Owner { get; private set; }
        public string Name { get; private set; }
        public bool Enabled = true;
        protected Collider(Entity owner, string name)
        {
            Owner = owner;
            Name = name;
        }

        public abstract void UpdateColliderPosition();
    }

    public class ColliderCircle : Collider
    {
        public Point Position = new Point();
        public float Radius;
        public Point Offset;

        public ColliderCircle(Entity owner, string name, float radius, int offset_x, int offset_y, bool enabled = true) : base(owner, name)
        {
            Radius = radius;
            Offset = new Point(offset_x, offset_y);
            Enabled = enabled;
            UpdateColliderPosition();
        }

        public override void UpdateColliderPosition()
        {
            Position.X = (int)Owner.Position.X + Offset.X;
            Position.Y = (int)Owner.Position.Y + Offset.Y;
        }
    }

    public class ColliderRectangle : Collider
    {
        public Point Position = new Point();
        public Point Offset;
        public int Width;
        public int Height;

        public ColliderRectangle(Entity owner, string name, int offset_x, int offset_y, int width, int height, bool enabled = true) : base(owner, name)
        {
            Offset = new Point(offset_x, offset_y);
            Width = width;
            Height = height;
            Enabled = enabled;
            UpdateColliderPosition();
        }

        public override void UpdateColliderPosition()
        {
            Position.X = (int)Owner.Position.X + Offset.X;
            Position.Y = (int)Owner.Position.Y + Offset.Y;
        }
    }
}
