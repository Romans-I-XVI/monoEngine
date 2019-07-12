using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Retaliate;

namespace MonoEngine
{
    public class EngineGame : Game
    {
        public Color BackgroundColor = Color.Black;
        public ViewportAdapter Viewport;
        public SpriteBatch SpriteBatch { get; private set; }
        public GraphicsDeviceManager Graphics { get; private set; }
        public readonly int CanvasWidth;
        public readonly int CanvasHeight;
        public readonly int HorizontalBleed;
        public readonly int VerticalBleed;
        public bool ExitGame = false;

        public EngineGame(int canvasWidth, int canvasHeight, int horizontalBleed, int verticalBleed)
        {
            CanvasWidth = canvasWidth;
            CanvasHeight = canvasHeight;
            HorizontalBleed = horizontalBleed;
            VerticalBleed = verticalBleed;
            Graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = canvasWidth,
                PreferredBackBufferHeight = canvasHeight
            };
        }

        protected override void Initialize()
        {
            RectangleDrawer.Initialize();
            Viewport = new BoxingViewportAdapter(Window, GraphicsDevice, CanvasWidth, CanvasHeight, HorizontalBleed, VerticalBleed);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            Engine.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Engine.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
