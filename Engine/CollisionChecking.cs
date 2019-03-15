using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public static class CollisionChecking
    {
        // A method with overrides for checking anything against anything without specifying the type of collision
        public static bool Check(Rectangle rect1, Rectangle rect2)
        {
            return RectRect(rect1, rect2);
        }

        public static bool Check(Circle circle1, Circle circle2)
        {
            return CircleCircle(circle1, circle2);
        }

        public static bool Check(Circle circle, Rectangle rect)
        {
            return CircleRect(circle, rect);
        }

        public static bool Check(Point point, Rectangle rect)
        {
            return PointRect(point, rect);
        }

        public static bool Check(Vector2 point, Rectangle rect)
        {
            return PointRect(point, rect);
        }

        public static bool Check(Point point, Circle circle)
        {
            return PointCircle(point, circle);
        }

        public static bool Check(Vector2 point, Circle circle)
        {
            return PointCircle(point, circle);
        }

        // Rectangle to Rectangle
        public static bool RectRect(Rectangle rect1, Rectangle rect2)
        {
            return RectRect(rect1.X, rect1.Y, rect1.Width, rect1.Height, rect2.X, rect2.Y, rect2.Width, rect2.Height);
        }

        public static bool RectRect(int x1, int y1, int w1, int h1, int x2, int y2, int w2, int h2)
        {
            return (x1 < x2 + w2 &&
            x2 < x1 + w1 &&
            y1 < y2 + h2 &&
            y2 < y1 + h1);
        }

        // Circle to Circle
        public static bool CircleCircle(Circle circle1, Circle circle2)
        {
            return CircleCircle(circle1.X, circle1.Y, circle1.Radius, circle2.X, circle2.Y, circle2.Radius);
        }

        public static bool CircleCircle(int x1, int y1, float r1, int x2, int y2, float r2)
        {
            int distance_x = x1 - x2;
            int distance_y = y1 - y2;
            var dist = Math.Sqrt(distance_x * distance_x + distance_y * distance_y);
            return (dist <= r1 + r2);
        }

        // Circle to Rectangle
        public static bool CircleRect(Circle circle, Rectangle rectangle)
        {
            return CircleRect(circle.X, circle.Y, circle.Radius, rectangle);
        }

        public static bool CircleRect(int circle_x, int circle_y, float circle_r, Rectangle rectangle)
        {
            return CircleRect(circle_x, circle_y, circle_r, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        public static bool CircleRect(int cx, int cy, float cr, int rx, int ry, int rw, int rh )
        {
            var circle_distance_x = Math.Abs(cx - rx - rw / 2);
            var circle_distance_y = Math.Abs(cy - ry - rh / 2);

            if (circle_distance_x > (rw / 2 + cr) || circle_distance_y > (rh / 2 + cr))
                return false;
            else if (circle_distance_x <= (rw / 2) || circle_distance_y <= (rh / 2))
                return true;

            return (Math.Pow(circle_distance_x - rw / 2, 2) + Math.Pow(circle_distance_y - rh / 2, 2)) <= Math.Pow(cr, 2);
        }

        // Point to Rectangle
        public static bool PointRect(Point point, Rectangle rectangle)
        {
            return PointRect(point.X, point.Y, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        public static bool PointRect(Vector2 vector, Rectangle rectangle)
        {
            return PointRect((int)vector.X, (int)vector.Y, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        public static bool PointRect(int px, int py, int rx, int ry, int rw, int rh)
        {
            return (px >= rx && px <= (rx + rw) && py >= ry && py <= (ry + rh));
        }

        // Point to Circle
        public static bool PointCircle(Point point, Circle circle)
        {
            return PointCircle(point.X, point.Y, circle.X, circle.Y, circle.Radius);
        }
        
        public static bool PointCircle(Vector2 point, Circle circle)
        {
            return PointCircle((int)point.X, (int)point.Y, circle.X, circle.Y, circle.Radius);
        }

        public static bool PointCircle(int px, int py, int cx, int cy, float cr)
        {

            int distance_x = px - cx;
            int distance_y = py - cy;
            var dist = Math.Sqrt(distance_x * distance_x + distance_y * distance_y);
            return (dist <= cr);
        }
    }
}
