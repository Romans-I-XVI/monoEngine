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
        public static bool ButtonHasBeenPressed { get; private set; }
        public static List<Entity> Entities { get { return _entities; } }
        static List<Entity> _entities = new List<Entity>();
        private static Entity _current_focusable_entity = null;
        private readonly static GameTimeSpan _last_button_press_timer = new GameTimeSpan();


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
            EngineInputState inputState = GetEngineInputState();

            var entity_list = _entities.ToList();
            var starting_room = RoomManager.CurrentRoom;

            _current_focusable_entity = null;
            foreach (var entity in entity_list)
            {
                if (RoomManager.CurrentRoom != starting_room)
                    break;
                
                foreach (var key_press in inputState.KeyPresses)
                {
                    if (ShouldProcessInput(entity))
                        entity.onKeyDown(key_press);
                }
                foreach (var button_press in inputState.GamepadPresses)
                {
                    if (ShouldProcessInput(entity))
                        entity.onButtonDown(button_press);
                }

                if (ShouldProcessInput(entity))
                    entity.onKey(inputState.KeyboardState);
                if (ShouldProcessInput(entity))
                    entity.onButton(inputState.GamepadStates);
                if (ShouldProcessInput(entity))
                    entity.onMouse(inputState.MouseState);

                foreach (var key_release in inputState.KeyReleases)
                {
                    if (ShouldProcessInput(entity))
                        entity.onKeyUp(key_release);
                }
                foreach (var button_release in inputState.GamepadReleases)
                {
                    if (ShouldProcessInput(entity))
                        entity.onButtonUp(button_release);
                }
                entity.onUpdate(gameTime);

            }

            // Destroying Expired Entities
            int goto_count = 0;
            destroy_expired_entities:
            var expired_entities = _entities.Where(x => x.IsExpired).ToList();
            foreach (var entity in expired_entities)
            {
                entity.onDestroy();
                _entities.Remove(entity);
            }
            if (_entities.Where(x => x.IsExpired).Count() > 0)
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

        public static EngineInputState GetEngineInputState()
        {
            Input.Update();
            
            var keyboard_pressed_events = new List<KeyboardEventArgs>();
            var keyboard_released_events = new List<KeyboardEventArgs>();
            var gamepad_pressed_events = new List<GamePadEventArgs>();
            var gamepad_released_events = new List<GamePadEventArgs>();

            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                if (key == Keys.ChatPadOrange || key == Keys.ChatPadGreen)
                    continue;

                if (Input.Keyboard.isPressed(key))
                    keyboard_pressed_events.Add(new KeyboardEventArgs(key));
                else if (Input.Keyboard.isReleased(key))
                    keyboard_released_events.Add(new KeyboardEventArgs(key));
            }

            foreach (PlayerIndex player in Enum.GetValues(typeof(PlayerIndex)))
            {
                foreach (Buttons button in Enum.GetValues(typeof(Buttons)))
                {
                    if (Input.Gamepad.isPressed(button, player))
                    {
                        ButtonHasBeenPressed = true;
                        _last_button_press_timer.Mark();
                        gamepad_pressed_events.Add(new GamePadEventArgs(player, button));
                    }
                    else if (Input.Gamepad.isReleased(button, player))
                        gamepad_released_events.Add(new GamePadEventArgs(player, button));
                }
            }

            return new EngineInputState(keyboard_pressed_events, keyboard_released_events, gamepad_pressed_events, gamepad_released_events);
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

