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

        public abstract bool CheckCollision(Collider other_collider);
    }

    public class ColliderCircle : Collider
    {
        public float Radius;
        public Point Offset;
        public Circle Circle
        {
            get
            {
                return new Circle((int)Owner.Position.X + Offset.X, (int)Owner.Position.Y + Offset.Y, Radius);
            }
        }

        public ColliderCircle(Entity owner, string name, float radius, int offset_x, int offset_y, bool enabled = true) : base(owner, name)
        {
            Radius = radius;
            Offset = new Point(offset_x, offset_y);
            Enabled = enabled;
        }

        public override bool CheckCollision(Collider other_collider)
        {
            if (other_collider is ColliderCircle)
            {
                ColliderCircle subc = (ColliderCircle)other_collider;
                var x1 = (int)Owner.Position.X + Offset.X;
                var y1 = (int)Owner.Position.Y + Offset.Y;
                var r1 = Radius;
                var x2 = (int)subc.Owner.Position.X + subc.Offset.X;
                var y2 = (int)subc.Owner.Position.Y + subc.Offset.Y;
                var r2 = subc.Radius;

                return CollisionChecking.CircleCircle(x1, y1, r1, x2, y2, r2);
            }
            else if (other_collider is ColliderRectangle)
            {
                ColliderRectangle subc = (ColliderRectangle)other_collider;
                var cx = (int)Owner.Position.X + Offset.X;
                var cy = (int)Owner.Position.Y + Offset.Y;
                var cr = Radius;
                var rx = (int)subc.Owner.Position.X + subc.Offset.X;
                var ry = (int)subc.Owner.Position.Y + subc.Offset.Y;
                var rw = subc.Width;
                var rh = subc.Height;

                return CollisionChecking.CircleRect(cx, cy, cr, rx, ry, rw, rh);
            }
            else
            {
                return false;
            }
        }
    }

    public class ColliderRectangle : Collider
    {
        public Point Offset;
        public int Width;
        public int Height;
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Owner.Position.X + Offset.X, (int)Owner.Position.Y + Offset.Y, Width, Height);
            }
        }

        public ColliderRectangle(Entity owner, string name, int offset_x, int offset_y, int width, int height, bool enabled = true) : base(owner, name)
        {
            Offset = new Point(offset_x, offset_y);
            Width = width;
            Height = height;
            Enabled = enabled;
        }

        public override bool CheckCollision(Collider other_collider)
        {
            if (other_collider is ColliderCircle)
            {
                ColliderCircle subc = (ColliderCircle)other_collider;
                var cx = (int)subc.Owner.Position.X + subc.Offset.X;
                var cy = (int)subc.Owner.Position.Y + subc.Offset.Y;
                var cr = subc.Radius;
                var rx = (int)Owner.Position.X + Offset.X;
                var ry = (int)Owner.Position.Y + Offset.Y;
                var rw = Width;
                var rh = Height;

                return CollisionChecking.CircleRect(cx, cy, cr, rx, ry, rw, rh);
            }
            else if (other_collider is ColliderRectangle)
            {
                ColliderRectangle subc = (ColliderRectangle)other_collider;
                var x1 = (int)Owner.Position.X + Offset.X;
                var y1 = (int)Owner.Position.Y + Offset.Y;
                var w1 = Width;
                var h1 = Height;
                var x2 = (int)subc.Owner.Position.X + subc.Offset.X;
                var y2 = (int)subc.Owner.Position.Y + subc.Offset.Y;
                var w2 = subc.Width;
                var h2 = subc.Height;

                return CollisionChecking.RectRect(x1, y1, w1, h1, x2, y2, w2, h2);
            }
            else
            {
                return false;
            }
        }
    }
}
