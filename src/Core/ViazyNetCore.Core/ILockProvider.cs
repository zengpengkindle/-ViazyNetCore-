namespace ViazyNetCore
{
    /// <summary>
    /// 定义一个锁的提供程序。
    /// </summary>
    public interface ILockProvider
    {
        /// <summary>
        /// 锁定指定种子。
        /// </summary>
        /// <typeparam name="TSeed">种子的数据类型。</typeparam>
        /// <param name="seed">生成锁对象实例的种子，将采用默认的 <see cref="EqualityComparer{TSeed}"/> 匹配种子。</param>
        /// <returns>可解锁的对象。</returns>
        IDisposable Lock<TSeed>(TSeed seed) where TSeed : notnull;
    }

    /// <summary>
    /// 表示一个简单锁的提供程序。
    /// </summary>
    public class SimpleLockProvider : ILockProvider
    {
        /// <summary>
        /// 锁定指定种子。
        /// </summary>
        /// <typeparam name="TSeed">种子的数据类型。</typeparam>
        /// <param name="seed">生成锁对象实例的种子，将采用默认的 <see cref="EqualityComparer{TSeed}"/> 匹配种子。</param>
        /// <returns>可解锁的对象。</returns>
        public IDisposable Lock<TSeed>(TSeed seed) where TSeed : notnull => GA.Lock(seed);
    }
}
