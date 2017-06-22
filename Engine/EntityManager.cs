using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.InputListeners;
using MonoGame.Extended.ViewportAdapters;

namespace Engine
{
    static class EntityManager
    {
        public static float TimeSinceLastButtonPress { get { return _last_button_press_timer.TotalMilliseconds; } }
        public static bool ButtonHasBeenPressed { get; private set; }
        public static bool EnableGamepadSupport = true;
        public static List<Entity> Entities { get { return _entities; } }
        static List<Entity> _entities = new List<Entity>();
        private static bool _processed_focusable_input;
        private readonly static GameTimeSpan _last_button_press_timer = new GameTimeSpan();
        public static bool ProcessedFocusableInput { get { return _processed_focusable_input; } }


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
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyboardState = Keyboard.GetState();
            Dictionary<PlayerIndex, GamePadState> gamepadStates = new Dictionary<PlayerIndex, GamePadState>()
            {
                {PlayerIndex.One, GamePad.GetState(PlayerIndex.One)},
                {PlayerIndex.Two, GamePad.GetState(PlayerIndex.Two)},
                {PlayerIndex.Three, GamePad.GetState(PlayerIndex.Three)},
                {PlayerIndex.Four, GamePad.GetState(PlayerIndex.Four)}
            };
            var entity_list = _entities.ToList();

            _processed_focusable_input = false;
            foreach (var entity in entity_list)
            {
                if (ShouldProcessInput(entity))
                {
                    entity.onMouse(mouseState);
                    entity.onKey(keyboardState);
                    entity.onButton(gamepadStates);
                }
                entity.onUpdate(gameTime);
                if (entity.IsExpired)
                    entity.onDestroy();
            }

            _entities = _entities.Where(x => !x.IsExpired).ToList();
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
                            entity.onDrawBegin(spriteBatch);
                            entity.onDraw(spriteBatch);
                            entity.onDrawEnd(spriteBatch);
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
                    entity.onDrawBegin(spriteBatch);
                    entity.onDraw(spriteBatch);
                    entity.onDrawEnd(spriteBatch);
                }
            }
            spriteBatch.End();
        }

        public static class MouseEvents
        {
            public static void onMouseDown(object sender, MouseEventArgs e)
            {
                _processed_focusable_input = false;
                var entity_list = _entities.ToList();
                foreach (var entity in entity_list)
                    if (ShouldProcessInput(entity))
                        entity.onMouseDown(e);
            }

            public static void onMouseUp(object sender, MouseEventArgs e)
            {
                _processed_focusable_input = false;
                var entity_list = _entities.ToList();
                foreach (var entity in entity_list)
                    if (ShouldProcessInput(entity))
                        entity.onMouseUp(e);
            }

            public static void onMouseWheel(object sender, MouseEventArgs e)
            {
                _processed_focusable_input = false;
                var entity_list = _entities.ToList();
                foreach (var entity in entity_list)
                    if (ShouldProcessInput(entity))
                        entity.onMouseWheel(e);
            }

        }

        public static class KeyboardEvents
        {

            public static void onKeyPressed(object sender, KeyboardEventArgs e)
            {
                _processed_focusable_input = false;
                var entity_list = _entities.ToList();
                foreach (var entity in entity_list)
                    if (ShouldProcessInput(entity))
                        entity.onKeyDown(e);
            }

            public static void onKeyReleased(object sender, KeyboardEventArgs e)
            {
                _processed_focusable_input = false;
                var entity_list = _entities.ToList();
                foreach (var entity in entity_list)
                    if (ShouldProcessInput(entity))
                        entity.onKeyUp(e);
            }

        }

        public static class GamepadEvents
        {
            public static void onButtonDown(object sender, GamePadEventArgs e)
            {
                ButtonHasBeenPressed = true;
                _last_button_press_timer.Mark();
                if (!EnableGamepadSupport)
                    return;
                var entity_list = _entities.ToList();

                _processed_focusable_input = false;
                foreach (var entity in entity_list)
                    if (ShouldProcessInput(entity))
                        entity.onButtonDown(e);
            }

            public static void onButtonUp(object sender, GamePadEventArgs e)
            {
                if (!EnableGamepadSupport)
                    return;
                var entity_list = _entities.ToList();

                _processed_focusable_input = false;
                foreach (var entity in entity_list)
                    if (ShouldProcessInput(entity))
                        entity.onButtonUp(e);
            }
        }

        private static bool ShouldProcessInput(Entity entity)
        {
            if (!(entity is IFocusable))
            {
                return true;
            }
            else if (entity is IFocusable && ((IFocusable)entity).IsFocused && !_processed_focusable_input)
            {
                _processed_focusable_input = true;
                return true;
            }
            return false;
        }

    }
}

