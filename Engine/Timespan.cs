using System.Timers;

namespace Engine
{
    public class Timespan
    {
        private Timer _timer;
        private int _elapsed_milliseconds;

        public Timespan()
        {
            _elapsed_milliseconds = 0;
            _timer = new Timer(1);
            _timer.Start();
            _timer.Elapsed += (sender, e) => _elapsed_milliseconds += 1;
        }

        public void Mark(int mark_to = 0)
        {
            _elapsed_milliseconds = mark_to;
        }

        public int TotalMilliseconds
        {
            get 
            {
                return _elapsed_milliseconds;
            }
        }

        public int TotalSeconds
        {
            get
            {
                return _elapsed_milliseconds / 1000;
            }
        }

    }
}
