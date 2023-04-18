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
#if NET5_0 || NET6_0
            , Environment.ProcessId
#else
            , Process.GetCurrentProcess().Id
#endif
            );
    }
}
