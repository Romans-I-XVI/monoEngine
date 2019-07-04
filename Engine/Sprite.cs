using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Engine
{
    public enum DrawFrom
    {
        TopLeft,
        TopCenter,
        TopRight,
        BottomLeft,
        BottomCenter,
        BottomRight,
        Center,
        RightCenter,
        LeftCenter,
    }

	public class Sprite
	{
        public Region Region { get; protected set; }
		public float Rotation = 0f;
        public Vector2 Offset = new Vector2();
		public Vector2 Scale = new Vector2(1.0f, 1.0f);
		public float Depth = 0.5f;
        public float Alpha = 255f;
		public Color Color = Color.White;
        public SpriteEffects SpriteEffects = SpriteEffects.None;
        public bool Enabled = true;

		public Sprite(Region region)
		{
            Region = region;
        }

        public virtual void Draw (SpriteBatch spriteBatch, Vector2 position)
		{
            if (Enabled)
            {
                spriteBatch.Draw(Region.Texture, position + Offset, Region.SourceRectangle, Color * (Alpha / 255f), VectorMath.DegreesToRadians(Rotation), Region.Origin, Scale, SpriteEffects, Depth);
            }
		}
	}

    public class AnimatedSprite : Sprite
    {
        public Region[] Regions { get; protected set; }
        public int Index = 0;
        public Tween AnimationTween = Tween.LinearTween;
        public int AnimationSpeed;
        public bool ReverseAnimationDirection;
        public GameTimeSpan Timer { get; protected set; }

        public AnimatedSprite(Region[] regions, int animation_speed = 0, bool reverse_animation_direction = false) : base(regions[0])
        {
            Regions = regions;
            AnimationSpeed = animation_speed;
            ReverseAnimationDirection = reverse_animation_direction;
            Timer = new GameTimeSpan();
        }

        public void Animate()
        {
            var frame_count = Regions.Length;
            var current_time = Timer.TotalMilliseconds;
            int start_frame;
            int dest_frame;
            if (!ReverseAnimationDirection)
            {
                start_frame = 0;
                dest_frame = frame_count;
            }
            else
            {
                start_frame = frame_count - 1;
                dest_frame = -1;
            }

            if (current_time > AnimationSpeed)
            {
                current_time -= AnimationSpeed;
                Timer.RemoveTime(AnimationSpeed);
            }
            Index = (int)Tweens.SwitchTween(AnimationTween, start_frame, dest_frame, current_time, AnimationSpeed);
            if (Index > frame_count - 1)
            {
                Index = frame_count - 1;
            }
            else if (Index < 0)
            {
                Index = 0;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            if (Enabled)
            {
                if (AnimationSpeed > 0 && !EntityManager.IsPaused())
                {
                    Animate();
                }
                Region = Regions[Index];
                base.Draw(spriteBatch, position);
            }
        }
    }
}

