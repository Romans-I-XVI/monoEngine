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
		public Texture2D Texture { get; private set; }
        public Rectangle? SourceRectangle = null;
		public float Rotation = 0f;
		public Vector2 Origin = new Vector2();
        public Vector2 Offset = new Vector2();
		public Vector2 Scale = new Vector2(1.0f, 1.0f);
		public float Depth = 0.5f;
        public float Alpha = 255f;
		public Color Color = Color.White;
        public bool Enabled = true;

		public Sprite (Texture2D texture)
		{
			Texture = texture;
		}

		public virtual void Draw (SpriteBatch spriteBatch, Vector2 position)
		{
            if (Enabled)
                spriteBatch.Draw (Texture, sourceRectangle: SourceRectangle, position: position + Offset, origin: Origin, rotation: Rotation, scale: Scale, color: Color * (Alpha/255f), layerDepth: Depth);
		}

        public void AutoOrigin(DrawFrom draw_from)
        {
            int width;
            int height;
            if (SourceRectangle.HasValue)
            {
                width = SourceRectangle.Value.Width;
                height = SourceRectangle.Value.Height;
            }
            else
            {
                width = Texture.Width;
                height = Texture.Height;
            }

            switch (draw_from)
            {
                case DrawFrom.TopCenter:
                    Origin.X = width / 2;
                    break;
                case DrawFrom.TopRight:
                    Origin.X = width;
                    break;
                case DrawFrom.BottomLeft:
                    Origin.Y = height;
                    break;
                case DrawFrom.BottomCenter:
                    Origin.X = width / 2;
                    Origin.Y = height;
                    break;
                case DrawFrom.BottomRight:
                    Origin.X = width;
                    Origin.Y = height;
                    break;
                case DrawFrom.Center:
                    Origin.X = width / 2;
                    Origin.Y = height / 2;
                    break;
                case DrawFrom.RightCenter:
                    Origin.X = width;
                    Origin.Y = height / 2;
                    break;
                case DrawFrom.LeftCenter:
                    Origin.Y = height / 2;
                    break;
            }
        }
	}

    public class AnimatedSprite : Sprite
    {
        public int RegionCount { get; private set; }
        public int RegionWidth { get; private set; }
        public int RegionHeight { get; private set; }
        public int AnimationSpeed;
        public int AnimationPosition;
        public bool ReverseAnimationDirection;
        private GameTimeSpan _timer;
        private int _previous_animation_position;

        public AnimatedSprite(Texture2D texture, int region_count, int region_width, int region_height, int animation_speed = 0, int animation_position = 0, bool reverse_animation_direction = false) : base(texture)
        {
            RegionCount = region_count;
            RegionWidth = region_width;
            RegionHeight = region_height;
            SourceRectangle = new Rectangle(0, 0, RegionWidth, RegionHeight);

            AnimationSpeed = animation_speed;
            AnimationPosition = animation_position;
            ReverseAnimationDirection = reverse_animation_direction;
            _previous_animation_position = AnimationPosition;
            _timer = new GameTimeSpan();
        }

        public void Process()
        {
            if (RegionCount > 1 && AnimationSpeed > 0)
            {
                if (!ReverseAnimationDirection)
                    AnimationPosition = (int)Tweens.LinearTween(0, RegionCount, _timer.TotalMilliseconds, AnimationSpeed);
                else
                    AnimationPosition = (int)Tweens.LinearTween(RegionCount, 0, _timer.TotalMilliseconds, AnimationSpeed);

                if (AnimationPosition > RegionCount - 1)
                    AnimationPosition = RegionCount - 1;

                if (_timer.TotalMilliseconds >= AnimationSpeed)
                {
                    if (!ReverseAnimationDirection)
                        AnimationPosition = 0;
                    else
                        AnimationPosition = RegionCount - 1;
                    _timer.Mark();
                }
            }

            if (AnimationPosition != _previous_animation_position)
            {
                int y_row_offset = (AnimationPosition * RegionWidth / Texture.Width);
                int x_offset = (AnimationPosition * RegionWidth - Texture.Width * y_row_offset);
                int y_offset = RegionHeight * y_row_offset;

                SourceRectangle = new Rectangle(x_offset, y_offset, RegionWidth, RegionHeight);
                _previous_animation_position = AnimationPosition;
            }
        }
    }
}

