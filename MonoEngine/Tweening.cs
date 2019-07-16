using System;
using System.Collections.Generic;
namespace MonoEngine
{
    public enum Tween
    {
        LinearTween,
        QuadraticTween,
        QuadraticEaseIn,
        QuadraticEaseOut,
        QuadraticEaseInOut,
        QuadraticEaseOutIn,
        SquareTween,
        SquareEaseIn,
        SquareEaseOut,
        SquareEaseInOut,
        SquareEaseOutIn,
        CubicTween,
        CubicEaseIn,
        CubicEaseOut,
        CubicEaseInOut,
        CubicEaseOutIn,
        QuarticTween,
        QuarticEaseIn,
        QuarticEaseOut,
        QuarticEaseInOut,
        QuarticEaseOutIn,
        QuinticTween,
        QuinticEaseIn,
        QuinticEaseOut,
        QuinticEaseInOut,
        QuinticEaseOutIn,
        SinusoidalTween,
        SinusoidalEaseIn,
        SinusoidalEaseOut,
        SinusoidalEaseInOut,
        SinusoidalEaseOutIn,
        ExponentialTween,
        ExponentialEaseIn,
        ExponentialEaseOut,
        ExponentialEaseInOut,
        ExponentialEaseOutIn,
        CircularTween,
        CircularEaseIn,
        CircularEaseOut,
        CircularEaseInOut,
        CircularEaseOutIn,
        ElasticTween,
        ElasticEaseIn,
        ElasticEaseOut,
        ElasticEaseInOut,
        ElasticEaseOutIn,
        OvershootTween,
        OvershootEaseIn,
        OvershootEaseOut,
        OvershootEaseInOut,
        OvershootEaseOutIn,
        BounceTween,
        BounceEaseIn,
        BounceEaseOut,
        BounceEaseInOut,
        BounceEaseOutIn
    }

    public class Tweener
    {

        public Tween Tween;
        public float Start;
        public float Dest;
        public float Current { get { return Tweening.SwitchTween(Tween, Start, Dest, Timer.TotalMilliseconds, Duration); } }
        public float Duration;
        public readonly GameTimeSpan Timer;
        public bool Done { get { return Timer.TotalMilliseconds >= Duration; } }

        public Tweener(float start, float dest, float duration, Tween tween)
        {
            Timer = new GameTimeSpan();
            Start = start;
            Dest = dest;
            Duration = duration;
            Tween = tween;
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
        public Tweener Tweener { get; private set; }
        private bool _forward = true;
        private float _start;
        private float _dest;

        public Oscillator(float start, float dest, float duration, Tween tween)
        {
            _start = start;
            _dest = dest;
            Tweener = new Tweener(start, dest, duration, tween);
        }

        public float Update()
        {
            float current = Tweener.Current;

            if (Tweener.Done)
            {
                if (_forward)
                {
                    Tweener.ChangeDest(_start);
                    _forward = false;
                }
                else
                {
                    Tweener.ChangeDest(_dest);
                    _forward = true;
                }
            }

            return current;
        }

    }

    public static class Tweening
    {
        public static float SwitchTween(Tween tween, float start, float finish, float currentTime, float duration)
        {
            switch (tween)
            {
                case Tween.LinearTween:
                    return LinearTween(start, finish, currentTime, duration);
                case Tween.QuadraticTween:
                    return QuadraticTween(start, finish, currentTime, duration);
                case Tween.QuadraticEaseIn:
                    return QuadraticEaseIn(start, finish, currentTime, duration);
                case Tween.QuadraticEaseOut:
                    return QuadraticEaseOut(start, finish, currentTime, duration);
                case Tween.QuadraticEaseInOut:
                    return QuadraticEaseInOut(start, finish, currentTime, duration);
                case Tween.QuadraticEaseOutIn:
                    return QuadraticEaseOutIn(start, finish, currentTime, duration);
                case Tween.SquareTween:
                    return SquareTween(start, finish, currentTime, duration);
                case Tween.SquareEaseIn:
                    return SquareEaseIn(start, finish, currentTime, duration);
                case Tween.SquareEaseOut:
                    return SquareEaseOut(start, finish, currentTime, duration);
                case Tween.SquareEaseInOut:
                    return SquareEaseInOut(start, finish, currentTime, duration);
                case Tween.SquareEaseOutIn:
                    return SquareEaseOutIn(start, finish, currentTime, duration);
                case Tween.CubicTween:
                    return CubicTween(start, finish, currentTime, duration);
                case Tween.CubicEaseIn:
                    return CubicEaseIn(start, finish, currentTime, duration);
                case Tween.CubicEaseOut:
                    return CubicEaseOut(start, finish, currentTime, duration);
                case Tween.CubicEaseInOut:
                    return CubicEaseInOut(start, finish, currentTime, duration);
                case Tween.CubicEaseOutIn:
                    return CubicEaseOutIn(start, finish, currentTime, duration);
                case Tween.QuarticTween:
                    return QuarticTween(start, finish, currentTime, duration);
                case Tween.QuarticEaseIn:
                    return QuarticEaseIn(start, finish, currentTime, duration);
                case Tween.QuarticEaseOut:
                    return QuarticEaseOut(start, finish, currentTime, duration);
                case Tween.QuarticEaseInOut:
                    return QuarticEaseInOut(start, finish, currentTime, duration);
                case Tween.QuarticEaseOutIn:
                    return QuarticEaseOutIn(start, finish, currentTime, duration);
                case Tween.QuinticTween:
                    return QuinticTween(start, finish, currentTime, duration);
                case Tween.QuinticEaseIn:
                    return QuinticEaseIn(start, finish, currentTime, duration);
                case Tween.QuinticEaseOut:
                    return QuinticEaseOut(start, finish, currentTime, duration);
                case Tween.QuinticEaseInOut:
                    return QuinticEaseInOut(start, finish, currentTime, duration);
                case Tween.QuinticEaseOutIn:
                    return QuinticEaseOutIn(start, finish, currentTime, duration);
                case Tween.SinusoidalTween:
                    return SinusoidalTween(start, finish, currentTime, duration);
                case Tween.SinusoidalEaseIn:
                    return SinusoidalEaseIn(start, finish, currentTime, duration);
                case Tween.SinusoidalEaseOut:
                    return SinusoidalEaseOut(start, finish, currentTime, duration);
                case Tween.SinusoidalEaseInOut:
                    return SinusoidalEaseInOut(start, finish, currentTime, duration);
                case Tween.SinusoidalEaseOutIn:
                    return SinusoidalEaseOutIn(start, finish, currentTime, duration);
                case Tween.ExponentialTween:
                    return ExponentialTween(start, finish, currentTime, duration);
                case Tween.ExponentialEaseIn:
                    return ExponentialEaseIn(start, finish, currentTime, duration);
                case Tween.ExponentialEaseOut:
                    return ExponentialEaseOut(start, finish, currentTime, duration);
                case Tween.ExponentialEaseInOut:
                    return ExponentialEaseInOut(start, finish, currentTime, duration);
                case Tween.ExponentialEaseOutIn:
                    return ExponentialEaseOutIn(start, finish, currentTime, duration);
                case Tween.CircularTween:
                    return CircularTween(start, finish, currentTime, duration);
                case Tween.CircularEaseIn:
                    return CircularEaseIn(start, finish, currentTime, duration);
                case Tween.CircularEaseOut:
                    return CircularEaseOut(start, finish, currentTime, duration);
                case Tween.CircularEaseInOut:
                    return CircularEaseInOut(start, finish, currentTime, duration);
                case Tween.CircularEaseOutIn:
                    return CircularEaseOutIn(start, finish, currentTime, duration);
                case Tween.ElasticTween:
                    return ElasticTween(start, finish, currentTime, duration);
                case Tween.ElasticEaseIn:
                    return ElasticEaseIn(start, finish, currentTime, duration);
                case Tween.ElasticEaseOut:
                    return ElasticEaseOut(start, finish, currentTime, duration);
                case Tween.ElasticEaseInOut:
                    return ElasticEaseInOut(start, finish, currentTime, duration);
                case Tween.ElasticEaseOutIn:
                    return ElasticEaseOutIn(start, finish, currentTime, duration);
                case Tween.OvershootTween:
                    return OvershootTween(start, finish, currentTime, duration);
                case Tween.OvershootEaseIn:
                    return OvershootEaseIn(start, finish, currentTime, duration);
                case Tween.OvershootEaseOut:
                    return OvershootEaseOut(start, finish, currentTime, duration);
                case Tween.OvershootEaseInOut:
                    return OvershootEaseInOut(start, finish, currentTime, duration);
                case Tween.OvershootEaseOutIn:
                    return OvershootEaseOutIn(start, finish, currentTime, duration);
                case Tween.BounceTween:
                    return BounceTween(start, finish, currentTime, duration);
                case Tween.BounceEaseIn:
                    return BounceEaseIn(start, finish, currentTime, duration);
                case Tween.BounceEaseOut:
                    return BounceEaseOut(start, finish, currentTime, duration);
                case Tween.BounceEaseInOut:
                    return BounceEaseInOut(start, finish, currentTime, duration);
                case Tween.BounceEaseOutIn:
                    return BounceEaseOutIn(start, finish, currentTime, duration);
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

        #region Quadratic
        public static float QuadraticTween(float start, float finish, float currentTime, float duration)
        {
            return QuadraticEaseInOut(start, finish, currentTime, duration);
        }

        public static float QuadraticEaseIn(float start, float finish, float currentTime, float duration)
        {
            if (currentTime > duration || duration == 0) { return finish; }
            var change = finish - start;
            var time = currentTime / duration;
            return (change * time * time + start);
        }

        public static float QuadraticEaseOut(float start, float finish, float currentTime, float duration)
        {
            if (currentTime > duration || duration == 0) { return finish; }
            var change = finish - start;
            var time = currentTime / duration;
            return (-change * time * (time - 2) + start);
        }

        public static float QuadraticEaseInOut(float start, float finish, float currentTime, float duration)
        {
            if (currentTime > duration || duration == 0) { return finish; }
            var change = finish - start;
            var time = currentTime / (duration / 2);
            if (time < 1)
            {
                return change / 2 * time * time + start;
            }
            else
            {
                time = time - 1;
                return -change / 2 * (time * (time - 2) - 1) + start;
            }
        }

        public static float QuadraticEaseOutIn(float start, float finish, float currentTime, float duration)
        {
            if (currentTime > duration || duration == 0) { return finish; }
            var change = finish - start;
            if (currentTime < duration / 2)
                return QuadraticEaseOut(0, change, currentTime * 2, duration) * .5f + start;
            else
                return QuadraticEaseIn(0, change, currentTime * 2 - duration, duration) * .5f + (change * .5f) + start;
        }
        #endregion

        #region Square
        public static float SquareTween(float start, float finish, float currentTime, float duration)
        {
            return SquareEaseInOut(start, finish, currentTime, duration);
        }

        public static float SquareEaseIn(float start, float finish, float currentTime, float duration)
        {
            if (currentTime > duration || duration == 0) { return finish; }
            var change = finish - start;
            var time = currentTime / duration;
            return change * time * time + start;
        }

        public static float SquareEaseOut(float start, float finish, float currentTime, float duration)
        {
            if (currentTime > duration || duration == 0) { return finish; }
            var change = finish - start;
            var time = (currentTime / duration) - 1;
            return -change * (time * time - 1) + start;
        }

        public static float SquareEaseInOut(float start, float finish, float currentTime, float duration)
        {
            if (currentTime > duration || duration == 0) { return finish; }
            var change = finish - start;
            var time = currentTime / (duration / 2);
            if (time < 1)
            {
                return change / 2 * time * time + start;
            }
            else
            {
                time = time - 2;
                return -change / 2 * (time * time - 2) + start;
            }
        }

        public static float SquareEaseOutIn(float start, float finish, float currentTime, float duration)
        {
            if (currentTime > duration || duration == 0) { return finish; }
            var change = finish - start;
            if (currentTime < duration / 2)
                return SquareEaseOut(0, change, currentTime * 2, duration) * .5f + start;
            else
                return SquareEaseIn(0, change, currentTime * 2 - duration, duration) * .5f + (change * .5f) + start;
        }
        #endregion

        #region Cubic
        public static float CubicTween(float start, float finish, float currentTime, float duration)
        {
            return CubicEaseInOut(start, finish, currentTime, duration);
        }

        public static float CubicEaseIn(float start, float finish, float currentTime, float duration)
        {
            if (currentTime > duration || duration == 0) { return finish; }
            var change = finish - start;
            var time = currentTime / duration;
            return change * time * time * time + start;
        }

        public static float CubicEaseOut(float start, float finish, float currentTime, float duration)
        {
            if (currentTime > duration || duration == 0) { return finish; }
            var change = finish - start;
            var time = (currentTime / duration) - 1;
            return change * (time * time * time + 1) + start;
        }

        public static float CubicEaseInOut(float start, float finish, float currentTime, float duration)
        {
            if (currentTime > duration || duration == 0) { return finish; }
            var change = finish - start;
            var time = currentTime / (duration / 2);
            if (time < 1)
            {
                return change / 2 * time * time * time + start;
            }
            else
            {
                time = time - 2;
                return change / 2 * (time * time * time + 2) + start;
            }
        }

        public static float CubicEaseOutIn(float start, float finish, float currentTime, float duration)
        {
            if (currentTime > duration || duration == 0) { return finish; }
            var change = finish - start;
            if (currentTime < duration / 2)
                return CubicEaseOut(0, change, currentTime * 2, duration) * .5f + start;
            else
                return CubicEaseIn(0, change, currentTime * 2 - duration, duration) * .5f + (change * .5f) + start;
        }
        #endregion

        #region Quartic
        public static float QuarticTween(float start, float finish, float currentTime, float duration)
        {
            return QuarticEaseInOut(start, finish, currentTime, duration);
        }

        public static float QuarticEaseIn(float start, float finish, float currentTime, float duration)
        {
            if (currentTime > duration || duration == 0) { return finish; }
            var change = finish - start;
            var time = currentTime / duration;
            return change * time * time * time * time + start;
        }

        public static float QuarticEaseOut(float start, float finish, float currentTime, float duration)
        {
            if (currentTime > duration || duration == 0) { return finish; }
            var change = finish - start;
            var time = (currentTime / duration) - 1;
            return -change * (time * time * time * time - 1) + start;
        }

        public static float QuarticEaseInOut(float start, float finish, float currentTime, float duration)
        {
            if (currentTime > duration || duration == 0) { return finish; }
            var change = finish - start;
            var time = currentTime / (duration / 2);
            if (time < 1)
            {
                return change / 2 * time * time * time * time + start;
            }
            else
            {
                time = time - 2;
                return -change / 2 * (time * time * time * time - 2) + start;
            }
        }

        public static float QuarticEaseOutIn(float start, float finish, float currentTime, float duration)
        {
            if (currentTime > duration || duration == 0) { return finish; }
            var change = finish - start;
            if (currentTime < duration / 2)
                return QuarticEaseOut(0, change, currentTime * 2, duration) * .5f + start;
            else
                return QuarticEaseIn(0, change, currentTime * 2 - duration, duration) * .5f + (change * .5f) + start;
        }
        #endregion

        #region Quintic
        public static float QuinticTween(float start, float finish, float currentTime, float duration)
        {
            return QuinticEaseInOut(start, finish, currentTime, duration);
        }

        public static float QuinticEaseIn(float start, float finish, float currentTime, float duration)
        {
            if (currentTime > duration || duration == 0) { return finish; }
            var change = finish - start;
            var time = currentTime / duration;
            return change * time * time * time * time * time + start;
        }

        public static float QuinticEaseOut(float start, float finish, float currentTime, float duration)
        {
            if (currentTime > duration || duration == 0) { return finish; }
            var change = finish - start;
            var time = (currentTime / duration) - 1;
            return change * (time * time * time * time * time + 1) + start;
        }

        public static float QuinticEaseInOut(float start, float finish, float currentTime, float duration)
        {
            if (currentTime > duration || duration == 0) { return finish; }
            var change = finish - start;
            var time = currentTime / (duration / 2);
            if (time < 1)
            {
                return change / 2 * time * time * time * time * time + start;
            }
            else
            {
                time = time - 2;
                return change / 2 * (time * time * time * time * time + 2) + start;
            }
        }

        public static float QuinticEaseOutIn(float start, float finish, float currentTime, float duration)
        {
            if (currentTime > duration || duration == 0) { return finish; }
            var change = finish - start;
            if (currentTime < duration / 2)
                return QuinticEaseOut(0, change, currentTime * 2, duration) * .5f + start;
            else
                return QuinticEaseIn(0, change, currentTime * 2 - duration, duration) * .5f + (change * .5f) + start;
        }
        #endregion

        #region Sinusoidal
        public static float SinusoidalTween(float start, float finish, float currentTime, float duration)
        {
            return SinusoidalEaseInOut(start, finish, currentTime, duration);
        }

        public static float SinusoidalEaseIn(float start, float finish, float currentTime, float duration)
        {
            if (currentTime > duration || duration == 0) { return finish; }
            var pi = Math.PI;
            var change = finish - start;
            var time = currentTime / duration;
            return -change * (float)Math.Cos(time * (pi / 2)) + change + start;
        }

        public static float SinusoidalEaseOut(float start, float finish, float currentTime, float duration)
        {
            if (currentTime > duration || duration == 0) { return finish; }
            var pi = Math.PI;
            var change = finish - start;
            var time = currentTime / duration;
            return change * (float)Math.Sin(time * (pi / 2)) + start;
        }

        public static float SinusoidalEaseInOut(float start, float finish, float currentTime, float duration)
        {
            if (currentTime > duration || duration == 0) { return finish; }
            var pi = Math.PI;
            var change = finish - start;
            var time = currentTime / duration;
            return -change / 2 * (float)(Math.Cos(pi * time) - 1) + start;
        }

        public static float SinusoidalEaseOutIn(float start, float finish, float currentTime, float duration)
        {
            if (currentTime > duration || duration == 0) { return finish; }
            var change = finish - start;
            if (currentTime < duration / 2)
                return SinusoidalEaseOut(0, change, currentTime * 2, duration) * .5f + start;
            else
                return SinusoidalEaseIn(0, change, currentTime * 2 - duration, duration) * .5f + (change * .5f) + start;
        }
        #endregion

        #region Exponential
        public static float ExponentialTween(float start, float finish, float currentTime, float duration)
        {
            return ExponentialEaseInOut(start, finish, currentTime, duration);
        }

        public static float ExponentialEaseIn(float start, float finish, float currentTime, float duration)
        {
            if (currentTime > duration || duration == 0) { return finish; }
            var change = finish - start;
            if (currentTime == 0)
                return start;
            else
                return change * (float)(Math.Pow(2, (10 * (currentTime / duration - 1)))) + start;
        }

        public static float ExponentialEaseOut(float start, float finish, float currentTime, float duration)
        {
            if (currentTime > duration || duration == 0) { return finish; }
            var change = finish - start;
            if (currentTime == duration)
                return start + change;
            else
                return change * (float)(-(Math.Pow(2, (-10 * currentTime / duration))) + 1) + start;
        }

        public static float ExponentialEaseInOut(float start, float finish, float currentTime, float duration)
        {
            if (currentTime > duration || duration == 0) { return finish; }
            var change = finish - start;
            var time = currentTime / (duration / 2);
            if (currentTime == 0)
            {
                return start;
            }
            else if (currentTime == duration)
            {
                return start + change;
            }
            else if (time < 1)
            {
                return change / 2 * (float)(Math.Pow(2, (10 * (time - 1)))) + start;
            }
            else
            {
                time = time - 1;
                return change / 2 * (float)(-(Math.Pow(2, (-10 * time))) + 2) + start;
            }
        }

        public static float ExponentialEaseOutIn(float start, float finish, float currentTime, float duration)
        {
            if (currentTime > duration || duration == 0) { return finish; }
            var change = finish - start;
            var time = currentTime / (duration / 2);
            if (currentTime == 0)
                return start;
            else if (currentTime == duration)
                return start + change;
            else if (time < 1)
                return change / 2 * (float)(-(Math.Pow(2, (-10 * time / 1))) + 1) + start;
            else
                return change / 2 * (float)(Math.Pow(2, (10 * (time - 2) / 1)) + 1) + start;
        }
        #endregion

        #region Circular
        public static float CircularTween(float start, float finish, float currentTime, float duration)
        {
            return CircularEaseInOut(start, finish, currentTime, duration);
        }

        public static float CircularEaseIn(float start, float finish, float currentTime, float duration)
        {
            if (currentTime > duration || duration == 0) { return finish; }
            var change = finish - start;
            var time = currentTime / duration;
            return -change * (float)(Math.Sqrt(1 - time * time) - 1) + start;
        }

        public static float CircularEaseOut(float start, float finish, float currentTime, float duration)
        {
            if (currentTime > duration || duration == 0) { return finish; }
            var change = finish - start;
            var time = (currentTime / duration) - 1;
            return change * (float)Math.Sqrt(1 - time * time) + start;
        }

        public static float CircularEaseInOut(float start, float finish, float currentTime, float duration)
        {
            if (currentTime > duration || duration == 0) { return finish; }
            var change = finish - start;
            var time = currentTime / (duration / 2);
            if (time < 1)
            {
                return -change / 2 * (float)(Math.Sqrt(1 - time * time) - 1) + start;
            }
            else
            {
                time = time - 2;
                return change / 2 * (float)(Math.Sqrt(1 - time * time) + 1) + start;
            }
        }

        public static float CircularEaseOutIn(float start, float finish, float currentTime, float duration)
        {
            if (currentTime > duration || duration == 0) { return finish; }
            var change = finish - start;
            if (currentTime < duration / 2)
                return CircularEaseOut(0, change, currentTime * 2, duration) * .5f + start;
            else
                return CircularEaseIn(0, change, currentTime * 2 - duration, duration) * .5f + (change * .5f) + start;
        }
        #endregion

        #region Elastic
        public static float ElasticTween(float start, float finish, float currentTime, float duration)
        {
            return ElasticEaseInOut(start, finish, currentTime, duration);
        }

        public static float ElasticEaseIn(float start, float finish, float currentTime, float duration)
        {
            if (currentTime > duration || duration == 0) { return finish; }
            var pi = Math.PI;
            var change = finish - start;
            var time = currentTime / duration;
            if (currentTime == 0)
                return start;
            else if (time == 1)
                return start + change;

            var period = duration * 0.3f;
            var speed = period / 4;
            var amplitude = change;
            time = time - 1;
            return -(amplitude * (float)(Math.Pow(2, (10 * time))) * (float)Math.Sin((time * duration - speed) * (2 * pi) / period)) + start;
        }

        public static float ElasticEaseOut(float start, float finish, float currentTime, float duration)
        {
            if (currentTime > duration || duration == 0) { return finish; }
            var pi = Math.PI;
            var change = finish - start;
            var time = currentTime / duration;
            if (currentTime == 0)
                return start;
            else if (time == 1)
                return start + change;
            var period = duration * .3f;
            var speed = period / 4f;
            var amplitude = change;
            return (amplitude * (float)(Math.Pow(2, (-10 * time))) * (float)Math.Sin((time * duration - speed) * (2 * pi) / period) + change + start);
        }

        public static float ElasticEaseInOut(float start, float finish, float currentTime, float duration)
        {
            if (currentTime > duration || duration == 0) { return finish; }
            var pi = Math.PI;
            var change = finish - start;
            var time = currentTime / (duration / 2);
            if (currentTime == 0)
                return start;
            else if (time == 2)
                return start + change;

            var period = duration * (.3 * 1.5);
            var speed = period / 4;
            var amplitude = change;
            time = time - 1;
            if (time < 0)
                return -.5f * (amplitude * (float)(Math.Pow(2, (10 * time))) * (float)Math.Sin((time * duration - speed) * (2 * pi) / period)) + start;
            else
                return (amplitude * (float)(Math.Pow(2, (-10 * time))) * (float)Math.Sin((time * duration - speed) * (2 * pi) / period) * .5f + change + start);
        }

        public static float ElasticEaseOutIn(float start, float finish, float currentTime, float duration)
        {
            if (currentTime > duration || duration == 0) { return finish; }
            var change = finish - start;
            if (currentTime < duration / 2)
                return ElasticEaseOut(0, change, currentTime * 2, duration) * .5f + start;
            else
                return ElasticEaseIn(0, change, currentTime * 2 - duration, duration) * .5f + (change * .5f) + start;
        }
        #endregion

        #region Overshoot
        public static float OvershootTween(float start, float finish, float currentTime, float duration)
        {
            return OvershootEaseInOut(start, finish, currentTime, duration);
        }

        public static float OvershootEaseIn(float start, float finish, float currentTime, float duration, float overshoot = 1.70158f)
        {
            if (currentTime > duration || duration == 0) { return finish; }
            var change = finish - start;
            var time = currentTime / duration;
            return change * time * time * ((overshoot + 1) * time - overshoot) + start;
        }

        public static float OvershootEaseOut(float start, float finish, float currentTime, float duration, float overshoot = 1.70158f)
        {
            if (currentTime > duration || duration == 0) { return finish; }
            var change = finish - start;
            var time = (currentTime / duration) - 1;
            return change * (time * time * ((overshoot + 1) * time + overshoot) + 1) + start;
        }

        public static float OvershootEaseInOut(float start, float finish, float currentTime, float duration, float overshoot = 1.70158f)
        {
            if (currentTime > duration || duration == 0) { return finish; }
            var change = finish - start;
            var time = currentTime / (duration / 2);
            overshoot = overshoot * 1.525f;
            if (time < 1)
            {
                return change / 2 * (time * time * ((overshoot + 1) * time - overshoot)) + start;
            }
            else
            {
                time = time - 2;
                return change / 2 * (time * time * ((overshoot + 1) * time + overshoot) + overshoot) + start;
            }
        }

        public static float OvershootEaseOutIn(float start, float finish, float currentTime, float duration, float overshoot = 1.70158f)
        {
            if (currentTime > duration || duration == 0) { return finish; }
            var change = finish - start;
            if (currentTime < duration / 2)
                return OvershootEaseOut(0, change, currentTime * 2, duration, overshoot) * .5f + start;
            else
                return OvershootEaseIn(0, change, currentTime * 2 - duration, duration, overshoot) * .5f + (change * .5f) + start;
        }
        #endregion

        #region Bounce
        public static float BounceTween(float start, float finish, float currentTime, float duration)
        {
            return BounceEaseInOut(start, finish, currentTime, duration);
        }

        public static float BounceEaseIn(float start, float finish, float currentTime, float duration)
        {
            if (currentTime > duration || duration == 0) { return finish; }
            var change = finish - start;
            return change - BounceEaseOut(0, change, duration - currentTime, duration) + start;
        }

        public static float BounceEaseOut(float start, float finish, float currentTime, float duration)
        {
            if (currentTime > duration || duration == 0) { return finish; }
            var change = finish - start;
            var time = currentTime / duration;
            if (time < (1 / 2.75))
            {
                return change * (7.5625f * time * time) + start;
            }
            else if (time < (2 / 2.75))
            {
                time = time - (1.5f / 2.75f);
                return change * (7.5625f * time * time + .75f) + start;
            }
            else if (time < (2.5 / 2.75))
            {
                time = time - (2.25f / 2.75f);
                return change * (7.5625f * time * time + .9375f) + start;
            }
            else
            {
                time = time - (2.625f / 2.75f);
                return change * (7.5625f * time * time + .984375f) + start;
            }
        }

        public static float BounceEaseInOut(float start, float finish, float currentTime, float duration)
        {
            if (currentTime > duration || duration == 0) { return finish; }
            var change = finish - start;
            if (currentTime < duration / 2)
                return BounceEaseIn(0, change, currentTime * 2, duration) * .5f + start;
            else
                return BounceEaseOut(0, change, currentTime * 2 - duration, duration) * .5f + (change * .5f) + start;
        }

        public static float BounceEaseOutIn(float start, float finish, float currentTime, float duration)
        {
            if (currentTime > duration || duration == 0) { return finish; }
            var change = finish - start;
            if (currentTime < duration / 2)
                return BounceEaseOut(0, change, currentTime * 2, duration) * .5f + start;
            else
                return BounceEaseIn(0, change, currentTime * 2 - duration, duration) * .5f + (change * .5f) + start;
        }
    }
}
        #endregion

