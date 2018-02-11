using System;

namespace Engine
{
    public class GameTimeSpan
    {
        private DateTime _timestamp;

        public GameTimeSpan()
        {
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

    }
}
