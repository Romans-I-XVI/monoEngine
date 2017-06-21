using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input.InputListeners;

namespace Engine
{
	public abstract class Entity
	{
		protected Sprite _sprite;
		protected Room Room { get { return RoomManager.CurrentRoom;} }
        public Sprite Sprite { get { return _sprite; } }
        public RenderCanvas renderTarget = null;
		public Vector2 Position = new Vector2();
		public bool IsExpired = false;
        public bool IsPersistent = false;
        public bool ShouldDraw = true;
        
		protected Entity ()
		{
			EntityManager.Add (this);
		}

		public void SetRenderCanvas (RenderCanvas renderCanvas) 
		{
			renderTarget = renderCanvas;
		}

		public void Destroy()
		{
			this.IsExpired = true;
		}
		//Event override methods

		public virtual void onCreate() {}

		public virtual void onDestroy() {}

		public virtual void onChangeRoom(Room previous_room, Room next_room) {}

		public virtual void onMouse(MouseState state) {}

		public virtual void onMouseDown(MouseEventArgs e) {}

		public virtual void onMouseUp(MouseEventArgs e) {}

		public virtual void onMouseWheel(MouseEventArgs e) {}

		public virtual void onKey(KeyboardState state) {}

		public virtual void onKeyDown(KeyboardEventArgs e) {}

		public virtual void onKeyUp(KeyboardEventArgs e) {}

		public virtual void onButton(Dictionary<PlayerIndex, GamePadState> states) {}

		public virtual void onButtonDown(GamePadEventArgs e) {}

		public virtual void onButtonUp(GamePadEventArgs e) {}

		public virtual void onUpdate (GameTime gameTime) {}

		public virtual void onDrawBegin (SpriteBatch spriteBatch) {}

		public virtual void onDrawEnd (SpriteBatch spriteBatch) {}

		public virtual void onDraw(SpriteBatch spriteBatch)
		{
			if (_sprite != null)
				_sprite.Draw (spriteBatch, Position);
		}
	}
}

