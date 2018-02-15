﻿using System;
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
        private static EngineInputState _input_state = new EngineInputState();

        public static void Pause() { _paused = true; }
        public static void Resume() { _paused = false; }
        public static bool IsPaused() { return _paused; }


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

                if (entity is ITouchable && ShouldProcessInput(entity))
                    ((ITouchable)entity).onTouch(_input_state.TouchState);
                if (ShouldProcessInput(entity))
                    entity.onKey(_input_state.KeyboardState);
                if (ShouldProcessInput(entity))
                    entity.onButton(_input_state.GamepadStates);
                if (ShouldProcessInput(entity))
                    entity.onMouse(_input_state.MouseState);

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
                    if (Engine.Settings.Debug)
                        Console.WriteLine("Recursive Destruction Occured - Count: " + goto_count.ToString());
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

