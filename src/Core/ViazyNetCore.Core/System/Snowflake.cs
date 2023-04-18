using System.Diagnostics;
using System.Net;

namespace System
{
    public static class Snowflake
    {
        private static long Seq = Math.Abs(Process.GetCurrentProcess().StartTime.Ticks << 1 >> 2 << 3 >> 4) / 10000L;
        private static readonly IPAddress LocalIPAddress = SnowflakeExtension.Snowflake.GetLocalIPAddress();
        private static readonly Lazy<SnowflakeExtension.Snowflake> Instance = new Lazy<SnowflakeExtension.Snowflake>(() => SnowflakeExtension.Snowflake.Create(LocalIPAddress, Process.GetCurrentProcess().Id));//ObjectFactory.Global.Get<ISnowflakeFactory>().Create<TModel>());

        /// <summary>
        /// 获取下一个唯一编号。
        /// </summary>
        /// <returns>一个在当前数据中心和工作机器中的唯一编号。</returns>
        public static long NextId() => Instance.Value.NextId();

        /// <summary>
        /// 生成一个不可逆的十六位唯一字符串。
        /// </summary>
        /// <returns>一个在当前数据中心和工作机器中的唯一编号。</returns>
        public static string NextIdString() => Instance.Value.NextIdString() + (System.Threading.Interlocked.Increment(ref Seq) % 10000L).ToString("0000");
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
