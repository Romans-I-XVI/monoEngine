using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoEngine
{
    public class Region
    {
        public Texture2D Texture;
        public Rectangle SourceRectangle;
        public Vector2 Origin;

        public Region(Texture2D texture, Rectangle sourceRectangle, Vector2 origin)
        {
            Texture = texture;
            SourceRectangle = sourceRectangle;
            Origin = origin;
        }

        public Region(Texture2D texture, int x, int y, int width, int height, int originX, int originY) : this(texture, new Rectangle(x, y, width, height), new Vector2(originX, originY))
        {
        }

        public Region(Texture2D texture) : this(texture, new Rectangle(0, 0, texture.Width, texture.Height), new Vector2(0, 0))
        {
        }

        public void AutoOrigin(DrawFrom drawFrom)
        {
            int width = SourceRectangle.Width;
            int height = SourceRectangle.Height;

            switch (drawFrom)
            {
                case DrawFrom.TopCenter:
                    Origin.X = width / 2;
                    break;
                case DrawFrom.TopRight:
                    Origin.X = width;
                    break;
                case DrawFrom.BottomLeft:
                    Origin.Y = height;
                    break;
                case DrawFrom.BottomCenter:
                    Origin.X = width / 2;
                    Origin.Y = height;
                    break;
                case DrawFrom.BottomRight:
                    Origin.X = width;
                    Origin.Y = height;
                    break;
                case DrawFrom.Center:
                    Origin.X = width / 2;
                    Origin.Y = height / 2;
                    break;
                case DrawFrom.RightCenter:
                    Origin.X = width;
                    Origin.Y = height / 2;
                    break;
                case DrawFrom.LeftCenter:
                    Origin.Y = height / 2;
                    break;
            }
        }

        public int GetWidth()
        {
            return this.SourceRectangle.Width;
        }

        public int GetHeight()
        {
            return this.SourceRectangle.Height;
        }

        public Region Copy()
        {
            return new Region(Texture, SourceRectangle, Origin);
        }
    }
}
