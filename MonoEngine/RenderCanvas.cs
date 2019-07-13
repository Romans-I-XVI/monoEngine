using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoEngine
{
	public class RenderCanvas : Entity
	{
        public RenderTarget2D othersRenderTarget { get; protected set; }
        public Vector2 Size { get; protected set; }
        public Color BackgroundColor = Color.Transparent;
        public bool ShouldClear = true;

		public RenderCanvas(int width, int height) {
            Size = new Vector2(width, height);
            othersRenderTarget = new RenderTarget2D(Engine.Game.GraphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
			var sprite = new Sprite(new Region(othersRenderTarget, 0, 0, othersRenderTarget.Width, othersRenderTarget.Height, 0, 0));
            AddSprite("main", sprite);
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

