using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoEngine
{
    public static class Engine
    {
        public static EngineGame Game { get; private set; }
        public static Room Room { get; private set; }
        public static float Dt { get; private set; }
        public static int FPS { get; private set; }
        public static Random Random = new Random();

        private static SpriteSortMode _spriteSortMode = SpriteSortMode.Deferred;
        private static bool _paused;
        private static GameTimeSpan _pauseTimer = new GameTimeSpan();
        private static GameTimeSpan _fpsTimer = new GameTimeSpan();
        private static EngineInputState _inputState = new EngineInputState();
        private static Dictionary<string, object> _currentRoomArgs;
        private static List<Entity> _entities = new List<Entity>();
        private static int _currentFrameCount = 0;

        public static void Start(EngineGame game)
        {
            Game = game;
            Game.Run();
        }

        public static void Update(GameTime gameTime)
        {
            Dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _inputState.Update();

            List<Entity> entityList;
            lock (_entities)
            {
                entityList = _entities.OrderByDescending(entity => entity.Depth).ToList();
            }
            var startingRoom = Room;
            bool startedPaused = _paused;

            foreach (var entity in entityList)
            {
                if (Room != startingRoom)
                    break;

                if (startedPaused && entity.IsPauseable)
                    continue;

                foreach (var mousePress in _inputState.MousePresses)
                {
                    entity.onMouseDown(mousePress);
                    if (entity.IsExpired) break;
                }
                if (entity.IsExpired) continue;

                foreach (var touchPress in _inputState.TouchPresses)
                {
                    if (entity is ITouchable)
                    {
                        ((ITouchable)entity).onTouchPressed(touchPress);
                        if (entity.IsExpired) break;
                    }
                }
                if (entity.IsExpired) continue;


                foreach (var keyPress in _inputState.KeyPresses)
                {
                    entity.onKeyDown(keyPress);
                    if (entity.IsExpired) break;
                }
                if (entity.IsExpired) continue;

                foreach (var buttonPress in _inputState.GamepadPresses)
                {
                    entity.onButtonDown(buttonPress);
                    if (entity.IsExpired) break;
                }
                if (entity.IsExpired) continue;

                entity.onMouse(_inputState.MouseState);
                if (entity.IsExpired) continue;

                if (entity is ITouchable)
                {
                    ((ITouchable)entity).onTouch(_inputState.TouchState);
                    if (entity.IsExpired) continue;
                }

                entity.onKey(_inputState.KeyboardState);
                if (entity.IsExpired) continue;

                entity.onButton(_inputState.GamepadStates);
                if (entity.IsExpired) continue;

                foreach (var mouseRelease in _inputState.MouseReleases)
                {
                    entity.onMouseUp(mouseRelease);
                    if (entity.IsExpired) break;
                }
                if (entity.IsExpired) continue;

                foreach (var touchRelease in _inputState.TouchReleases)
                {
                    if (entity is ITouchable)
                    {
                        ((ITouchable)entity).onTouchReleased(touchRelease);
                        if (entity.IsExpired) break;
                    }
                }
                if (entity.IsExpired) continue;

                foreach (var keyRelease in _inputState.KeyReleases)
                {
                    entity.onKeyUp(keyRelease);
                    if (entity.IsExpired) break;
                }
                if (entity.IsExpired) continue;

                foreach (var buttonRelease in _inputState.GamepadReleases)
                {
                    entity.onButtonUp(buttonRelease);
                    if (entity.IsExpired) break;
                }
                if (entity.IsExpired) continue;

                entity.onUpdate(gameTime);
            }


            // Do all collision checking
            var colliderList = new List<Collider>();
            foreach (var entity in entityList)
            {
                if (!entity.IsExpired)
                {
                    foreach (var collider in entity.Colliders)
                    {
                        if (collider.Enabled)
                        {
                            colliderList.Add(collider);
                        }
                    }
                }
            }

            for (int i = colliderList.Count - 1; i >= 0; i--)
            {
                var collider = colliderList[i];
                for (int j = colliderList.Count - 2; j >= 0; j--)
                {
                    var otherCollider = colliderList[j];

                    if (collider.Owner.IsExpired)
                    {
                        break;
                    }
                    if (otherCollider.Owner.IsExpired)
                    {
                        continue;
                    }

                    var collisionOccured = false;
                    if (collider is ColliderCircle)
                    {
                        var c1 = (ColliderCircle)collider;
                        if (otherCollider is ColliderCircle)
                        {
                            var c2 = (ColliderCircle)otherCollider;
                            collisionOccured = CollisionChecking.CircleCircle(c1.Position.X, c1.Position.Y, c1.Radius, c2.Position.X, c2.Position.Y, c2.Radius);
                        }
                        else
                        {
                            var c2 = (ColliderRectangle)otherCollider;
                            collisionOccured = CollisionChecking.CircleRect(c1.Position.X, c1.Position.Y, c1.Radius, c2.Position.X, c2.Position.Y, c2.Width, c2.Height);
                        }
                    }
                    else
                    {
                        var c1 = (ColliderRectangle)collider;
                        if (otherCollider is ColliderCircle)
                        {
                            var c2 = (ColliderCircle)otherCollider;
                            collisionOccured = CollisionChecking.CircleRect(c2.Position.X, c2.Position.Y, c2.Radius, c1.Position.X, c1.Position.Y, c1.Width, c1.Height);
                        }
                        else
                        {
                            var c2 = (ColliderRectangle)otherCollider;
                            collisionOccured = CollisionChecking.RectRect(c1.Position.X, c1.Position.Y, c1.Width, c1.Height, c2.Position.X, c2.Position.Y, c2.Width, c2.Height);
                        }

                    }
                    if (collisionOccured)
                    {
                        collider.Owner.onCollision(collider, otherCollider, otherCollider.Owner);
                        otherCollider.Owner.onCollision(otherCollider, collider, collider.Owner);
                    }
                }
                colliderList.RemoveAt(i);
            }

            // Destroying Expired Entities
            int gotoCount = 0;

            destroy_expired_entities:
            int destroyedEntityCount = 0;
            lock (_entities)
            {
                entityList = _entities.ToList();
            }
            foreach (var entity in entityList)
            {
                if (entity.IsExpired)
                {
                    destroyedEntityCount++;
                    DestroyInstance(entity);
                }
            }
            if (destroyedEntityCount > 0)
            {
                gotoCount++;
                if (gotoCount < 250)
                {
                    goto destroy_expired_entities;
                }
                else
                {
                    throw new Exception("WARNING! You appear to have an endless loop of creating and destroying entities!");
                }
            }
        }

        public static void Draw(GameTime gameTime)
        {
            // Update the current fps
            _currentFrameCount++;
            if (_fpsTimer.TotalMilliseconds >= 1000)
            {
                FPS = _currentFrameCount;
                _currentFrameCount = 0;
                _fpsTimer.Mark();
                Console.WriteLine(FPS);
            }

            List<Entity> entityList;
            lock (_entities)
            {
                entityList = _entities.OrderByDescending(entity => entity.Depth).ToList();
            }

            // Draw to render targets
            foreach (var renderCanvas in entityList.OfType<RenderCanvas>())
            {
                if (renderCanvas.ShouldDraw)
                {
                    Game.GraphicsDevice.SetRenderTarget(renderCanvas.othersRenderTarget);
                    if (renderCanvas.ShouldClear)
                    {
                        Game.GraphicsDevice.Clear(renderCanvas.BackgroundColor);
                    }

                    Game.SpriteBatch.Begin(_spriteSortMode);
                    List<Entity> secondEntityList;
                    lock (_entities)
                    {
                        secondEntityList = _entities.OrderByDescending(entity => entity.Depth).ToList();
                    }
                    foreach (var entity in secondEntityList)
                    {
                        if (entity.renderTarget == renderCanvas && entity.ShouldDraw)
                        {
                            entity.onDraw(Game.SpriteBatch);
                        }
                    }
                    Game.SpriteBatch.End();

                    Game.GraphicsDevice.SetRenderTarget(null);
                    Game.Viewport.Reset();
                }

            }

            // Clear the screen
            Game.GraphicsDevice.Clear(Game.BackgroundColor);

            // Draw to the screen
            Game.SpriteBatch.Begin(_spriteSortMode, transformMatrix: Game.Viewport.GetScaleMatrix());
            foreach (var entity in entityList)
            {
                if (entity.renderTarget == null && entity.ShouldDraw)
                {
                    entity.onDraw(Game.SpriteBatch);
                }
            }
            Game.SpriteBatch.End();
        }

        public static void Pause()
        {
            List<Entity> entityList;
            lock (_entities)
            {
                entityList = _entities.ToList();
            }
            foreach (var entity in entityList)
            {
                entity.onPause();
            }
            _paused = true;
            _pauseTimer.Mark();
        }

        public static void Resume()
        {
            int pauseTime = (int)_pauseTimer.TotalMilliseconds;
            List<Entity> entityList;
            lock (_entities)
            {
                entityList = _entities.ToList();
            }
            foreach (var entity in entityList)
            {
                entity.onResume(pauseTime);
            }
            _paused = false;
        }

        public static bool IsPaused()
        {
            return _paused;
        }

        public static void ChangeRoom<T>(Dictionary<string, object> args = null) where T : Room, new()
        {
            ChangeRoom(new T(), args);
        }

        private static void ChangeRoom(Room room, Dictionary<string, object> args = null)
        {
            Room previousRoom = Room;
            Room = room;
            _currentRoomArgs = args;

            if (previousRoom != null)
            {
                // Call onChangeRoom on all entities
                List<Entity> entityList;
                lock (_entities)
                {
                    entityList = _entities.ToList();
                }
                foreach (var entity in entityList)
                {
                    entity.onChangeRoom(previousRoom, Room);
                }

                // Call onSwitchAway on previous room
                previousRoom.onSwitchAway(Room);
            }


            // Clear entities and change the room
            lock (_entities)
            {
                _entities = _entities.Where(entity => entity.IsPersistent).ToList();
            }
            Room.onSwitchTo(previousRoom, args);
        }

        public static void ResetRoom()
        {
            ChangeRoom(Room, _currentRoomArgs);
        }

        public static void SpawnInstance(Entity entity)
        {
            entity.IsExpired = false;
            lock (_entities)
            {
                if (_entities.Contains(entity))
                {
                    return;
                }

                _entities.Add(entity);
                entity.onSpawn();
            }

        }

        public static T SpawnInstance<T>() where T : Entity, new()
        {
            var instance = new T();
            SpawnInstance(instance);
            return instance;
        }

        public static void DestroyInstance(Entity entity)
        {
            if (entity == null)
            {
                return;
            }

            entity.IsExpired = true;
            lock (_entities)
            {
                if (_entities.Contains(entity))
                {
                    entity.onDestroy();
                    _entities.Remove(entity);
                }
            }
        }

        public static void DestroyAllInstances<T>() where T : Entity
        {
            var instances = GetAllInstances<T>();
            for (int i = 0; i < instances.Count(); i++)
            {
                DestroyInstance(instances[i]);
            }
        }

        public static T GetFirstInstanceByType<T>() where T : Entity
        {
            lock (_entities)
            {
                return _entities.OfType<T>().FirstOrDefault();
            }
        }

        public static Entity GetFirstInstanceByID(int id)
        {
            lock (_entities)
            {
                return _entities.FirstOrDefault(entity => entity.ID == id);
            }
        }

        public static List<T> GetAllInstances<T>() where T : Entity
        {
            lock (_entities)
            {
                return _entities.OfType<T>().ToList();
            }
        }

        public static int InstanceCount<T>() where T : Entity
        {
            lock (_entities)
            {
                return _entities.OfType<T>().Count();
            }
        }

        public static bool ToggleFullscreen()
        {
            Game.Graphics.ToggleFullScreen();
            Game.Viewport.Reset();
            return Game.Graphics.IsFullScreen;
        }

        public static void PostGameEvent(GameEvent gameEvent)
        {
            List<Entity> entityList;
            lock (_entities)
            {
                entityList = _entities.ToList();
            }
            foreach (var entity in entityList)
            {
                entity.onGameEvent(gameEvent);
            }
        }
    }
}
