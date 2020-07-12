using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoEngine
{
    public static class Engine
    {
        private static EngineGame _game = null;
        public static EngineGame Game
        {
            get
            {
                return _game;
            }
            set
            {
                if (_game == null)
                {
                    _game = value;
                    EventHandler<EventArgs> on_disposed = null;
                    on_disposed = (object sender, EventArgs e) =>
                    {
                        Engine.Reset();
                        _game.Disposed -= on_disposed;
                        _game = null;
                    };
                    _game.Disposed += on_disposed;
                }
            }
        }
        public static double? FakeDt = null;
        public static InputLayer InputLayer { get; private set; } = InputLayer.One;
        public static Room Room { get; private set; }
        public static double Dt { get; private set; }
        public static int FPS { get; private set; }
        public static Random Random = new Random();
        public static class MainSpritebatchSettings
        {
            public static BlendState BlendState = null;
            public static SamplerState SamplerState = null;
            public static DepthStencilState DepthStencilState = null;
            public static RasterizerState RasterizerState = null;
            public static Effect Effect = null;
        }

        private static SpriteSortMode _spriteSortMode = SpriteSortMode.Deferred;
        private static bool _paused;
        private static GameTimeSpan _pauseTimer = new GameTimeSpan();
        private static GameTimeSpan _fpsTimer = new GameTimeSpan();
        private static Dictionary<string, object> _currentRoomArgs;
        private static List<Entity> _entities = new List<Entity>();
        private static int _currentFrameCount = 0;

        public static void Update(GameTime gameTime)
        {
            Dt = gameTime.ElapsedGameTime.TotalSeconds;
            float deltaTime = (float)Dt;
            if (FakeDt != null)
            {
                deltaTime = (float)FakeDt;
            }
            Input.Update();
            EngineInputState.Update();

            Entity[] sortedEntities;
            lock (_entities)
            {
                _entities = _entities.OrderByDescending(entity => entity.Depth).ToList();
                sortedEntities = _entities.ToArray();
            }
            var startingRoom = Room;
            bool startedPaused = _paused;

            for (int entity_index = 0; entity_index < sortedEntities.Length; entity_index++)
            {
                var entity = sortedEntities[entity_index];

                if (Room != startingRoom)
                    break;

                if (startedPaused && entity.IsPauseable)
                    continue;

                if ((entity.InputLayer & Engine.InputLayer) != 0) {
                    for (int i = 0; i < EngineInputState.MousePresses.Length; i++)
                    {
                        var mousePress = EngineInputState.MousePresses[i];
                        entity.onMouseDown(mousePress);
                        if (entity.IsExpired) break;
                    }

                    if (entity.IsExpired) continue;

                    for (int i = 0; i < EngineInputState.TouchPresses.Length; i++)
                    {
                        var touchPress = EngineInputState.TouchPresses[i];
                        if (entity is ITouchable)
                        {
                            ((ITouchable)entity).onTouchPressed(touchPress);
                            if (entity.IsExpired) break;
                        }
                    }

                    if (entity.IsExpired) continue;

                    for (int i = 0; i < EngineInputState.KeyPresses.Length; i++)
                    {
                        var keyPress = EngineInputState.KeyPresses[i];
                        entity.onKeyDown(keyPress);
                        if (entity.IsExpired) break;
                    }

                    if (entity.IsExpired) continue;

                    for (int i = 0; i < EngineInputState.GamepadPresses.Length; i++)
                    {
                        var buttonPress = EngineInputState.GamepadPresses[i];
                        entity.onButtonDown(buttonPress);
                        if (entity.IsExpired) break;
                    }

                    if (entity.IsExpired) continue;

                    entity.onMouse(EngineInputState.MouseState);
                    if (entity.IsExpired) continue;

                    if (entity is ITouchable)
                    {
                        ((ITouchable)entity).onTouch(EngineInputState.TouchState);
                        if (entity.IsExpired) continue;
                    }

                    entity.onKey(EngineInputState.KeyboardState);
                    if (entity.IsExpired) continue;

                    entity.onButton(EngineInputState.GamepadStates);
                    if (entity.IsExpired) continue;

                    for (int i = 0; i < EngineInputState.MouseReleases.Length; i++)
                    {
                        var mouseRelease = EngineInputState.MouseReleases[i];
                        entity.onMouseUp(mouseRelease);
                        if (entity.IsExpired) break;
                    }

                    if (entity.IsExpired) continue;

                    if (entity is ITouchable)
                    {
                        for (int i = 0; i < EngineInputState.TouchReleases.Length; i++)
                        {
                            var touchRelease = EngineInputState.TouchReleases[i];
                            ((ITouchable)entity).onTouchReleased(touchRelease);
                            if (entity.IsExpired) break;
                        }
                    }

                    if (entity.IsExpired) continue;

                    for (int i = 0; i < EngineInputState.KeyReleases.Length; i++)
                    {
                        var keyRelease = EngineInputState.KeyReleases[i];
                        entity.onKeyUp(keyRelease);
                        if (entity.IsExpired) break;
                    }

                    if (entity.IsExpired) continue;

                    for (int i = 0; i < EngineInputState.GamepadReleases.Length; i++)
                    {
                        var buttonRelease = EngineInputState.GamepadReleases[i];
                        entity.onButtonUp(buttonRelease);
                        if (entity.IsExpired) break;
                    }

                    if (entity.IsExpired) continue;
                }

                entity.onUpdate(deltaTime);
            }

            // Do all collision checking
            var colliderList = new List<Collider>();
            for (int i = 0; i < sortedEntities.Length; i++)
            {
                var entity = sortedEntities[i];
                if (!entity.IsExpired) {
                    for (int j = 0; j < entity.Colliders.Count; j++)
                    {
                        var collider = entity.Colliders[j];
                        if (collider.Enabled)
                        {
                            colliderList.Add(collider);
                        }
                    }
                }
            }

            var colliderArray = colliderList.ToArray();
            int colliderArrayCount = colliderArray.Length;
            for (int i = colliderArrayCount - 1; i >= 0; i--)
            {
                Collider collider = colliderArray[i];
                for (int j = colliderArrayCount - 2; j >= 0; j--)
                {
                    Collider otherCollider = colliderArray[j];
                    bool collisionIsValid = ((collider.CollidableFlags & otherCollider.MemberFlags) != 0) || ((otherCollider.CollidableFlags & collider.MemberFlags) != 0);

                    if (collider.Owner.IsExpired || !collider.Enabled)
                    {
                        break;
                    }

                    if (!collisionIsValid || otherCollider.Owner.IsExpired || !otherCollider.Enabled || collider.Owner == otherCollider.Owner)
                    {
                        continue;
                    }

                    bool collisionOccured = false;
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

                colliderArrayCount--;
            }

            // Destroying Expired Entities
            int gotoCount = 0;

            destroy_expired_entities:
            int destroyedEntityCount = 0;
            if (gotoCount > 0)
            {
                lock (_entities)
                {
                    sortedEntities = _entities.ToArray();
                }
            }

            for (int i = 0; i < sortedEntities.Length; i++)
            {
                var entity = sortedEntities[i];
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
                    Game.GraphicsDevice.SetRenderTarget(renderCanvas.OthersRenderTarget);
                    if (renderCanvas.ShouldClear)
                    {
                        Game.GraphicsDevice.Clear(renderCanvas.BackgroundColor);
                    }

                    Game.SpriteBatch.Begin(_spriteSortMode, renderCanvas.BlendState, renderCanvas.SamplerState, renderCanvas.DepthStencilState, renderCanvas.RasterizerState, renderCanvas.Effect, renderCanvas.TransformMatrix);
                    List<Entity> secondEntityList;
                    lock (_entities)
                    {
                        secondEntityList = _entities.OrderByDescending(entity => entity.Depth).ToList();
                    }
                    foreach (var entity in secondEntityList)
                    {
                        if (entity.RenderTarget == renderCanvas && entity.ShouldDraw)
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
            Game.SpriteBatch.Begin(_spriteSortMode, MainSpritebatchSettings.BlendState, MainSpritebatchSettings.SamplerState, MainSpritebatchSettings.DepthStencilState, MainSpritebatchSettings.RasterizerState, MainSpritebatchSettings.Effect, Game.Viewport.GetScaleMatrix());
            foreach (var entity in entityList)
            {
                if (entity.RenderTarget == null && entity.ShouldDraw)
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
                var persistent_entities = new List<Entity>();
                for (int i = 0; i < _entities.Count; i++)
                {
                    var entity = _entities[i];
                    if (entity.IsPersistent)
                        persistent_entities.Add(entity);
                    else
                        entity.IsExpired = true;
                }

                _entities = persistent_entities;
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

        public static void SetInputLayer(InputLayer inputLayer)
        {
            InputLayer = inputLayer;
        }

        private static void Reset()
        {
            _spriteSortMode = SpriteSortMode.Deferred;
            _paused = false;
            _currentRoomArgs = null;
            _entities = new List<Entity>();
            _currentFrameCount = 0;
            FakeDt = null;
            InputLayer = InputLayer.One;
            Room = null;
            MainSpritebatchSettings.BlendState = null;
            MainSpritebatchSettings.SamplerState = null;
            MainSpritebatchSettings.DepthStencilState = null;
            MainSpritebatchSettings.RasterizerState = null;
            MainSpritebatchSettings.Effect = null;
        }
    }
}
