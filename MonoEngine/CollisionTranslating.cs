using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngine
{
    public static class CollisionTranslating
    {
        public static Vector2 RectRect(Rectangle rectangle1, Rectangle rectangle2)
        {
            Vector2 translationAmount = new Vector2(0, 0);

            Dictionary<string, int> distances = new Dictionary<string, int>()
            {
                {"left", 0},
                {"right", 0},
                {"top", 0},
                {"bottom", 0}
            };

            distances["left"] = Math.Abs(rectangle1.Right - rectangle2.Left);
            distances["right"] = Math.Abs(rectangle1.Left - rectangle2.Right);
            distances["top"] = Math.Abs(rectangle1.Bottom - rectangle2.Top);
            distances["bottom"] = Math.Abs(rectangle1.Top - rectangle2.Bottom);

            var smallestDistance = Utilities.GetSmallestInDictionary(distances);

            if (smallestDistance == "left")
                translationAmount.X = rectangle2.Left - rectangle1.Right;
            else if (smallestDistance == "right")
                translationAmount.X = rectangle2.Right - rectangle1.Left;
            else if (smallestDistance == "top")
                translationAmount.Y = rectangle2.Top - rectangle1.Bottom;
            else if (smallestDistance == "bottom")
                translationAmount.Y = rectangle2.Bottom - rectangle1.Top;

            return translationAmount;
        }
    }
}
