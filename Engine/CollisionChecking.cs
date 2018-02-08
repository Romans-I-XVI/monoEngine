﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public static class CollisionChecking
    {
        public static bool RectRect(Rectangle rect1, Rectangle rec2)
        {
            return RectRect(rect1.X, rect1.Y, rect1.Width, rect1.Height, rec2.X, rec2.Y, rec2.Width, rec2.Height);
        }

        public static bool RectRect(int x1, int y1, int w1, int h1, int x2, int y2, int w2, int h2)
        {
            return (x1 < x2 + w2 &&
            x2 < x1 + w1 &&
            y1 < y2 + h2 &&
            y2 < y1 + h1);
        }

        public static bool CircleCircle(int x1, int y1, int r1, int x2, int y2, int r2)
        {
            var dist = Math.Sqrt((x1 - x2) ^ 2 + (y1 - y2) ^ 2);
            return (dist <= r1 + r2);
        }

        public static bool CircleRect(int circle_x, int circle_y, int circle_r, Rectangle rectangle)
        {
            return CircleRect(circle_x, circle_y, circle_r, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        public static bool CircleRect(int cx, int cy, int cr, int rx, int ry, int rw, int rh )
        {
            var circle_distance_x = Math.Abs(cx - rx - rw / 2);
            var circle_distance_y = Math.Abs(cy - ry - rh / 2);

            if (circle_distance_x > (rw / 2 + cr) || circle_distance_y > (rh / 2 + cr))
                return false;
            else if (circle_distance_x <= (rw / 2) || circle_distance_y <= (rh / 2))
                return true;

            return (Math.Pow(circle_distance_x - rw / 2, 2) + Math.Pow(circle_distance_y - rh / 2, 2)) <= Math.Pow(cr, 2);
        }

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
            return (px >= rx && px < (rx + rw) && py >= ry && py < (ry + rh));
        }
    }
}