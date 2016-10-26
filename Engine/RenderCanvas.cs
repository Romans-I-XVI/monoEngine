using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Engine
{
	class RenderCanvas : Entity
	{
		public RenderTarget2D othersRenderTarget;

		public RenderCanvas(int width, int height) {
			this.othersRenderTarget = new RenderTarget2D (GameRoot.graphicsDevice, width, height);
			this.Sprite = new Sprite(this.othersRenderTarget);
		}

	}
}

