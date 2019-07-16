using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoEngine
{
	public abstract class Entity
	{
        public readonly Dictionary<string, Sprite> Sprites = new Dictionary<string, Sprite>();
        public readonly List<Collider> Colliders = new List<Collider>();
        public RenderCanvas RenderTarget = null;
        public Vector2 Position = new Vector2();
        public Vector2 Speed = new Vector2();
        public bool IsExpired = false;
        public bool IsPersistent = false;
        public bool IsPauseable = true;
        public bool ShouldDraw = true;
        public int ID;
        public int Depth = 0;

        protected Entity ()
		{
		}

		public void SetRenderCanvas (RenderCanvas renderCanvas)
		{
			RenderTarget = renderCanvas;
		}

		public void Destroy()
		{
			this.IsExpired = true;
		}

        public Sprite AddSprite(string name, Sprite sprite)
        {
            Sprites.Add(name, sprite);
            return sprite;
        }

        public void RemoveSprite(string name)
        {
            Sprites.Remove(name);
        }

        public Sprite GetSprite(string name)
        {
            if (Sprites.ContainsKey(name))
                return Sprites[name];
            else
                return null;
        }

        public ColliderCircle AddColliderCircle(string colliderName, float radius, int offsetX = 0, int offsetY = 0, bool enabled = true)
        {
            RemoveCollider(colliderName);
            var collider = new ColliderCircle(this, colliderName, radius, offsetX, offsetY, enabled);
            Colliders.Add(collider);
            return collider;
        }

        public ColliderRectangle AddColliderRectangle(string colliderName, int offsetX, int offsetY, int width, int height, bool enabled = true)
        {
            RemoveCollider(colliderName);
            var collider = new ColliderRectangle(this, colliderName, offsetX, offsetY, width, height, enabled);
            Colliders.Add(collider);
            return collider;
        }

        public Collider GetCollider(string colliderName)
        {
            foreach (var item in Colliders)
            {
                if (item.Name == colliderName)
                    return item;
            }
            return null;
        }

        public void RemoveCollider(string colliderName)
        {
            for (int i = Colliders.Count - 1; i >= 0; i--)
            {
                if (Colliders[i].Name == colliderName)
                {
                    Colliders[i].Enabled = false;
                    Colliders.RemoveAt(i);
                }
            }
        }

        //Event override methods

        public virtual void onSpawn() {}

		public virtual void onDestroy() {}

        public virtual void onPause() {}

        public virtual void onResume(int pauseTime)
        {
            if (IsPauseable)
            {
                foreach (Sprite sprite in Sprites.Values)
                {
                    if (sprite is AnimatedSprite && this.IsPauseable)
                    {
                        ((AnimatedSprite)sprite).Timer.RemoveTime(pauseTime);
                    }
                }
            }
        }

		public virtual void onChangeRoom(Room previousRoom, Room nextRoom) {}

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
            Position.X += Speed.X * 60 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position.Y += Speed.Y * 60 * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public virtual void onCollision(Collider collider, Collider otherCollider, Entity otherInstance) {}

        public virtual void onDraw(SpriteBatch spriteBatch)
		{
            foreach (var sprite in Sprites.Values)
            {
                sprite.Draw(spriteBatch, Position);
            }
        }

        public virtual void onGameEvent(GameEvent gameEvent) {}
	}
}

