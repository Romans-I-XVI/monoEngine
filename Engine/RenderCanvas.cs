using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Engine
{
	class RenderCanvas : Entity
	{
		public readonly RenderTarget2D othersRenderTarget;
        public readonly Vector2 Size;

		public RenderCanvas(int width, int height) {
            Size = new Vector2(width, height);
			this.othersRenderTarget = new RenderTarget2D (GameRoot.graphicsDevice, width, height);
			this.Sprite = new Sprite(this.othersRenderTarget);
		}

	}
}

