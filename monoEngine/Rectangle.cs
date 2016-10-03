using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

namespace monogame
{
	public static class Rectangle
	{
		static bool isValid = false;
		static Texture2D Texture;

		public static void Initialize(GraphicsDeviceManager graphics){
			isValid = true;
			Texture = new Texture2D (graphics.GraphicsDevice, 1, 1);
			Texture.SetData (new Color[] { Color.White });
		}

		public static void Draw(SpriteBatch spriteBatch, Vector2 position, Vector2 scale, Color color, float layerDepth = 0f){
			if (isValid) {
				spriteBatch.Draw (Texture, position: position, scale: scale, color: color, layerDepth: layerDepth);
			}
		}

		public static void Draw(SpriteBatch spriteBatch, float x, float y, float width, float height, Color color, float layerDepth = 0f){
			if (isValid) {
				Vector2 position = new Vector2 (x, y);
				Vector2 scale = new Vector2 (width, height);
				spriteBatch.Draw (Texture, position: position, scale: scale, color: color, layerDepth: layerDepth);
			}
		}

	}
}

