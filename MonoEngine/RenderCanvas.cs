using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoEngine
{
	public class RenderCanvas : Entity
	{
        public RenderTarget2D OthersRenderTarget { get; protected set; }
        public Vector2 Size { get; protected set; }
        public Color BackgroundColor = Color.Transparent;
        public bool ShouldClear = true;
        public BlendState BlendState = null;
        public SamplerState SamplerState = null;
        public DepthStencilState DepthStencilState = null;
        public RasterizerState RasterizerState = null;
        public Effect Effect = null;
        public Matrix? TransformMatrix = null;

		public RenderCanvas(int width, int height, SurfaceFormat surfaceFormat = SurfaceFormat.Color, DepthFormat depthFormat = DepthFormat.None, int preferredMultiSampleCount = 0, RenderTargetUsage renderTargetUsage = RenderTargetUsage.PreserveContents) {
            Size = new Vector2(width, height);
            OthersRenderTarget = new RenderTarget2D(Engine.Game.GraphicsDevice, width, height, false, surfaceFormat, depthFormat, preferredMultiSampleCount, renderTargetUsage);
			var sprite = new Sprite(new Region(OthersRenderTarget, 0, 0, OthersRenderTarget.Width, OthersRenderTarget.Height, 0, 0));
            AddSprite("main", sprite);
		}

        ~RenderCanvas()
        {
            if (OthersRenderTarget != null)
                OthersRenderTarget.Dispose();
        }

        public override void onDestroy()
        {
            if (OthersRenderTarget != null)
                OthersRenderTarget.Dispose();
            base.onDestroy();
        }

	}
}

