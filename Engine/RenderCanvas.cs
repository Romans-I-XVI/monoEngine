using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Engine
{
	public class RenderCanvas : Entity
	{
        public RenderTarget2D othersRenderTarget { get; protected set; }
        public Vector2 Size { get; protected set; }
        public Color BackgroundColor = Color.Transparent;
        public bool ShouldClear = true;

		public RenderCanvas(int width, int height) {
            Size = new Vector2(width, height);
            this.othersRenderTarget = new RenderTarget2D(GameRoot.graphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
			var sprite = new Sprite(this.othersRenderTarget);
            AddSprite(sprite);
		}

        ~RenderCanvas()
        {
            if (othersRenderTarget != null)
                othersRenderTarget.Dispose();
        }

        public override void onDestroy()
        {
            if (othersRenderTarget != null)
                othersRenderTarget.Dispose();
            base.onDestroy();
        }

	}
}

