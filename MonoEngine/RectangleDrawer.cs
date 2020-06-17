using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoEngine
{
	public static class RectangleDrawer
	{
		static bool _initialized = false;
		static Texture2D _rectangleTexture;

		public static void Initialize(GraphicsDevice graphicsDevice){
			_initialized = true;
			_rectangleTexture = new Texture2D (graphicsDevice, 1, 1);
			_rectangleTexture.SetData (new Color[] { Color.White });
		}

		public static Texture2D GetTexture(){
			return _rectangleTexture;
		}

		public static void Draw(SpriteBatch spriteBatch, Rectangle rectangle, Color color, Vector2 origin = default(Vector2), float layerDepth = 0f){
			if (_initialized) {
                spriteBatch.Draw (_rectangleTexture, new Vector2(rectangle.X - origin.X, rectangle.Y - origin.Y), null, color, 0, Vector2.Zero, new Vector2((float)rectangle.Width, (float)rectangle.Height), SpriteEffects.None, layerDepth);
			}
		}

		public static void Draw(SpriteBatch spriteBatch, float x, float y, float width, float height, Color color, float origin_x = 0f, float origin_y = 0f, float layerDepth = 0f){
			if (_initialized) {
				spriteBatch.Draw (_rectangleTexture, new Vector2(x - origin_x, y - origin_y), null, color, 0, Vector2.Zero, new Vector2(width, height), SpriteEffects.None, layerDepth);
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

