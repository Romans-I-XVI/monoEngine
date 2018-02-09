using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Engine
{
    public enum DrawFrom
    {
        TopLeft,
        TopCenter,
        TopRight,
        BottomLeft,
        BottomCenter,
        BottomRight,
        Center,
        RightCenter,
        LeftCenter,
    }
	public class Sprite
	{
		public Texture2D Texture { get; private set; }
		public float Orientation = 0f;
		public Vector2 Origin = new Vector2();
        public Vector2 Offset = new Vector2();
		public Vector2 Scale = new Vector2(1.0f, 1.0f);
		public float Depth = 0.5f;
        public float Alpha = 255f;
		public Color Color = Color.White;

		public Sprite (Texture2D texture)
		{
			Texture = texture;
		}

		public virtual void Draw (SpriteBatch spriteBatch, Vector2 position)
		{
            spriteBatch.Draw (Texture, position: position + Offset, origin: Origin, rotation: Orientation, scale: Scale, color: Color * (Alpha/255f), layerDepth: Depth);
		}

        public void AutoOrigin(DrawFrom draw_from)
        {
            switch (draw_from)
            {
                case DrawFrom.TopCenter:
                    Origin.X = Texture.Width / 2;
                    break;
                case DrawFrom.TopRight:
                    Origin.X = Texture.Width;
                    break;
                case DrawFrom.BottomLeft:
                    Origin.Y = Texture.Height;
                    break;
                case DrawFrom.BottomCenter:
                    Origin.X = Texture.Width / 2;
                    Origin.Y = Texture.Height;
                    break;
                case DrawFrom.BottomRight:
                    Origin.X = Texture.Width;
                    Origin.Y = Texture.Height;
                    break;
                case DrawFrom.Center:
                    Origin.X = Texture.Width / 2;
                    Origin.Y = Texture.Height / 2;
                    break;
                case DrawFrom.RightCenter:
                    Origin.X = Texture.Width;
                    Origin.Y = Texture.Height / 2;
                    break;
                case DrawFrom.LeftCenter:
                    Origin.Y = Texture.Height / 2;
                    break;
            }
        }
	}
}

