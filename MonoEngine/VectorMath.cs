using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngine
{
    public static class VectorMath
    {
        public static Vector2 MoveTowards(Vector2 current_pos, Vector2 dest_pos, float total_speed)
        {
            var x_distance = current_pos.X - dest_pos.X;
            var y_distance = current_pos.Y - dest_pos.Y;
            var angle = Math.Atan2(y_distance, x_distance);

            var speed_vector = HypotenuseToVector(total_speed, (float)angle);
            return new Vector2(current_pos.X - speed_vector.X, current_pos.Y - speed_vector.Y);
        }

        public static Vector2 HypotenuseToVector(float hypotenuse, float angle)
        {
            var x = Math.Cos(angle) * hypotenuse;
            var y = Math.Sin(angle) * hypotenuse;

            return new Vector2((float)x, (float)y);
        }

        public static float VectorToHypotenuse(Vector2 vector)
        {
            return (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        }

        public static float TotalDistance(Vector2 position1, Vector2 position2)
        {
            var x_distance = position1.X - position2.X;
            var y_distance = position1.Y - position2.Y;
            var total_distance = Math.Sqrt(x_distance * x_distance + y_distance * y_distance);
            return (float)total_distance;
        }

        public static float GetAngle(Vector2 vector1, Vector2 vector2)
        {
            var x_distance = vector1.X - vector2.X;
            var y_distance = vector1.Y - vector2.Y;
            return (float)(Math.Atan2(y_distance, x_distance) + Math.PI);
        }

        public static float DegreesToRadians(float degrees)
        {
            return (float)((degrees / 180f) * Math.PI);
        }

        public static float RadiansToDegrees(float radians)
        {
            return (float)((180f / Math.PI) * radians);
        }
    }
}
