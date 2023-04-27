namespace ViazyNetCore.Authorization.Dtos
{
    /// <summary>
    /// 表示一个 用户管理使用Dto
    /// </summary>
    public class UserManageDto
    {
        /// <summary>
        /// 用户编码
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 用户账号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 随机密码
        /// </summary>
        public string Password { get; set; }
    }
}