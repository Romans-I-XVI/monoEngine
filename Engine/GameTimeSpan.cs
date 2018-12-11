using System;

namespace Engine
{
    public class GameTimeSpan
    {
        private DateTime _timestamp;

        public GameTimeSpan(bool is_pauseable = true)
        {
            if (is_pauseable)
                EntityManager.OnResume += (pause_time) => RemoveTime(pause_time);
            Mark();
        }
        public void Mark(float mark_to = 0)
        {
            _timestamp = DateTime.Now;
            _timestamp = _timestamp.AddMilliseconds(mark_to * -1);
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
