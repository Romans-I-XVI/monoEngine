using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoEngine
{
    public class Debugger : Entity
    {
        public static readonly Dictionary<string, string> Variables = new Dictionary<string, string>()
        {
            {"draw_colliders", "0"},
            {"draw_safe_zones", "0"},
            {"draw_fps", "0"}
        };

        public Debugger()
        {
            InputLayer = InputLayer.One | InputLayer.DebuggerTerminal;
            Depth = -int.MaxValue;
            IsPersistent = true;
            IsPauseable = false;
        }

        public override void onDraw(SpriteBatch spriteBatch)
        {
            base.onDraw(spriteBatch);

            if (Variables["draw_colliders"] == "1")
            {
                DrawColliders(spriteBatch);
            }

            if (Variables["draw_safe_zones"] == "1")
            {
                DrawSafeZones(spriteBatch);
            }

            if (Variables["draw_fps"] == "1")
            {
                DrawFPS(spriteBatch);
            }
        }

        public void SetDrawColliders(bool shouldDraw)
        {
            Variables["draw_colliders"] = shouldDraw ? "1" : "0";
        }

        public void SetDrawSafeZones(bool shouldDraw)
        {
            Variables["draw_safe_zones"] = shouldDraw ? "1" : "0";
        }

        public void SetDrawFPS(bool shouldDraw)
        {
            Variables["draw_fps"] = shouldDraw ? "1" : "0";
        }

        public static void DrawColliders(SpriteBatch spriteBatch)
        {
            var entities = Engine.GetAllInstances<Entity>();
            foreach (Entity entity in entities)
            {
                foreach (Collider collider in entity.Colliders)
                {
                    if (!collider.Enabled)
                    {
                        continue;
                    }

                    if (collider is ColliderRectangle)
                    {
                        var rect = ((ColliderRectangle)collider).Rectangle;
                        RectangleDrawer.DrawAround(spriteBatch, rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height - 2, Color.Red, 1);
                    }
                    else if (collider is ColliderCircle)
                    {
                        var circle = ((ColliderCircle)collider).Circle;

                        var lineCount = circle.Radius * 10;
                        for (int i = 0; i < lineCount; i++)
                        {
                            float degrees = 360f * (i / (float)lineCount);
                            float currentX = (float)Math.Cos(VectorMath.DegreesToRadians(degrees)) * circle.Radius;
                            float currentY = (float)Math.Sin(VectorMath.DegreesToRadians(degrees)) * circle.Radius;
                            RectangleDrawer.Draw(spriteBatch, circle.X + currentX, circle.Y + currentY, 1, 1, Color.Red);
                        }
                    }
                }
            }
        }

        public static void DrawSafeZones(SpriteBatch spriteBatch)
        {
            var screenWidth = Engine.Game.Viewport.VirtualWidth;
            var screenHeight = Engine.Game.Viewport.VirtualHeight;

            var actionSafeZone = new Rectangle(0, 0, (int)(screenWidth * 0.93f), (int)(screenHeight * 0.93f));
            actionSafeZone.X = (screenWidth - actionSafeZone.Width) / 2;
            actionSafeZone.Y = (screenHeight - actionSafeZone.Height) / 2;
            RectangleDrawer.Draw(spriteBatch, actionSafeZone, Color.Red * (60f / 255f));

            var titleSafeZone = new Rectangle(0, 0, (int)(screenWidth * 0.90f), (int)(screenHeight * 0.90f));
            titleSafeZone.X = (screenWidth - titleSafeZone.Width) / 2;
            titleSafeZone.Y = (screenHeight - titleSafeZone.Height) / 2;
            RectangleDrawer.Draw(spriteBatch, titleSafeZone, Color.Blue * (60f / 255f));
        }

        public static void DrawFPS(SpriteBatch spriteBatch)
        {
            int x = 0;
            int y = 0;
            float viewportScale = (float)Engine.Game.Viewport.ViewportWidth / (float)Engine.Game.Viewport.VirtualWidth;

            if (spriteBatch.GraphicsDevice.Viewport.X < 0)
            {
                x += (int)(-spriteBatch.GraphicsDevice.Viewport.X / viewportScale);
            }

            if (spriteBatch.GraphicsDevice.Viewport.Y < 0)
            {
                y += (int)(-spriteBatch.GraphicsDevice.Viewport.Y / viewportScale);
            }

            int fps = Engine.FPS;
            int charCount = fps.ToString().Length;
            int height = 20;
            int buffer = height / 4;
            int width = charCount * (height / 2) + charCount * buffer + buffer;

            RectangleDrawer.Draw(spriteBatch, x, y - 2, width, height + 8, Color.Black);
            NumberDrawer.Draw(spriteBatch, fps, x + buffer, y + 2, height, Color.White);
        }
    }

    public class DebuggerWithTerminal : Debugger
    {
        public bool ConsoleOpen { get; private set; }
        private Action<string> _evaluator;
        private SpriteFont _spriteFont;
        private string _consoleInput = "";
        private readonly GameTimeSpan _cursorBlinkTimer = new GameTimeSpan();
        private readonly GameTimeSpan _keyRepeatTimer = new GameTimeSpan();
        private Keys _mostRecentKeyPressed;
        private bool _cursorBlinkState = false;
        private InputLayer _previousInputLayer;

        public DebuggerWithTerminal(SpriteFont spriteFont)
        {
            _evaluator = this.Evaluate;
            _previousInputLayer = Engine.InputLayer;
            _spriteFont = spriteFont;
        }

        public override void onUpdate(float dt)
        {
            if (ConsoleOpen && _cursorBlinkTimer.TotalMilliseconds > 400)
            {
                _cursorBlinkState = !_cursorBlinkState;
                _cursorBlinkTimer.Mark();
            }

            base.onUpdate(dt);
        }

        public override void onKey(KeyboardState state)
        {
            if (_keyRepeatTimer.TotalMilliseconds >= 25 && state.IsKeyDown(_mostRecentKeyPressed) && _mostRecentKeyPressed != Keys.OemTilde && _mostRecentKeyPressed != Keys.Enter)
            {
                onKeyInput(new KeyboardEventArgs(_mostRecentKeyPressed));
                _keyRepeatTimer.Mark();
            }

            base.onKey(state);
        }

        public override void onKeyDown(KeyboardEventArgs e)
        {
            _keyRepeatTimer.Mark(-475);
            _mostRecentKeyPressed = e.Key;
            onKeyInput(e);

            base.onKeyDown(e);
        }

        public override void onDraw(SpriteBatch spriteBatch)
        {
            base.onDraw(spriteBatch);

            if (ConsoleOpen)
            {
                int height = 28;
                int border = 2;
                float viewportScale = (float)Engine.Game.Viewport.ViewportWidth / (float)Engine.Game.Viewport.VirtualWidth;
                int startX = 0;
                int startY = 0;

                if (spriteBatch.GraphicsDevice.Viewport.X < 0)
                {
                    startX = (int)(-spriteBatch.GraphicsDevice.Viewport.X / viewportScale);
                }

                if (spriteBatch.GraphicsDevice.Viewport.Y < 0)
                {
                    startY = (int)(-spriteBatch.GraphicsDevice.Viewport.Y / viewportScale);
                }

                RectangleDrawer.Draw(spriteBatch, startX, startY, Engine.Game.Viewport.VirtualWidth - startX * 2, height, Color.White);
                RectangleDrawer.Draw(spriteBatch, startX + border, startY + border, Engine.Game.Viewport.VirtualWidth - border * 2 - startX * 2, height - border * 2, Color.Black);
                float scale = 18 / _spriteFont.MeasureString("|").Y;
                spriteBatch.DrawString(_spriteFont, _consoleInput, new Vector2(startX + border + 5, startY + border + 3), Color.White, 0, Vector2.Zero, new Vector2(scale), SpriteEffects.None, 0);

                if (_cursorBlinkState)
                {
                    Vector2 cursorPosition = new Vector2(0, _spriteFont.MeasureString("a").Y * scale);
                    if (_consoleInput.Length > 0)
                    {
                        cursorPosition = (_spriteFont.MeasureString(_consoleInput) * scale);
                    }
                    cursorPosition += new Vector2(startX + border + 5, startY + border + 1);
                    RectangleDrawer.Draw(spriteBatch, cursorPosition.X, cursorPosition.Y, 10, 2, Color.White);
                }
            }
        }

        private void onKeyInput(KeyboardEventArgs e)
        {
            if (e.Key == Keys.OemTilde)
            {
                OpenCloseConsole();
            }
            else if (ConsoleOpen)
            {
                if (e.Key == Keys.Back)
                {
                    if (_consoleInput.Length > 0)
                    {
                        _consoleInput = _consoleInput.Substring(0, _consoleInput.Length - 1);
                    }
                }
                else if (e.Key == Keys.Enter)
                {
                    if (_evaluator == null)
                        _evaluator = Evaluate;
                    _evaluator(_consoleInput);
                    OpenCloseConsole();
                }
                else
                {
                    var modifier = KeyMap.Modifier.None;
                    if (Input.Keyboard.isHeld(Keys.LeftShift) || Input.Keyboard.isHeld(Keys.RightShift))
                    {
                        modifier = KeyMap.Modifier.Shift;
                    }
                    _consoleInput += KeyMap.GetChar(e.Key, modifier);
                    _cursorBlinkState = true;
                    _cursorBlinkTimer.Mark();
                }
            }
        }

        public void OpenCloseConsole()
        {
            ConsoleOpen = !ConsoleOpen;
            if (ConsoleOpen)
            {
                _previousInputLayer = Engine.InputLayer;
                Engine.SetInputLayer(InputLayer.DebuggerTerminal);
            }
            else
            {
                Engine.SetInputLayer(_previousInputLayer);
            }
            _consoleInput = "";
            _evaluator = Evaluate;
            if (ConsoleOpen)
            {
                if (!Engine.IsPaused())
                {
                    Engine.Pause();
                }
            }
            else
            {
                if (Engine.IsPaused())
                {
                    Engine.Resume();
                }
            }
        }

        public void OpenConsoleWithCustomEvaluator(Action<string> evaluator)
        {
            if (!ConsoleOpen) {
                OpenCloseConsole();
            }
            _evaluator = evaluator;
        }

        public void Evaluate(string input)
        {
            if (input.Contains("="))
            {
                string left_expression = input.Substring(0, input.IndexOf('='));
                string right_expression = input.Substring(input.IndexOf('=') + 1, input.Length - left_expression.Length - 1);
                left_expression = left_expression.Trim();
                right_expression = right_expression.Trim();

                Variables[left_expression] = right_expression;
            }
        }
    }
}
