using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

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
