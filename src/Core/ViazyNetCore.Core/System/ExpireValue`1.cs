namespace System
{
    /// <summary>
    /// 表示一个可过期的数据。
    /// </summary>
    /// <typeparam name="T">数据类型。</typeparam>
    public class ExpireValue<T>
    {
        private readonly Threading.SemaphoreSlim _semaphoreSlim = new(1, 1);
        private T? _value;
        private readonly Func<T> _valueFactory;

        /// <summary>
        /// 获取一个值，如果值为 <see langword="null"/> 值或已过期，则重新生成新的值。
        /// </summary>
        public T? Value
        {
            get
            {
                if(this.IsExpired)
                {
                    try
                    {
                        this._semaphoreSlim.Wait();
                        if(this.IsExpired)
                        {
                            if(this._value is IDisposable value) value.Dispose();
                            this._value = this._valueFactory();
                            this.LastCreateTime = GA.Now;
                        }
                    }
                    finally
                    {
                        this._semaphoreSlim.Release();
                    }
                }
                return this._value;
            }
        }

        /// <summary>
        /// 获取一个值，表示当前数据是否已过期。
        /// </summary>
        public bool IsExpired => this._value is null || this.LastCreateTime.Add(this.ExpireTime) <= GA.Now;

        /// <summary>
        /// 获取一个值，表示过期的时间。
        /// </summary>
        public TimeSpan ExpireTime { get; }

        /// <summary>
        /// 获取一个值，表示最后创建数据的时间。
        /// </summary>
        public DateTime LastCreateTime { get; private set; }

        /// <summary>
        /// 初始化一个 <see cref="ExpireValue{T}"/> 类的新实例。
        /// </summary>
        /// <param name="expireTime">过期的时间。</param>
        /// <param name="valueFactory">数据的值创建委托。</param>
        public ExpireValue(TimeSpan expireTime, Func<T> valueFactory)
        {
            this._valueFactory = valueFactory ?? throw new ArgumentNullException(nameof(valueFactory));
            this.ExpireTime = expireTime;
        }

        /// <summary>
        /// 重置当前数据。
        /// </summary>
        public void Reset()
        {
            this._value = default;

        }
    }
}
