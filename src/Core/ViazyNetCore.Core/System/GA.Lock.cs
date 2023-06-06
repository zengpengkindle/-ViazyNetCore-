using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public partial class GA
    {
        #region Lock

        /// <summary>
        /// 获取或设置默认的锁超时间隔（默认15秒）。
        /// </summary>
#pragma warning disable CA2211 // 非常量字段应当不可见
        public static TimeSpan DefaultLockTimeout = TimeSpan.FromSeconds(15);
#pragma warning restore CA2211 // 非常量字段应当不可见

        public static DateTime Now => DateTime.Now;

        internal static void TimeoutError(string key, TimeSpan timeout)
        {
            throw new TimeoutException($"The key '{key}' lock timeout {timeout}.");
        }

        internal class SimpleLockItem : IDisposable
        {
            private readonly Action _disposing;
            private bool _isDisposed = false;
            public SimpleLockItem(Action disposing) => this._disposing = disposing;
            public void Dispose()
            {
                lock (this)
                {
                    if (this._isDisposed) return;
                    this._isDisposed = true;
                    this._disposing();
                }
            }
            ~SimpleLockItem()
            {
                this.Dispose();
            }
        }

        /// <summary>
        /// 采用默认的超时时间（15秒），锁定指定种子。
        /// </summary>
        /// <typeparam name="TSeed">种子的数据类型。</typeparam>
        /// <param name="seed">生成锁对象实例的种子，将采用默认的 <see cref="EqualityComparer{TSeed}"/> 匹配种子。</param>
        /// <returns>可解锁的对象。</returns>
        public static IDisposable Lock<TSeed>(TSeed seed) where TSeed : notnull
        {
            return Lock(seed, DefaultLockTimeout);
        }

        /// <summary>
        /// 给定超时时间，锁定指定种子。
        /// </summary>
        /// <typeparam name="TSeed">种子的数据类型。</typeparam>
        /// <param name="seed">生成锁对象实例的种子，将采用默认的 <see cref="EqualityComparer{TSeed}"/> 匹配种子。</param>
        /// <param name="timeout">锁的超时时间。</param>
        /// <returns>可解锁的对象。</returns>
        public static IDisposable Lock<TSeed>(TSeed seed, TimeSpan timeout) where TSeed : notnull
        {
            if (Equals(seed, default(TSeed))) throw new ArgumentNullException(nameof(seed));

            var s = Seed<TSeed>.LockeableObjects.GetOrAdd(seed, key => new Seed<TSeed>(key));
            return s.LockSeed(timeout);
        }

        /// <summary>
        /// 采用默认的超时时间（15秒），异步锁定指定种子。
        /// </summary>
        /// <typeparam name="TSeed">种子的数据类型。</typeparam>
        /// <param name="seed">生成锁对象实例的种子，将采用默认的 <see cref="EqualityComparer{TSeed}"/> 匹配种子。</param>
        /// <returns>可解锁的异步操作。</returns>
        public static Task<IDisposable> LockAsync<TSeed>(TSeed seed) where TSeed : notnull
        {
            return LockAsync(seed, DefaultLockTimeout);
        }

        /// <summary>
        /// 给定超时时间，异步锁定指定种子。
        /// </summary>
        /// <typeparam name="TSeed">种子的数据类型。</typeparam>
        /// <param name="seed">生成锁对象实例的种子，将采用默认的 <see cref="EqualityComparer{TSeed}"/> 匹配种子。</param>
        /// <param name="timeout">锁的超时时间。</param>
        /// <returns>可解锁的异步操作。</returns>
        public static Task<IDisposable> LockAsync<TSeed>(TSeed seed, TimeSpan timeout) where TSeed : notnull
        {
            if (Equals(seed, default(TSeed))) throw new ArgumentNullException(nameof(seed));

            var s = Seed<TSeed>.LockeableObjects.GetOrAdd(seed, key => new Seed<TSeed>(key));
            return s.LockSeedAsync(timeout);
        }

        class Seed<TKey> where TKey : notnull
        {
            public readonly static ConcurrentDictionary<TKey, Seed<TKey>> LockeableObjects
                = new ConcurrentDictionary<TKey, Seed<TKey>>();


            private TKey _key;
            private SemaphoreSlim _sema;
            private int _length;

            public Seed(TKey key)
            {
                this._key = key;
                this._sema = new SemaphoreSlim(1, 1);
            }

            IDisposable GetDisposable()
            {
                return new SimpleLockItem(() =>
                {
                    this._sema.Release();
                    if (Interlocked.Decrement(ref this._length) == 0) LockeableObjects.TryRemove(this._key, out var s);
                });
            }

            public IDisposable LockSeed(TimeSpan timeout)
            {
                Interlocked.Increment(ref this._length);
                if (!this._sema.Wait(timeout)) TimeoutError(Convert.ToString(this._key)!, timeout);
                return this.GetDisposable();
            }

            public async Task<IDisposable> LockSeedAsync(TimeSpan timeout)
            {
                Interlocked.Increment(ref this._length);
                if (!await this._sema.WaitAsync(timeout)) TimeoutError(Convert.ToString(this._key)!, timeout);
                return this.GetDisposable();
            }

            ~Seed()
            {
                this._sema.Dispose();
                this._sema = null;
                this._key = default;
            }
        }

        #endregion

        /// <summary>
        /// 线程等待指定时间。
        /// </summary>
        /// <param name="seconds">等待的秒数。</param>
        public static void Wait(int seconds)
        {
            Wait(TimeSpan.FromSeconds(seconds));
        }
        /// <summary>
        /// 线程等待指定时间。
        /// </summary>
        /// <param name="timeSpan">等待的时间。</param>
        public static void Wait(TimeSpan timeSpan)
        {
            SpinWait.SpinUntil(() => false, timeSpan);
        }
    }
}
