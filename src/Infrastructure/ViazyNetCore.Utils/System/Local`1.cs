namespace System.Threading
{
    /// <summary>
    /// 表示对于给定异步控制流（如异步方法）是本地数据的环境数据，与 <see cref="AsyncLocal{T}"/> 不同的是额外提供了环境数据初始化的功能。
    /// </summary>
    /// <typeparam name="T">环境数据的类型。</typeparam>
    public class Local<T>
    {
        private readonly Func<T> _valueFactory;
        private readonly AsyncLocal<T> _localValue = new AsyncLocal<T>();
        private readonly AsyncLocal<bool> _localStatus = new AsyncLocal<bool>();

        /// <summary>
        /// 使用默认实例初始化函数，初始化一个 <see cref="Local{T}"/> 类的新实例。
        /// </summary>
        public Local() : this(() => (T)Activator.CreateInstance(typeof(T), true)) { }

        /// <summary>
        /// 指定实例初始化函数，初始化一个 <see cref="Local{T}"/> 类的新实例。
        /// </summary>
        /// <param name="valueFactory">如果在 <see cref="Local{T}.Value"/> 之前尚未初始化的情况下尝试对其进行检索，则会调用 <see cref="Func{TResult}"/> 生成延迟初始化的值。</param>
        public Local(Func<T> valueFactory)
        {
            this._valueFactory = valueFactory ?? throw new ArgumentNullException(nameof(valueFactory));
        }

        /// <summary>
        /// 获取一个值，指示异步控制流（如异步方法）是否已初始化本地数据的环境数据。
        /// </summary>
        public bool IsValueCreated => this._localStatus.Value;

        /// <summary>
        /// 获取或设置环境数据的值。
        /// </summary>
        public T Value
        {
            get
            {
                if (!this._localStatus.Value)
                {
                    lock (this._localValue)
                        if (!this._localStatus.Value) this.SetValue(this._valueFactory());
                }

                return this._localValue.Value;
            }
            set { lock (this._localValue) this.SetValue(value); }
        }

        /// <summary>
        /// 重置当前环境数据的值。
        /// </summary>
        /// <param name="disposing">指示在重置操作之前是否尝试释放已创建的环境数据的值。</param>
        public void Reset(bool disposing = true)
        {
            if (this._localStatus.Value)
            {
                lock (this._localValue)
                {
                    if (this._localStatus.Value)
                    {
                        this._localStatus.Value = false;
                        if (disposing && this._localValue.Value is IDisposable value) value.Dispose();
                        this._localValue.Value = default;
                    }
                }
            }
        }

        private void SetValue(T value)
        {
            this._localValue.Value = value;
            this._localStatus.Value = true;
        }
    }
}
