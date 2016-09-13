﻿using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

namespace monogame
{
	public class Image
	{
		public Texture2D Texture { get; private set; }

		public float Orientation = 0f;
		public Vector2 Origin = new Vector2();
		public Vector2 Scale = new Vector2(1.0f, 1.0f);
		public float Depth = 1f;
		public Color Color = Color.White;


		public Image (Texture2D texture)
		{
			this.Texture = texture;
		}

		public void Draw (SpriteBatch spriteBatch, Vector2 position)
		{
			spriteBatch.Draw (Texture, position: position, origin: Origin, rotation: Orientation, scale: Scale, color: Color, layerDepth: Depth);
		}


			
	}
}
