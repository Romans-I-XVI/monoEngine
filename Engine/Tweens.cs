using System;
namespace Engine
{
    public static class Tweens
    {

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
