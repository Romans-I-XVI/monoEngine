using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoEngine
{
    public class EngineGame : Game
    {
        public Color BackgroundColor = Color.Black;
        public ViewportAdapter Viewport;
        public SpriteBatch SpriteBatch { get; private set; }
        public GraphicsDeviceManager Graphics { get; private set; }
        public int CanvasWidth { get; protected set; }
        public int CanvasHeight { get; protected set; }
        public int HorizontalBleed { get; protected set; }
        public int VerticalBleed { get; protected set; }

        public EngineGame(int canvasWidth, int canvasHeight, int horizontalBleed, int verticalBleed)
        {
            Engine.Game = this;
            Content.RootDirectory = "Content";
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
            RectangleDrawer.Initialize(GraphicsDevice);
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
