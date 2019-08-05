using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoEngine
{
    public static class NumberDrawer
    {
        public static void Draw(SpriteBatch spriteBatch, int number, int x, int y, int height, Color color, DrawFrom drawFrom = DrawFrom.TopLeft)
        {
            // Set up initial variables
            string text = number.ToString();
            int buffer = height / 4;
            int charWidth = GetWidth(height);
            int totalWidth = charWidth * text.Length + buffer * text.Length;
            Point drawFromOffset = new Point(0, 0);

            // Calculate the DrawFrom offset X
            if (drawFrom == DrawFrom.TopRight || drawFrom == DrawFrom.BottomRight || drawFrom == DrawFrom.RightCenter)
            {
                drawFromOffset.X = -totalWidth + buffer;
            }
            else if (drawFrom == DrawFrom.TopCenter || drawFrom == DrawFrom.Center || drawFrom == DrawFrom.BottomCenter)
            {
                drawFromOffset.X = -totalWidth / 2 + buffer / 2;
            }

            // Calculate the DrawFrom offset Y
            if (drawFrom == DrawFrom.BottomLeft || drawFrom == DrawFrom.BottomCenter || drawFrom == DrawFrom.BottomRight)
            {
                drawFromOffset.Y = -height;
            }
            else if (drawFrom == DrawFrom.LeftCenter || drawFrom == DrawFrom.Center || drawFrom == DrawFrom.RightCenter)
            {
                drawFromOffset.Y = -height / 2;
            }


            // Do the drawing
            int i = 0;
            foreach (char c in text)
            {
                Rectangle[] rectangles = GetRectangles(c, height);
                if (rectangles != null)
                {
                    int charOffset = charWidth * i + buffer * i;
                    foreach (Rectangle rectangle in rectangles)
                    {
                        rectangle.Offset(x + drawFromOffset.X + charOffset, y + drawFromOffset.Y);
                        RectangleDrawer.Draw(spriteBatch, rectangle, color);
                    }
                }
                i++;
            }
        }

        public static Rectangle[] GetRectangles(char c, int height)
        {
            switch (c)
            {
                case '0':
                    return new[]
                    {
                        GetRectangle_TopLeft(height),
                        GetRectangle_TopCenter(height),
                        GetRectangle_TopRight(height),
                        GetRectangle_BottomRight(height),
                        GetRectangle_BottomCenter(height),
                        GetRectangle_BottomLeft(height)
                    };
                case '1':
                    return new[]
                    {
                        GetRectangle_TopRight(height),
                        GetRectangle_BottomRight(height)
                    };
                case '2':
                    return new[]
                    {
                        GetRectangle_TopCenter(height),
                        GetRectangle_TopRight(height),
                        GetRectangle_MidCenter(height),
                        GetRectangle_BottomLeft(height),
                        GetRectangle_BottomCenter(height)
                    };
                case '3':
                    return new[]
                    {
                        GetRectangle_TopCenter(height),
                        GetRectangle_MidCenter(height),
                        GetRectangle_BottomCenter(height),
                        GetRectangle_TopRight(height),
                        GetRectangle_BottomRight(height)
                    };
                case '4':
                    return new[]
                    {
                        GetRectangle_TopLeft(height),
                        GetRectangle_MidCenter(height),
                        GetRectangle_TopRight(height),
                        GetRectangle_BottomRight(height)
                    };
                case '5':
                    return new[]
                    {
                        GetRectangle_TopCenter(height),
                        GetRectangle_TopLeft(height),
                        GetRectangle_MidCenter(height),
                        GetRectangle_BottomRight(height),
                        GetRectangle_BottomCenter(height)
                    };
                case '6':
                    return new[]
                    {
                        GetRectangle_TopCenter(height),
                        GetRectangle_TopLeft(height),
                        GetRectangle_MidCenter(height),
                        GetRectangle_BottomRight(height),
                        GetRectangle_BottomCenter(height),
                        GetRectangle_BottomLeft(height)
                    };
                case '7':
                    return new[]
                    {
                        GetRectangle_TopCenter(height),
                        GetRectangle_TopRight(height),
                        GetRectangle_BottomRight(height)
                    };
                case '8':
                    return new[]
                    {

                        GetRectangle_TopCenter(height),
                        GetRectangle_TopLeft(height),
                        GetRectangle_TopRight(height),
                        GetRectangle_MidCenter(height),
                        GetRectangle_BottomLeft(height),
                        GetRectangle_BottomCenter(height),
                        GetRectangle_BottomRight(height)
                    };
                case '9':
                    return new[]
                    {

                        GetRectangle_TopCenter(height),
                        GetRectangle_TopLeft(height),
                        GetRectangle_TopRight(height),
                        GetRectangle_MidCenter(height),
                        GetRectangle_BottomCenter(height),
                        GetRectangle_BottomRight(height)
                    };
                default:
                    return null;
            }
        }

        private static Rectangle GetRectangle_TopCenter(int height)
        {
            return new Rectangle(0, 0, GetWidth(height), GetThickness(height));
        }

        private static Rectangle GetRectangle_MidCenter(int height)
        {
            int thickness = GetThickness(height);
            return new Rectangle(0, height / 2 - thickness / 2, GetWidth(height), thickness);
        }

        private static Rectangle GetRectangle_BottomCenter(int height)
        {
            int thickness = GetThickness(height);
            return new Rectangle(0, height - thickness, GetWidth(height), thickness);
        }

        private static Rectangle GetRectangle_TopLeft(int height)
        {
            return new Rectangle(0, 0, GetThickness(height), height / 2);
        }

        private static Rectangle GetRectangle_TopRight(int height)
        {
            int thickness = GetThickness(height);
            return new Rectangle(GetWidth(height) - thickness, 0, thickness, height / 2);
        }

        private static Rectangle GetRectangle_BottomLeft(int height)
        {
            Rectangle rect = GetRectangle_TopLeft(height);
            rect.Y += height / 2;
            return rect;
        }

        private static Rectangle GetRectangle_BottomRight(int height)
        {
            Rectangle rect = GetRectangle_TopRight(height);
            rect.Y += height / 2;
            return rect;
        }

        private static int GetWidth(int height)
        {
            return height / 2;
        }

        private static int GetThickness(int height)
        {
            return GetWidth(height) / 5;
        }
    }
}
