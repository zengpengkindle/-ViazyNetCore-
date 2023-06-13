using System;
using System.Collections.Generic;
using System.Text;

namespace VizayNetCore.WebApi.Remote
{
    /// <summary>
    /// 表示一个远端接口配置。
    /// </summary>
    public class RemoteApiOptions
    {
        /// <summary>
        /// 获取或设置一个值，表示远端接口的配置名称。
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 获取或设置一个值，表示远端接口的基础地址。
        /// </summary>
        public string BaseAddress { get; set; }
        /// <summary>
        /// 获取或设置一个值，表示远端接口请求路径的后缀。
        /// </summary>
        public string ApiUrlSuffix { get; set; } = null;
        /// <summary>
        /// 获取或设置一个值，表示客户端调用超时毫秒数。
        /// <para>默认 5000 毫秒。</para>
        /// </summary>
        public int MillisecondsTimeout { get; set; }
            =
#if DEBUG
            100 * 1000
#else
            5000
#endif
            ;

        /// <summary>
        /// 获取或设置一个值，表示授权类型。
        /// <para>默认为 <see cref="RemoteGrantType.Secret"/> 。</para>
        /// </summary>
        public RemoteGrantType GrantType { get; set; } = RemoteGrantType.Secret;

        /// <summary>
        /// 获取或设置一个值，表示元数据。
        /// </summary>
        public Dictionary<string, string> Metadata { get; set; }

        /// <summary>
        /// 填充至完整的 URL 地址。
        /// </summary>
        /// <param name="suburls">子地址集合。</param>
        /// <returns>完整地址。</returns>
        public string FullUrl(params string[] suburls)
        {
            return GA.UrlCombine(this.BaseAddress, suburls);
        }

    }

    /// <summary>
    /// 表示远端接口授权类型。
    /// </summary>
    public enum RemoteGrantType
    {
        /// <summary>
        /// 表示安全码授权模式。
        /// </summary>
        Secret = 0,
    }
}
