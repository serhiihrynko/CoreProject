using System;
using System.Diagnostics;

namespace API.Infrastructure
{
    public class UptimeService
    {
        private Stopwatch _timer;
        private readonly DateTime _timeStarted;

        public UptimeService()
        {
            _timer = Stopwatch.StartNew();
            _timeStarted = DateTime.Now;
        }

        public TimeSpan Uptime => _timer.Elapsed;
        public DateTime TimeStarted => _timeStarted;
    }
}
