using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Engine
{
    public class ParticleEmitter : Entity
    {
        GameTimeSpan _timer;
        public Color Color;
        public Vector2 Size;
        public float Rate;
        public float Depth;
        public float FadeSpeed;
        public float ParticleSpeed;
        public float SpawnOffset;
        public RenderCanvas RenderCanvas;
        readonly Random _random_number_generator = new Random((int)DateTime.Now.Ticks);
        public ParticleEmitter(Vector2 position, Color particle_color, Vector2 particle_size, float particle_speed, float spawn_rate, float fade_speed, float depth, float spawn_offset = 0, RenderCanvas render_canvas = null)
        {
            _timer = new GameTimeSpan();
            SpawnOffset = spawn_offset;
            Position = position;
            Color = particle_color;
            Size = particle_size;
            Rate = spawn_rate;
            FadeSpeed = fade_speed;
            Depth = depth;
            ParticleSpeed = particle_speed;
            if (render_canvas != null)
                RenderCanvas = render_canvas;
        }

        public override void onUpdate(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            float passed_ms = _timer.TotalMilliseconds;
            if (passed_ms >= Rate)
            {

                for (int i = 0; i < (int)(passed_ms / Rate); i++)
                {
                    float random_direction = _random_number_generator.Next(0, 57297) / 1000f;
                    float x_speed = (float)Math.Cos(random_direction) * ParticleSpeed;
                    float y_speed = (float)Math.Sin(random_direction) * ParticleSpeed;
                    float x_offset = (float)Math.Cos(random_direction) * SpawnOffset;
                    float y_offset = (float)Math.Sin(random_direction) * SpawnOffset;
                    var particle = new Particle(new Vector2(Position.X + x_offset, Position.Y + y_offset), Size, new Vector2(x_speed, y_speed), Color, FadeSpeed * (float)(0.5 + _random_number_generator.NextDouble()), Depth);
                    if (RenderCanvas != null)
                        particle.SetRenderCanvas(RenderCanvas);
                }
                _timer.Mark();
            }

            base.onUpdate(gameTime);
        }
    }

    public class Particle : Entity
    {
        Vector2 _size;
        Vector2 _velocity;
        Color _color;
        float _depth;
        float _fade_speed;
        GameTimeSpan _timer;
        public Particle(Vector2 position, Vector2 size, Vector2 velocity, Color color, float fade_speed, float depth)
        {
            _timer = new GameTimeSpan();
            Position = position;
            _size = size;
            _velocity = velocity;
            _color = color;
            _depth = depth;
            _fade_speed = fade_speed;
        }

        public override void onUpdate(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            Position.X += _velocity.X * dt / 60;
            Position.Y += _velocity.Y * dt / 60;
            base.onUpdate(gameTime);
        }

        public override void onDraw(SpriteBatch spriteBatch)
        {
            float alpha = Tweens.SinusoidalTween(0.8f, 0f, _timer.TotalMilliseconds, _fade_speed);
            RectangleDrawer.Draw(spriteBatch, Position.X, Position.Y, _size.X, _size.Y, _color * alpha, _size.X / 2, _size.Y / 2, _depth);

            if (alpha <= 0)
                IsExpired = true;
            base.onDraw(spriteBatch);
        }
    }
}
