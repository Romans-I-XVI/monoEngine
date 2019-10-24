using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoEngine
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
        public float Alpha = 255f;
		public Color Color = Color.White;
        public SpriteEffects SpriteEffects = SpriteEffects.None;
        public bool Enabled = true;
        protected float LayerDepth = 0;

		public Sprite(Region region)
		{
            Region = region;
        }

        public virtual void Draw (SpriteBatch spriteBatch, Vector2 position)
		{
            if (Enabled)
            {
                this.DrawSelf(spriteBatch, position);
            }
		}

        public void DrawSelf(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(Region.Texture, position + Offset, Region.SourceRectangle, Color * (Alpha / 255f), VectorMath.DegreesToRadians(Rotation), Region.Origin, Scale, SpriteEffects, LayerDepth);
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

        public AnimatedSprite(Region[] regions, int animationSpeed = 0, bool reverseAnimationDirection = false) : base(regions[0])
        {
            Regions = regions;
            AnimationSpeed = animationSpeed;
            ReverseAnimationDirection = reverseAnimationDirection;
            Timer = new GameTimeSpan();
        }

        public void Animate()
        {
            var frameCount = Regions.Length;
            var currentTime = Timer.TotalMilliseconds;
            int startFrame;
            int destFrame;
            if (!ReverseAnimationDirection)
            {
                startFrame = 0;
                destFrame = frameCount;
            }
            else
            {
                startFrame = frameCount - 1;
                destFrame = -1;
            }

            if (currentTime > AnimationSpeed)
            {
                int time_to_remove = ((int)(currentTime / AnimationSpeed)) * AnimationSpeed;
                currentTime -= time_to_remove;
                Timer.RemoveTime(time_to_remove);
            }
            Index = (int)Tweening.SwitchTween(AnimationTween, startFrame, destFrame, currentTime, AnimationSpeed);
            if (Index > frameCount - 1)
            {
                Index = frameCount - 1;
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
                if (AnimationSpeed > 0 && !Engine.IsPaused())
                {
                    Animate();
                }
                Region = Regions[Index];
                base.Draw(spriteBatch, position);
            }
        }
    }
}

