using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

namespace monogame
{
	class RenderCanvas : Entity
	{
		public RenderTarget2D othersRenderTarget;

		public RenderCanvas(int width, int height) {
			this.othersRenderTarget = new RenderTarget2D (GameRoot.graphicsDevice, width, height);
			this.Image = new Image(this.othersRenderTarget);
		}

	}
}

