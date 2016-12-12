using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Engine
{
    public class ParticleEmitter : Entity
    {
        GameTimeSpan _timer = new GameTimeSpan();
        public Color Color;
        public Vector2 Size;
        public int Rate;
        public float Depth;
        public float FadeSpeed;
        public float ParticleSpeed;
        readonly Random _random_number_generator = new Random((int)DateTime.Now.Ticks);
        public ParticleEmitter(Vector2 position, Color particle_color, Vector2 particle_size, float particle_speed, int spawn_rate, int fade_speed, float depth)
        {
            Position = position;
            Color = particle_color;
            Size = particle_size;
            Rate = spawn_rate;
            FadeSpeed = fade_speed;
            Depth = depth;
            ParticleSpeed = particle_speed;
        }

        public override void onUpdate(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (_timer.TotalMilliseconds >= Rate)
            {
                float random_direction = _random_number_generator.Next(0, 57297) / 1000f;
                float x_speed = (float)Math.Cos(random_direction) * ParticleSpeed;
                float y_speed = (float)Math.Sin(random_direction) * ParticleSpeed;
                new Particle(Position, Size, new Vector2(x_speed, y_speed), Color, FadeSpeed, Depth);
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
        GameTimeSpan _timer = new GameTimeSpan();
        public Particle(Vector2 position, Vector2 size, Vector2 velocity, Color color, float fade_speed, float depth)
        {
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
            Position.X += _velocity.X * dt;
            Position.Y += _velocity.Y * dt;
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
