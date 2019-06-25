using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Engine
{
	public abstract class Entity
	{
        protected readonly Dictionary<string, Sprite> _sprites = new Dictionary<string, Sprite>();
        protected readonly List<Collider> _colliders = new List<Collider>();
        protected Room Room { get { return RoomManager.CurrentRoom;} }
        public List<Collider> Colliders { get { return _colliders; } }
        public RenderCanvas renderTarget = null;
        private Vector2 _relativePosition = new Vector2();
        public Vector2 Position
        {
            get { return _relativePosition + Origin; }
            set { _relativePosition = value; }
        }
        public virtual Vector2 Origin => Vector2.Zero;
        public Vector2 Speed = new Vector2();
        public bool IsExpired = false;
        public bool IsPersistent = false;
        public bool IsPauseable = true;
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

        public Sprite AddSprite(string name, Sprite sprite)
        {
            _sprites.Add(name, sprite);
            return sprite;
        }

        public void RemoveSprite(string name)
        {
            _sprites.Remove(name);
        }

        public Sprite GetSprite(string name)
        {
            if (_sprites.ContainsKey(name))
                return _sprites[name];
            else
                return null;
        }

        public ColliderCircle AddColliderCircle(string collider_name, float radius, int offset_x = 0, int offset_y = 0, bool enabled = true)
        {
            RemoveCollider(collider_name);
            var collider = new ColliderCircle(this, collider_name, radius, offset_x, offset_y, enabled);
            _colliders.Add(collider);
            return collider;
        }

        public ColliderRectangle AddColliderRectangle(string collider_name, int offset_x, int offset_y, int width, int height, bool enabled = true)
        {
            RemoveCollider(collider_name);
            var collider = new ColliderRectangle(this, collider_name, offset_x, offset_y, width, height, enabled);
            _colliders.Add(collider);
            return collider;
        }

        public Collider GetCollider(string collider_name)
        {
            foreach (var item in _colliders)
            {
                if (item.Name == collider_name)
                    return item;
            }
            return null;
        }

        public void RemoveCollider(string collider_name)
        {
            for (int i = _colliders.Count - 1; i >= 0; i--)
            {
                if (_colliders[i].Name == collider_name)
                {
                    _colliders[i].Enabled = false;
                    _colliders.RemoveAt(i);
                }
            }
        }

        //Event override methods

        public virtual void onCreate() {}

		public virtual void onDestroy() {}

        public virtual void onPause() {}

        public virtual void onResume(int pause_time)
        {
            if (IsPauseable)
            {
                foreach (Sprite sprite in _sprites.Values)
                {
                    if (sprite is AnimatedSprite && this.IsPauseable)
                    {
                        ((AnimatedSprite)sprite).Timer.RemoveTime(pause_time);
                    }
                }
            }
        }

		public virtual void onChangeRoom(Room previous_room, Room next_room) {}

		public virtual void onMouse(MouseState state) {}

        public virtual void onMouseDown(MouseEventArgs e) {}

        public virtual void onMouseUp(MouseEventArgs e) {}

		public virtual void onKey(KeyboardState state) {}

		public virtual void onKeyDown(KeyboardEventArgs e) {}

		public virtual void onKeyUp(KeyboardEventArgs e) {}

		public virtual void onButton(Dictionary<PlayerIndex, GamePadState> states) {}

		public virtual void onButtonDown(GamePadEventArgs e) {}

		public virtual void onButtonUp(GamePadEventArgs e) {}

		public virtual void onUpdate (GameTime gameTime)
        {
            Position += Speed * 60 * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public virtual void onCollision(Collider collider, Collider other_collider, Entity other_instance) {}

        public virtual void onDraw(SpriteBatch spriteBatch)
		{
            foreach (var sprite in _sprites.Values)
            {
                sprite.Draw(spriteBatch, Position);
            }
        }

        public virtual void onGameEvent(GameEvent game_event) {}
	}
}

