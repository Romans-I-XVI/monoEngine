using System;
using System.Collections.Generic;
namespace Engine
{
    public enum Tween
    {
        Linear,
        Sinusoidal
    }

    public class Tweener
    {

        public Tween Tween;
        public float Start;
        public float Dest;
        public float Current { get { return Tweens.SwitchTween(Tween, Start, Dest, Timer.TotalMilliseconds, Duration); } }
        public float Duration;
        public readonly GameTimeSpan Timer;
        public bool Done { get { return Timer.TotalMilliseconds >= Duration; } }

        public Tweener(float start, float dest, float duration, Tween tween, bool is_pauseable = true)
        {
            Timer = new GameTimeSpan(is_pauseable);
            Start = start;
            Dest = dest;
            Duration = duration;
        }

        public void ChangeDest(float dest)
        {
            Start = Current;
            Dest = dest;
            Timer.Mark();
        }
    }

    public class Oscillator
    {
        private Tweener _tweener;
        private bool _forward = true;
        private float _start;
        private float _dest;

        public Oscillator(float start, float dest, float duration, Tween tween)
        {
            _start = start;
            _dest = dest;
            _tweener = new Tweener(start, dest, duration, tween);
        }

        public float Update()
        {
            float current = _tweener.Current;

            if (_tweener.Done)
            {
                if (_forward)
                {
                    _tweener.ChangeDest(_start);
                    _forward = false;
                }
                else
                {
                    _tweener.ChangeDest(_dest);
                    _forward = true;
                }
            }

            return current;
        }

    }

    public static class Tweens
    {
        public static float SwitchTween(Tween tween, float start, float finish, float currentTime, float duration)
        {
            switch (tween)
            {
                case Tween.Linear:
                    return LinearTween(start, finish, currentTime, duration);
                case Tween.Sinusoidal:
                    return SinusoidalTween(start, finish, currentTime, duration);
                default:
                    return 0;
            }
        }

        public static float LinearTween(float start, float finish, float currentTime, float duration)
        {
            if (currentTime > duration || Math.Abs(duration) <= 0)
                return finish;
            float change = finish - start;
            float time = currentTime / duration;
            return change * time + start;
        }

        public static float SinusoidalTween(float start, float finish, float currentTime, float duration)
        {
            if (currentTime > duration || duration == 0)
                return finish;
            float change = finish - start;
            float time = currentTime / duration;
            return (float)(change * Math.Sin(time * (Math.PI / 2)) + start);
        }
    }
}
