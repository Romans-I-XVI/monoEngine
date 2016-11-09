using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Engine
{
	public class Sprite
	{
		public Texture2D Texture { get; private set; }
		public float Orientation = 0f;
		public Vector2 Origin = new Vector2();
		public Vector2 Scale = new Vector2(1.0f, 1.0f);
		public float Depth = 0.5f;
        public float Alpha = 255f;
		public Color Color = Color.White;


		public Sprite (Texture2D texture)
		{
			Texture = texture;
		}

		public void Draw (SpriteBatch spriteBatch, Vector2 position)
		{
            spriteBatch.Draw (Texture, position: position, origin: Origin, rotation: Orientation, scale: Scale, color: Color * (Alpha/255f), layerDepth: Depth);
		}


			
	}
}

