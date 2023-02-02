using System.Collections.Generic;

namespace System.Web.DevServer
{
    /// <summary>
    /// 表示一个 Node 开发服务器的配置基类。
    /// </summary>
    public abstract class NodeServerOptionsBase
    {
        /// <summary>
        /// 获取或设置一个值，表示脚本名称。
        /// </summary>
        /// <value>默认为“dev”值。</value>
        public string ScriptName { get; set; } = "dev";

        /// <summary>
        /// 获取或设置一个值，表示环境变量。
        /// </summary>
        public IDictionary<string, string?>? Environment { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示 HTTP 的访问协议。
        /// </summary>
        /// <value>默认为“http”值。</value>
        public string Scheme { get; set; } = "http";

        /// <summary>
        /// 获取或设置一个值，表示 HTTP 的主机。
        /// </summary>
        /// <value>默认为“127.0.0.1”值。</value>
        public string Host { get; set; } = "127.0.0.1";

        /// <summary>
        /// 获取或设置一个值，表示 HTTP 的端口。
        /// </summary>
        /// <value>默认为“0”值，表示随机生成一个新的端口。</value>
        public int Port { get; set; } = 0;

        /// <summary>
        /// 生成脚本参数。
        /// </summary>
        /// <returns>参数。</returns>
        public abstract string GenerateArguments();
    }
}
