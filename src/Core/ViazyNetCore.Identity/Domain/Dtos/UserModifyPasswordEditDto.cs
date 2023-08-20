using System.ComponentModel.DataAnnotations;

namespace ViazyNetCore.Dtos
{
    /// <summary>
    /// 表示一个用户修改密码的参数。
    /// </summary>
    public class UserModifyPasswordEditDto
    {
        /// <summary>
        /// 设置或获取一个值，表示旧的密码。
        /// </summary>
        [MaxLength(32), Required]
        public string OldPassword { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示新的密码。
        /// </summary>
        [MaxLength(32), Required]
        public string NewPassword { get; set; }
    }
}