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
        public static Vector2 MoveTowards(Vector2 currentPos, Vector2 destPos, float totalSpeed)
        {
            var distanceX = currentPos.X - destPos.X;
            var distanceY = currentPos.Y - destPos.Y;
            var angle = Math.Atan2(distanceY, distanceX);

            var speedVector = HypotenuseToVector(totalSpeed, (float)angle);
            return new Vector2(currentPos.X - speedVector.X, currentPos.Y - speedVector.Y);
        }

        public static Vector2 HypotenuseToVector(float hypotenuse, float angleInRadians)
        {
            var x = Math.Cos(angleInRadians) * hypotenuse;
            var y = Math.Sin(angleInRadians) * hypotenuse;

            return new Vector2((float)x, (float)y);
        }

        public static float VectorToHypotenuse(Vector2 vector)
        {
            return (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        }

        public static float TotalDistance(Vector2 position1, Vector2 position2)
        {
            var distanceX = position1.X - position2.X;
            var distanceY = position1.Y - position2.Y;
            var totalDistance = Math.Sqrt(distanceX * distanceX + distanceY * distanceY);
            return (float)totalDistance;
        }

        public static float GetAngle(Vector2 vector1, Vector2 vector2)
        {
            var distanceX = vector1.X - vector2.X;
            var distanceY = vector1.Y - vector2.Y;
            return (float)(Math.Atan2(distanceY, distanceX) + Math.PI);
        }

        public static float DegreesToRadians(float degrees)
        {
            return (float)((degrees / 180f) * Math.PI);
        }

        public static float RadiansToDegrees(float radians)
        {
            return (float)((180f / Math.PI) * radians);
        }

        public static Vector2 RotateVectorAroundVector(Vector2 vector1, Vector2 vector2, float radians) {
            double s = Math.Sin(radians);
            double c = Math.Cos(radians);

            vector1.X -= vector2.X;
            vector1.Y -= vector2.Y;

            double new_x = vector1.X * c - vector1.Y * s;
            double new_y = vector1.X * s + vector1.Y * c;

            vector1.X = (float)new_x + vector2.X;
            vector1.Y = (float)new_y + vector2.Y;

            return vector1;
        }
    }
}
