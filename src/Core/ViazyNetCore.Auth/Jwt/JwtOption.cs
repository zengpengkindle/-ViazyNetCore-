namespace ViazyNetCore.Auth.Jwt
{
    public class JwtOption
    {
        /// <summary>
        /// 签发者
        /// </summary>
        public string Issuer { get; set; } = "Viazy";

        /// <summary>
        /// 密钥
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// Token有效期（单位：秒）
        /// </summary>
        public int ExpiresIn { get; set; }

        /// <summary>
        /// 应用名，用于切分登录
        /// </summary>
        public string? AppName { get; set; }

        /// <summary>
        /// 启用分布式缓存验证
        /// </summary>
        public bool UseDistributedCache { get; set; }

        /// <summary>
        /// 是否允许多端登录
        /// </summary>
        public bool MultiPort { get; set; } = false;
    }
}
