using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Engine
{
    static class EntityManager
    {
        public static float TimeSinceLastButtonPress { get { return _last_button_press_timer.TotalMilliseconds; } }
        public static bool ButtonHasBeenPressed { get { return _input_state.ButtonHasBeenPressed; } }
        public static List<Entity> Entities { get { return _entities; } }
        static List<Entity> _entities = new List<Entity>();
        private static Entity _current_focusable_entity = null;
        private static GameTimeSpan _last_button_press_timer { get { return _input_state._last_button_press_timer; } }
        private static bool _paused = false;
        private static GameTimeSpan _pause_timer = new GameTimeSpan();
        private static EngineInputState _input_state = new EngineInputState();

        public static void Pause()
        {
            var entity_list = _entities.ToList();
            foreach (var entity in entity_list)
            {
                entity.onPause();
            }
            _paused = true;
            _pause_timer.Mark();
        }
        public static void Resume()
        {
            int pause_time = (int)_pause_timer.TotalMilliseconds;
            var entity_list = _entities.ToList();
            foreach (var entity in entity_list)
            {
                entity.onResume(pause_time);
            }
            _paused = false;
        }
        public static bool IsPaused() { return _paused; }

        public static void PostGameEvent(GameEvent game_event)
        {
            var entity_list = _entities.ToList();
            foreach (var entity in entity_list)
            {
                entity.onGameEvent(game_event);
            }
        }


        public static void ChangeRoom(Room previous_room, Room next_room)
        {
            if (previous_room != null)
            {
                if (previous_room.Persistent)
                    previous_room.Entities = _entities.Where(x => !(x.IsPersistent)).ToList();
                var entity_list = _entities.ToList();
                foreach (var entity in entity_list)
                    entity.onChangeRoom(previous_room, next_room);
            }

            Clear();
            if (next_room.Persistent)
            {
                foreach (var entity in next_room.Entities)
                    _entities.Add(entity);
                next_room.Entities.Clear();
            }
        }

        public static void Add(Entity entity)
        {
            entity.IsExpired = false;
            _entities.Add(entity);
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

        public static T GetFirst<T>() where T : Entity
        {
            return _entities.OfType<T>().FirstOrDefault();
        }

        public static IFocusable GetFocused()
        {
            var entity_list = _entities.ToList();
            foreach (var entity in entity_list)
                if (entity is IFocusable && ((IFocusable)entity).IsFocused)
                    return (IFocusable)entity;
            return null;
        }

        public static void Update(GameTime gameTime)
        {
            _input_state.Update();

            var entity_list = _entities.ToList();
            var starting_room = RoomManager.CurrentRoom;
            bool started_paused = _paused;

            _current_focusable_entity = null;
            foreach (var entity in entity_list)
            {
                if (RoomManager.CurrentRoom != starting_room)
                    break;

                if (started_paused && entity.IsPauseable)
                    continue;

                foreach (var mouse_press in _input_state.MousePresses)
                {
                    if (ShouldProcessInput(entity))
                        entity.onMouseDown(mouse_press);
                }
                foreach (var touch_press in _input_state.TouchPresses)
                {
                    if (entity is ITouchable && ShouldProcessInput(entity))
                    {
                        ((ITouchable)entity).onTouchPressed(touch_press);
                    }
                }
                foreach (var key_press in _input_state.KeyPresses)
                {
                    if (ShouldProcessInput(entity))
                        entity.onKeyDown(key_press);
                }
                foreach (var button_press in _input_state.GamepadPresses)
                {
                    if (ShouldProcessInput(entity))
                        entity.onButtonDown(button_press);
                }

                if (ShouldProcessInput(entity))
                    entity.onMouse(_input_state.MouseState);
                if (entity is ITouchable && ShouldProcessInput(entity))
                    ((ITouchable)entity).onTouch(_input_state.TouchState);
                if (ShouldProcessInput(entity))
                    entity.onKey(_input_state.KeyboardState);
                if (ShouldProcessInput(entity))
                    entity.onButton(_input_state.GamepadStates);

                foreach (var mouse_release in _input_state.MouseReleases)
                {
                    if (ShouldProcessInput(entity))
                        entity.onMouseUp(mouse_release);
                }
                foreach (var touch_release in _input_state.TouchReleases)
                {
                    if (entity is ITouchable && ShouldProcessInput(entity))
                    {
                        ((ITouchable)entity).onTouchReleased(touch_release);
                    }
                }
                foreach (var key_release in _input_state.KeyReleases)
                {
                    if (ShouldProcessInput(entity))
                        entity.onKeyUp(key_release);
                }
                foreach (var button_release in _input_state.GamepadReleases)
                {
                    if (ShouldProcessInput(entity))
                        entity.onButtonUp(button_release);
                }
                entity.onUpdate(gameTime);
            }

            
            // Do all collision checking
            var collider_list = new List<Collider>();
            foreach (var entity in entity_list)
            {
                if (!entity.IsExpired)
                {
                    foreach (var collider in entity.Colliders)
                    {
                        if (collider.Enabled)
                        {
                            collider_list.Add(collider);
                        }
                    }
                }
            }

            for (int i = collider_list.Count - 1; i >= 0; i--)
            {
                var collider = collider_list[i];
                for (int j = collider_list.Count - 2; j >= 0; j--)
                {
                    var other_collider = collider_list[j];

                    if (collider.Owner.IsExpired)
                    {
                        break;
                    }
                    if (other_collider.Owner.IsExpired)
                    {
                        continue;
                    }

                    var collision_occured = false;
                    if (collider is ColliderCircle)
                    {
                        var c1 = (ColliderCircle)collider;
                        if (other_collider is ColliderCircle)
                        {
                            var c2 = (ColliderCircle)other_collider;
                            collision_occured = CollisionChecking.CircleCircle(c1.Position.X, c1.Position.Y, c1.Radius, c2.Position.X, c2.Position.Y, c2.Radius);
                        }
                        else
                        {
                            var c2 = (ColliderRectangle)other_collider;
                            collision_occured = CollisionChecking.CircleRect(c1.Position.X, c1.Position.Y, c1.Radius, c2.Position.X, c2.Position.Y, c2.Width, c2.Height);
                        }
                    }
                    else
                    {
                        var c1 = (ColliderRectangle)collider;
                        if (other_collider is ColliderCircle)
                        {
                            var c2 = (ColliderCircle)other_collider;
                            collision_occured = CollisionChecking.CircleRect(c2.Position.X, c2.Position.Y, c2.Radius, c1.Position.X, c1.Position.Y, c1.Width, c1.Height);
                        }
                        else
                        {
                            var c2 = (ColliderRectangle)other_collider;
                            collision_occured = CollisionChecking.RectRect(c1.Position.X, c1.Position.Y, c1.Width, c1.Height, c2.Position.X, c2.Position.Y, c2.Width, c2.Height);
                        }

                    }
                    if (collision_occured)
                    {
                        collider.Owner.onCollision(collider, other_collider, other_collider.Owner);
                        other_collider.Owner.onCollision(other_collider, collider, collider.Owner);
                    }
                }
                collider_list.RemoveAt(i);
            }

            // Destroying Expired Entities
            int goto_count = 0;

            destroy_expired_entities:
            int destroyed_entity_count = 0;
            foreach (var entity in entity_list)
            {
                if (entity.IsExpired && _entities.Contains(entity))
                {
                    destroyed_entity_count++;
                    entity.onDestroy();
                    _entities.Remove(entity);
                }
            }
            if (destroyed_entity_count > 0)
            {
                goto_count++;
                if (goto_count < 250)
                {
                    goto destroy_expired_entities;
                }
                else
                {
                    throw new Exception("WARNING! You appear to have an endless loop of creating and destroying entities!");
                }
            }
        }

        public static void DrawToRenderTargets(SpriteBatch spriteBatch)
        {
            var entity_list = _entities.ToList();
            foreach (var renderCanvas in entity_list.OfType<RenderCanvas>())
            {
                if (renderCanvas.ShouldDraw)
                {
                    GameRoot.graphicsDevice.SetRenderTarget(renderCanvas.othersRenderTarget);
                    if (renderCanvas.ShouldClear)
                        GameRoot.graphicsDevice.Clear(renderCanvas.BackgroundColor);
                    spriteBatch.Begin(SpriteSortMode.BackToFront);
                    var second_entity_list = _entities.ToList();
                    foreach (var entity in second_entity_list)
                    {
                        if (entity.renderTarget == renderCanvas && entity.ShouldDraw)
                        {
                            entity.onDraw(spriteBatch);
                        }
                    }
                    spriteBatch.End();
                    GameRoot.graphicsDevice.SetRenderTarget(null);
                    GameRoot.BoxingViewport.Reset();
                }

            }

        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, transformMatrix: GameRoot.BoxingViewport.GetScaleMatrix());
            var entity_list = _entities.ToList();
            foreach (var entity in entity_list)
            {
                if (entity.renderTarget == null && entity.ShouldDraw)
                {
                    entity.onDraw(spriteBatch);
                }
            }
            spriteBatch.End();
        }

        private static bool ShouldProcessInput(Entity entity)
        {
            if (!(entity is IFocusable))
            {
                return true;
            }
            else if (entity is IFocusable && ((IFocusable)entity).IsFocused)
            {
                if (_current_focusable_entity == null)
                {
                    _current_focusable_entity = entity;
                    return true;
                }
                else if (_current_focusable_entity == entity)
                {
                    return true;
                }
            }
            return false;
        }

    }
}

