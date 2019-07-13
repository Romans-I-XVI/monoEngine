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
            Vector2 translation_amount = new Vector2(0, 0);

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

            var smallest_distance = Utilities.GetSmallestInDictionary(distances);

            if (smallest_distance == "left")
                translation_amount.X = rectangle2.Left - rectangle1.Right;
            else if (smallest_distance == "right")
                translation_amount.X = rectangle2.Right - rectangle1.Left;
            else if (smallest_distance == "top")
                translation_amount.Y = rectangle2.Top - rectangle1.Bottom;
            else if (smallest_distance == "bottom")
                translation_amount.Y = rectangle2.Bottom - rectangle1.Top;

            return translation_amount;
        }
    }
}
