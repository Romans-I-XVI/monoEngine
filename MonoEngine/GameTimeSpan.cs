using System;

namespace MonoEngine
{
    public class GameTimeSpan
    {
        private DateTime _timestamp;

        public GameTimeSpan()
        {
            Mark();
        }
        public void Mark(float markTo = 0)
        {
            _timestamp = DateTime.Now;
            _timestamp = _timestamp.AddMilliseconds(markTo * -1);
        }

        public float TotalMilliseconds
        {
            get
            {
                return (float)DateTime.Now.Subtract(_timestamp).TotalMilliseconds;
            }
        }

        public float TotalSeconds
        {
            get
            {
                return (float)DateTime.Now.Subtract(_timestamp).TotalSeconds;
            }
        }

        public void AddTime(float milliseconds)
        {
            _timestamp = _timestamp.AddMilliseconds(milliseconds * -1);
        }

        public void RemoveTime(float milliseconds)
        {
            _timestamp = _timestamp.AddMilliseconds(milliseconds);
        }

    }
}
