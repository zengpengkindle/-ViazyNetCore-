using System.Diagnostics;

namespace System
{
    /// <summary>
    /// 定义一个 Twitter-Snowflake 算法的分布式编号生成器的工厂。
    /// </summary>
    public interface ISnowflakeFactory
    {
        /// <summary>
        /// 创建基于业务实体类型的 Twitter-Snowflake 算法的分布式编号生成器。
        /// </summary>
        /// <typeparam name="TModel">业务实体类型。</typeparam>
        /// <returns>一个 <see cref="Snowflake"/> 的新实例。</returns>
        SnowflakeExtension.Snowflake Create<TModel>();
    }

    /// <summary>
    /// 表示一个默认的 Twitter-Snowflake 算法的分布式编号生成器的工厂。
    /// </summary>
    public class DefaultSnowflakeFactory : ISnowflakeFactory
    {
        /// <summary>
        /// 初始化一个 <see cref="DefaultSnowflakeFactory"/> 类的新实例。
        /// </summary>
        public DefaultSnowflakeFactory() { }
        /// <summary>
        /// 创建基于业务实体类型的 Twitter-Snowflake 算法的分布式编号生成器。
        /// </summary>
        /// <typeparam name="TModel">业务实体类型。</typeparam>
        /// <returns>一个 <see cref="Snowflake" /> 的新实例。</returns>
        public SnowflakeExtension.Snowflake Create<TModel>() => SnowflakeExtension.Snowflake.Create(SnowflakeExtension.Snowflake.GetLocalIPAddress()
#if NET5_0
            , Environment.ProcessId
#else
            , Process.GetCurrentProcess().Id
#endif
            );
    }


    /// <summary>
    /// 表示一个 Twitter-Snowflake 算法的分布式编号的统一管理器。
    /// </summary>
    /// <typeparam name="TModel">业务实体类型。</typeparam>
    public static class Snowflake<TModel>
    {
        private static long Seq = Math.Abs(Process.GetCurrentProcess().StartTime.Ticks << 1 >> 2 << 3 >> 4) / 10000L;
        private static readonly Lazy<SnowflakeExtension.Snowflake> Instance = new(new DefaultSnowflakeFactory().Create<TModel>);
        ///<summary>
        /// 获取下一个唯一编号。
        /// </summary>
        /// <returns>一个在当前数据中心和工作机器中的唯一编号。</returns>
        public static long NextId() => Instance.Value.NextId();

        /// <summary>
        /// 生成一个不可逆的十六位唯一字符串。
        /// </summary>
        /// <returns>一个在当前数据中心和工作机器中的唯一编号。</returns>
        public static string NextIdString() => Instance.Value.NextIdString() + (Interlocked.Increment(ref Seq) % 10000L).ToString("0000");
    }
}
