using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

namespace monogame
{
	public class GameRoot : Game
	{
		static GraphicsDeviceManager graphics;
		static SpriteBatch spriteBatch;

		public static GameRoot Instance { get; private set; }
		public static Viewport Viewport { get { return GameRoot.graphicsDevice.Viewport; } }
		public static Vector2 ScreenSize { get { return new Vector2(Viewport.Width, Viewport.Height); } }
		public static GraphicsDevice graphicsDevice { get { return graphics.GraphicsDevice; } }

		public GameRoot ()
		{
			Instance = this;
			graphics = new GraphicsDeviceManager (this);
			Content.RootDirectory = "Content";
		}
		protected override void Initialize ()
		{
			new Room_Main ();
			new Room_Play ();
			RoomManager.ChangeRoom ("Room_Main");
			base.Initialize ();
			Rectangle.Initialize ();
		}

		protected override void LoadContent ()
		{
			spriteBatch = new SpriteBatch (GraphicsDevice);
		}
		protected override void Update (GameTime gameTime)
		{
			
			InputHandler.Update();
			EntityManager.Update(gameTime);
			base.Update (gameTime);

		}

		protected override void Draw (GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear (Color.Black);

			EntityManager.DrawToRenderTargets (spriteBatch);
			EntityManager.Draw (spriteBatch);
		}
	}
}
