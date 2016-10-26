using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.InputListeners;

namespace Engine
{
	abstract class Entity
	{
		protected Image Image;
		public RenderTarget2D renderTarget = null;
		public Vector2 Position = new Vector2();
		public bool IsExpired = false;

		public Entity(){
			EntityManager.Add (this);
		}

		public void SetRenderCanvas (RenderCanvas renderCanvas){
			this.renderTarget = renderCanvas.othersRenderTarget;
		}

		public virtual void onMouse(MouseState state) {}

		public virtual void onMouseDown(MouseEventArgs e) {}

		public virtual void onMouseUp(MouseEventArgs e) {}

		public virtual void onMouseWheel(MouseEventArgs e) {}

		public virtual void onKey(KeyboardState state) {}

		public virtual void onKeyDown(KeyboardEventArgs e) {}

		public virtual void onKeyUp(KeyboardEventArgs e) {}

		public virtual void onButton(Dictionary<PlayerIndex, GamePadState> states) {}

		public virtual void onButtonDown(GamePadEventArgs e) {}

		public virtual void onButtonUp(GamePadEventArgs e) {}

		public virtual void onUpdate (GameTime gameTime) {}

		public virtual void onDrawBegin (SpriteBatch spriteBatch) {}

		public virtual void onDrawEnd (SpriteBatch spriteBatch) {}

		public virtual void onDraw(SpriteBatch spriteBatch)
		{
			if (Image != null) {
				Image.Draw (spriteBatch, Position);
			}
		}
	}
}

