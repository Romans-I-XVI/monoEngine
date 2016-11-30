using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGameTiles;

using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.InputListeners;
using MonoGame.Extended.ViewportAdapters;
using Newtonsoft.Json;

namespace Engine
{
	public class GameRoot : Game
	{
		static GraphicsDeviceManager graphics;
		static SpriteBatch spriteBatch;

		public static GameRoot Instance { get; private set; }
        public static Vector2 VirtualSize { get { return new Vector2(1280, 720); } }
		public static GraphicsDevice graphicsDevice { get { return graphics.GraphicsDevice; } }
        public static BoxingViewportAdapter BoxingViewport;
        public static bool ExitGame = false;

		public GameRoot ()
		{
			Instance = this;
			graphics = new GraphicsDeviceManager (this);
            graphics.IsFullScreen = true;
            graphics.HardwareModeSwitch = false;
            Window.AllowAltF4 = true;
            Window.AllowUserResizing = true;

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
			Content.RootDirectory = "Content";
		}
		protected override void Initialize ()
		{
            GameRoot.BoxingViewport = new BoxingViewportAdapter(Window, GraphicsDevice, (int)VirtualSize.X, (int)VirtualSize.Y, 0, 0);
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
            gamepadListenerOne.ButtonUp += EntityManager.GamepadEvents.onButtonUp;
            gamepadListenerTwo.ButtonUp += EntityManager.GamepadEvents.onButtonUp;
            gamepadListenerThree.ButtonUp += EntityManager.GamepadEvents.onButtonUp;
            gamepadListenerFour.ButtonUp += EntityManager.GamepadEvents.onButtonUp;

            HttpUtilities.GetUserData();
            RectangleDrawer.Initialize();
			base.Initialize ();
		}
			

		protected override void LoadContent ()
		{
			spriteBatch = new SpriteBatch (GraphicsDevice);
            ContentHolder.Init(this);
            Room r;
            r = new Room_Main();
            r.Initialize();
            r = new Room_Play();
            r.Initialize();
            r = new Room_OnlineLevels();
            r.Initialize();
            r = new Room_LocalLevels();
            r.Initialize();
            r = new Room_EditorLevels();
            r.Initialize();
            r = new Room_Editor();
            r.Initialize();
            r = new Room_PublishLevel();
            r.Initialize();
            new Selector();
            MediaPlayer.Play(ContentHolder.Get(AvailableMusic.chill));
            MediaPlayer.IsRepeating = true;
            RoomManager.ChangeRoom<Room_Main>();
		}
		protected override void Update (GameTime gameTime)
		{
            base.Update(gameTime);
			EntityManager.Update(gameTime);
            if (ExitGame)
                Exit();
		}

		protected override void Draw (GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear (Color.Black);

			EntityManager.DrawToRenderTargets (spriteBatch);
			EntityManager.Draw (spriteBatch);
		}
	}
}
