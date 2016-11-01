using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.InputListeners;

namespace Engine
{
	static class EntityManager
	{
		static List<Entity> _entities = new List<Entity>();

        public static void ChangeRoom(Room previous_room, Room next_room)
        {
            foreach (var entity in _entities.ToList())
                entity.onChangeRoom(previous_room, next_room);
            Clear();
        }


        public static void Add(Entity entity)
        {
            entity.IsExpired = false;
			_entities.Add (entity);
			entity.onCreate();
		}

		public static void Clear() 
        {
            _entities = _entities.Where(x => x.IsPersistent).ToList();
		}

		public static int Count<T>() where T : Entity
        {
			return _entities.OfType<T>().Count();
		}

		public static void Update(GameTime gameTime)
        {
			MouseState mouseState = Mouse.GetState();
			KeyboardState keyboardState = Keyboard.GetState();
			Dictionary<PlayerIndex, GamePadState> gamepadStates = new Dictionary<PlayerIndex, GamePadState>() 
            {
				{PlayerIndex.One, GamePad.GetState(PlayerIndex.One)},
				{PlayerIndex.Two, GamePad.GetState(PlayerIndex.Two)},
				{PlayerIndex.Three, GamePad.GetState(PlayerIndex.Three)},
				{PlayerIndex.Four, GamePad.GetState(PlayerIndex.Four)}
			};
			foreach (var entity in _entities.ToList()) 
            {
				entity.onMouse(mouseState);
				entity.onKey(keyboardState);
				entity.onButton(gamepadStates);
				entity.onUpdate (gameTime);
				if (entity.IsExpired)
					entity.onDestroy();
			}

			_entities = _entities.Where(x => !x.IsExpired).ToList();
		}

		public static void DrawToRenderTargets (SpriteBatch spriteBatch)
        {
			foreach (var renderCanvas in _entities.OfType<RenderCanvas>()) 
            {
				GameRoot.graphicsDevice.SetRenderTarget (renderCanvas.othersRenderTarget);
                GameRoot.graphicsDevice.Clear(Color.Transparent);
                spriteBatch.Begin (SpriteSortMode.BackToFront);
				foreach (var entity in _entities) 
                {
					if (entity.renderTarget == renderCanvas.othersRenderTarget) 
                    {
						entity.onDrawBegin (spriteBatch);
						entity.onDraw (spriteBatch);
						entity.onDrawEnd (spriteBatch);
					}
				}
				spriteBatch.End ();
				GameRoot.graphicsDevice.SetRenderTarget (null);
			}
						
		}

		public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront);
			foreach (var entity in _entities) 
            {
				if (entity.renderTarget == null) 
                {
					entity.onDrawBegin (spriteBatch);
					entity.onDraw (spriteBatch);
					entity.onDrawEnd (spriteBatch);
				}
			}
			spriteBatch.End ();
		}

		public static class MouseEvents
		{
			public static void onMouseDown(object sender, MouseEventArgs e)
			{
				foreach (var entity in _entities.ToList())
					entity.onMouseDown(e);
			}

			public static void onMouseUp(object sender, MouseEventArgs e)
			{
				foreach (var entity in _entities.ToList())
					entity.onMouseUp(e);
			}

			public static void onMouseWheel(object sender, MouseEventArgs e)
			{
				foreach (var entity in _entities.ToList())
					entity.onMouseWheel(e);
			}

		}

		public static class KeyboardEvents
		{

			public static void onKeyPressed(object sender, KeyboardEventArgs e)
			{
				foreach (var entity in _entities.ToList())
					entity.onKeyDown(e);
			}

			public static void onKeyReleased(object sender, KeyboardEventArgs e)
			{
				foreach (var entity in _entities.ToList())
					entity.onKeyUp(e);
			}
			
		}

		public static class GamepadEvents
		{
			public static void onButtonDown(object sender, GamePadEventArgs e)
			{
				foreach (var entity in _entities.ToList())
					entity.onButtonDown(e);
			}

			public static void onButtonUp(object sender, GamePadEventArgs e)
			{
				foreach (var entity in _entities.ToList())
					entity.onButtonUp(e);
			}
		}

	}
}

