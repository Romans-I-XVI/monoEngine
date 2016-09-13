﻿using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

namespace monogame
{
	abstract class Entity
	{
		protected Image Image;
		public Vector2 Position = new Vector2();
		public bool IsExpired = false;

		public Entity(){
			EntityManager.Add (this);
		}

		public virtual void Update (GameTime gameTime) {}

		public virtual void DrawBegin (SpriteBatch spriteBatch) {}

		public virtual void DrawEnd (SpriteBatch spriteBatch) {}

		public virtual void Draw(SpriteBatch spriteBatch)
		{
			if (Image != null) {
				Image.Draw (spriteBatch, Position);
			}
		}
	}
}
