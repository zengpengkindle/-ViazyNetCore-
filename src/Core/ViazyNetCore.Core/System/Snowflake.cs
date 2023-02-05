using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.SnowflakeExtension;

namespace System
{
    public static class Snowflake
    {
        private static long Seq = Process.GetCurrentProcess().StartTime.ConvertToJsTime();
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
}
