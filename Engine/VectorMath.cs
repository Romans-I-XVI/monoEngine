using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
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
    }
}
