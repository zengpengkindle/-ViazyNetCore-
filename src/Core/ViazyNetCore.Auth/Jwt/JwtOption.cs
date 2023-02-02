namespace ViazyNetCore.Auth.Jwt
{
    public class JwtOption
    {
        /// <summary>
        /// 签发者
        /// </summary>
        public string Issuer { get; set; }

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
        public string AppName { get; set; }
    }
}
