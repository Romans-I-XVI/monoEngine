using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.InputListeners;
using MonoGame.Extended.ViewportAdapters;

namespace Engine
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
			var mouseListener = new MouseListener(new MouseListenerSettings());
			var keyboardListener = new KeyboardListener(new KeyboardListenerSettings());
			var gamepadListenerOne = new GamePadListener(new GamePadListenerSettings(PlayerIndex.One));
			var gamepadListenerTwo = new GamePadListener(new GamePadListenerSettings(PlayerIndex.Two));
			var gamepadListenerThree = new GamePadListener(new GamePadListenerSettings(PlayerIndex.Three));
			var gamepadListenerFour = new GamePadListener(new GamePadListenerSettings(PlayerIndex.Four));
			Components.Add(new InputListenerComponent(this, mouseListener, keyboardListener, gamepadListenerOne, gamepadListenerTwo, gamepadListenerThree, gamepadListenerFour));

			mouseListener.MouseDown += EntityManager.MouseEvents.onMouseDown;
			mouseListener.MouseUp += EntityManager.MouseEvents.onMouseUp;
			mouseListener.MouseWheelMoved += EntityManager.MouseEvents.onMouseWheel;
			keyboardListener.KeyPressed += EntityManager.KeyboardEvents.onKeyPressed;
			keyboardListener.KeyReleased += EntityManager.KeyboardEvents.onKeyReleased;
			gamepadListenerOne.ButtonDown += EntityManager.GamepadEvents.onButtonDown;
			gamepadListenerTwo.ButtonDown += EntityManager.GamepadEvents.onButtonDown;
			gamepadListenerThree.ButtonDown += EntityManager.GamepadEvents.onButtonDown;
			gamepadListenerFour.ButtonDown += EntityManager.GamepadEvents.onButtonDown;


			new MonoGameTiles.Room_Main ();
			new MonoGameTiles.Room_Play ();
			RoomManager.ChangeRoom ("Room_Main");
			base.Initialize ();
			RectangleDrawer.Initialize ();
		}
			

		protected override void LoadContent ()
		{
			spriteBatch = new SpriteBatch (GraphicsDevice);
		}
		protected override void Update (GameTime gameTime)
		{
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
