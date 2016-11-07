using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Engine
{
	public class RenderCanvas : Entity
	{
		public readonly RenderTarget2D othersRenderTarget;
        public readonly Vector2 Size;
        public Color BackgroundColor = Color.Transparent;

		public RenderCanvas(int width, int height) {
            Size = new Vector2(width, height);
			this.othersRenderTarget = new RenderTarget2D (GameRoot.graphicsDevice, width, height);
			this._sprite = new Sprite(this.othersRenderTarget);
		}

	}
}

