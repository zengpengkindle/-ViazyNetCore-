using System.ComponentModel.DataAnnotations;

namespace ViazyNetCore.Dtos
{
    /// <summary>
    /// 表示用户登录的模型参数。
    /// </summary>
    public class UserLoginArgs
    {
        /// <summary>
        /// 设置或获取一个值，表示用户账号。
        /// </summary>
        [MaxLength(50), Required]
        public string Username { get; set; }
        /// <summary>
        /// 设置或获取一个值，表示旧的密码。
        /// </summary>
        [MaxLength(32), Required]
        public string Password { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示PIN
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示登录标记（用于用户多次登录使用）。
        /// </summary>
        [MaxLength(32)]
        public string? Mark { get; set; }

        /// <summary>
        /// 审核用户的userid
        /// </summary>
        public long? Auditor { get; set; }

        ///// <summary>
        ////api上下文，用于记录日志
        ///// </summary>
        //public string Ip { get; set; }
    }
}
