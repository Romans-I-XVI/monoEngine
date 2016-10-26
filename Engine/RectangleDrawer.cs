using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Engine
{
	public static class RectangleDrawer
	{
		static bool isValid = false;
		static Texture2D RectangleTexture;

		public static void Initialize(){
			isValid = true;
			RectangleTexture = new Texture2D (GameRoot.graphicsDevice, 1, 1);
			RectangleTexture.SetData (new Color[] { Color.White });
		}

		public static Texture2D GetTexture(){
			return RectangleTexture;
		}

		public static void Draw(SpriteBatch spriteBatch, Vector2 position, Vector2 scale, Color color, Vector2 origin = default(Vector2), float layerDepth = 0f){
			if (isValid) {
				spriteBatch.Draw (RectangleTexture, position: position, scale: scale, color: color, layerDepth: layerDepth, origin: origin);
			}
		}

		public static void Draw(SpriteBatch spriteBatch, float x, float y, float width, float height, Color color, float origin_x = 0f, float origin_y = 0f, float layerDepth = 0f){
			if (isValid) {
				Vector2 position = new Vector2 (x, y);
				Vector2 scale = new Vector2 (width, height);
				Vector2 origin = new Vector2 (origin_x, origin_y);
				spriteBatch.Draw (RectangleTexture, position: position, scale: scale, color: color, layerDepth: layerDepth, origin: origin);
			}
		}

        public static void DrawAround(SpriteBatch spriteBatch, float x, float y, float width, float height, Color color, float border, float layerDepth = 0f)
        {
            Draw(spriteBatch, x - border, y - border, width + border * 2, border, color, layerDepth: layerDepth);
            Draw(spriteBatch, x - border, y - border, border, height + border * 2, color, layerDepth: layerDepth);
            Draw(spriteBatch, x - border, y + height, width + border * 2, border, color, layerDepth: layerDepth);
            Draw(spriteBatch, x + width, y - border, border, height + border * 2, color, layerDepth: layerDepth);
        }

        public static void DrawAround(SpriteBatch spriteBatch, Vector2 position, Vector2 size, Color color, float border, float layerDepth = 0f)
        {
            DrawAround(spriteBatch, position.X, position.Y, size.X, size.Y, color, border, layerDepth);
        }

        public static void DrawAround(SpriteBatch spriteBatch, Rectangle rectangle, Color color, float border, float layerDepth = 0f)
        {
            DrawAround(spriteBatch, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, color, border, layerDepth);
        }


	}
}

