using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

namespace monogame
{
	public class GameRoot : Game
	{
		public static GameRoot Instance { get; private set; }
		public static Viewport Viewport { get { return Instance.GraphicsDevice.Viewport; } }
		public static Vector2 ScreenSize { get { return new Vector2(Viewport.Width, Viewport.Height); } }

		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

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
		}

		protected override void LoadContent ()
		{
			spriteBatch = new SpriteBatch (GraphicsDevice);
			Rectangle.Initialize (graphics);
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

			spriteBatch.Begin (); 
			EntityManager.Draw (spriteBatch);
			base.Draw (gameTime);
			spriteBatch.End ();       
		}
	}
}
