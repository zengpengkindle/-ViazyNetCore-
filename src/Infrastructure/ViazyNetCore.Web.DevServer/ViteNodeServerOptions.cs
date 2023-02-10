namespace System.Web.DevServer
{
    /// <summary>
    /// 表示一个基于 Vite 的 Node 开发服务器的配置。
    /// </summary>
    public class ViteNodeServerOptions : NodeServerOptionsBase
    {
        /// <inheritdoc />
        public override string GenerateArguments()
        {
            return FormattableString.Invariant($" --clearScreen false --host {this.Host} --port {this.Port} --strictPort true");
        }
    }
}
