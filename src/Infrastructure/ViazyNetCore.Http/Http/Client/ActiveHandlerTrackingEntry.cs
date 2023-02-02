using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ViazyNetCore.Http
{
    internal class ActiveHandlerTrackingEntry
    {
        private static readonly TimerCallback _timerCallback = s => ((ActiveHandlerTrackingEntry)s).Timer_Tick();
        private readonly object _lock;
        private bool _timerInitialized;
        private Timer _timer;
        private TimerCallback _callback;


        public ActiveHandlerTrackingEntry(
            string name,
            LifetimeTrackingHttpMessageHandler handler,
            TimeSpan lifetime)
        {
            Name = name;
            Handler = handler;
            Lifetime = lifetime;

            _lock = new object();
        }

        public LifetimeTrackingHttpMessageHandler Handler { get; private set; }

        public TimeSpan Lifetime { get; }

        public string Name { get; }

        public void StartExpiryTimer(TimerCallback callback)
        {
            if (Lifetime == Timeout.InfiniteTimeSpan)
            {
                return; // never expires.
            }

            if (Volatile.Read(ref _timerInitialized))
            {
                return;
            }

            StartExpiryTimerSlow(callback);
        }


        private void StartExpiryTimerSlow(TimerCallback callback)
        {
            lock (_lock)
            {
                if (Volatile.Read(ref _timerInitialized))
                {
                    return;
                }

                _callback = callback;
                _timer = NonCapturingTimer.Create(_timerCallback, this, Lifetime, Timeout.InfiniteTimeSpan);
                _timerInitialized = true;
            }
        }

        private void Timer_Tick()
        {
            lock (_lock)
            {
                if (_timer != null)
                {
                    _timer.Dispose();
                    _timer = null;

                    _callback(this);
                }
            }
        }
    }
}
