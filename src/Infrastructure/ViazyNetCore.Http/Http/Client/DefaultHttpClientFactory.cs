using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace ViazyNetCore.Http
{
    public class DefaultHttpClientFactory : IHttpClientFactory
    {
        private static readonly TimerCallback _cleanupCallback = (s) => ((DefaultHttpClientFactory)s).CleanupTimer_Tick();
        private readonly Func<EasyHttpProxy, Lazy<ActiveHandlerTrackingEntry>> _entryFactory;

        private readonly TimeSpan DefaultCleanupInterval = TimeSpan.FromSeconds(10);

        private Timer _cleanupTimer;
        private readonly object _cleanupTimerLock;
        private readonly object _cleanupActiveLock;

        internal readonly ConcurrentDictionary<EasyHttpProxy, Lazy<ActiveHandlerTrackingEntry>> _activeHandlers;

        // Collection of 'expired' but not yet disposed handlers.
        //
        // Used when we're rotating handlers so that we can dispose HttpMessageHandler instances once they
        // are eligible for garbage collection.
        //
        // internal for tests
        internal readonly ConcurrentQueue<ExpiredHandlerTrackingEntry> _expiredHandlers;
        private readonly TimerCallback _expiryCallback;

        public DefaultHttpClientFactory()
        {
            // case-sensitive because named options is.
            _activeHandlers = new ConcurrentDictionary<EasyHttpProxy, Lazy<ActiveHandlerTrackingEntry>>();
            _entryFactory = (proxy) =>
            {
                return new Lazy<ActiveHandlerTrackingEntry>(() =>
                    CreateHandlerEntry(proxy), LazyThreadSafetyMode.ExecutionAndPublication);
            };

            _expiredHandlers = new ConcurrentQueue<ExpiredHandlerTrackingEntry>();
            _expiryCallback = ExpiryTimer_Tick;

            _cleanupTimerLock = new object();
            _cleanupActiveLock = new object();
        }



        public HttpClient CreateClient(EasyHttpProxy proxy)
        {
            proxy ??= new EasyHttpProxy();

            HttpMessageHandler handler = CreateHandler(proxy);
            var client = new HttpClient(handler, disposeHandler: false);
            return client;
        }

        public HttpMessageHandler CreateHandler(EasyHttpProxy proxy)
        {
            if (proxy == null)
            {
                throw new ArgumentNullException(nameof(proxy));
            }

            ActiveHandlerTrackingEntry entry = _activeHandlers.GetOrAdd(proxy, _entryFactory).Value;

            StartHandlerEntryTimer(entry);

            return entry.Handler;
        }

        internal ActiveHandlerTrackingEntry CreateHandlerEntry(EasyHttpProxy proxy)
        {

            try
            {
                var httpHandler = new SocketsHttpHandler
                {
                    // Treat all headers as UTF8
                    RequestHeaderEncodingSelector = delegate { return Encoding.Latin1; },
                    UseCookies = false,
                    AllowAutoRedirect = false,
                    //PreAuthenticate = true,
                    Proxy = proxy?.Proxy,
                    AutomaticDecompression = DecompressionMethods.Brotli | DecompressionMethods.GZip |
                                             DecompressionMethods.Deflate,
                    ConnectTimeout = proxy.Lifetime
                };


                var handler = new LifetimeTrackingHttpMessageHandler(httpHandler);
                // this would happen, but we want to be sure.
                return new ActiveHandlerTrackingEntry(proxy, handler, proxy.Lifetime);
            }
            catch
            {
                throw;
            }
        }

        internal void ExpiryTimer_Tick(object state)
        {
            var active = (ActiveHandlerTrackingEntry)state;

            // The timer callback should be the only one removing from the active collection. If we can't find
            // our entry in the collection, then this is a bug.
            bool removed = _activeHandlers.TryRemove(active.Name, out Lazy<ActiveHandlerTrackingEntry> found);

            // At this point the handler is no longer 'active' and will not be handed out to any new clients.
            // However we haven't dropped our strong reference to the handler, so we can't yet determine if
            // there are still any other outstanding references (we know there is at least one).
            //
            // We use a different state object to track expired handlers. This allows any other thread that acquired
            // the 'active' entry to use it without safety problems.
            var expired = new ExpiredHandlerTrackingEntry(active);
            _expiredHandlers.Enqueue(expired);

            StartCleanupTimer();
        }

        internal virtual void StartHandlerEntryTimer(ActiveHandlerTrackingEntry entry)
        {
            entry.StartExpiryTimer(_expiryCallback);
        }

        internal virtual void StartCleanupTimer()
        {
            lock (_cleanupTimerLock)
            {
                _cleanupTimer ??=
                    NonCapturingTimer.Create(_cleanupCallback, this, DefaultCleanupInterval, Timeout.InfiniteTimeSpan);
            }
        }

        internal virtual void StopCleanupTimer()
        {
            lock (_cleanupTimerLock)
            {
                _cleanupTimer.Dispose();
                _cleanupTimer = null;
            }
        }

        internal void CleanupTimer_Tick()
        {
            // Stop any pending timers, we'll restart the timer if there's anything left to process after cleanup.
            //
            // With the scheme we're using it's possible we could end up with some redundant cleanup operations.
            // This is expected and fine.
            //
            // An alternative would be to take a lock during the whole cleanup process. This isn't ideal because it
            // would result in threads executing ExpiryTimer_Tick as they would need to block on cleanup to figure out
            // whether we need to start the timer.
            StopCleanupTimer();

            if (!Monitor.TryEnter(_cleanupActiveLock))
            {
                // We don't want to run a concurrent cleanup cycle. This can happen if the cleanup cycle takes
                // a long time for some reason. Since we're running user code inside Dispose, it's definitely
                // possible.
                //
                // If we end up in that position, just make sure the timer gets started again. It should be cheap
                // to run a 'no-op' cleanup.
                StartCleanupTimer();
                return;
            }

            try
            {
                int initialCount = _expiredHandlers.Count;


                int disposedCount = 0;
                for (int i = 0; i < initialCount; i++)
                {
                    // Since we're the only one removing from _expired, TryDequeue must always succeed.
                    _expiredHandlers.TryDequeue(out var entry);

                    if (entry.CanDispose)
                    {
                        try
                        {
                            entry.InnerHandler.Dispose();
                            disposedCount++;
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else
                    {
                        // If the entry is still live, put it back in the queue so we can process it
                        // during the next cleanup cycle.
                        _expiredHandlers.Enqueue(entry);
                    }
                }

            }
            finally
            {
                Monitor.Exit(_cleanupActiveLock);
            }

            // We didn't totally empty the cleanup queue, try again later.
            if (!_expiredHandlers.IsEmpty)
            {
                StartCleanupTimer();
            }
        }
    }
}
