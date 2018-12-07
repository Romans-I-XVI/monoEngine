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
        protected bool isCollisionValid(Collider other_collider)
        {
            return (Enabled && other_collider.Enabled && !Owner.IsExpired && !other_collider.Owner.IsExpired);
        }
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
            if (isCollisionValid(other_collider))
            {
                if (other_collider is ColliderCircle)
                {
                    return CollisionChecking.CircleCircle(this.Circle, ((ColliderCircle)other_collider).Circle);
                }
                else if (other_collider is ColliderRectangle)
                {
                    return CollisionChecking.CircleRect(this.Circle, ((ColliderRectangle)other_collider).Rectangle);
                }
                else
                {
                    return false;
                }
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
            if (isCollisionValid(other_collider))
            {
                if (other_collider is ColliderCircle)
                {
                    return CollisionChecking.CircleRect(((ColliderCircle)other_collider).Circle, this.Rectangle);
                }
                else if (other_collider is ColliderRectangle)
                {
                    return CollisionChecking.RectRect(this.Rectangle, ((ColliderRectangle)other_collider).Rectangle);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
