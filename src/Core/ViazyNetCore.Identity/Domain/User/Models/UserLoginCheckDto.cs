using System;
using System.ComponentModel.DataAnnotations;

namespace ViazyNetCore.Dtos
{
    /// <summary>
    /// 表示用户登录的模型参数。
    /// </summary>
    public class UserLoginCheckDto
    {
        /// <summary>
        /// 设置或获取一个值，表示此账号未成功次数。
        /// </summary>
        public int ErrorCount { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示此账号起始禁用时间。
        /// </summary>
        public DateTime LastForbiddenTime { get; set; }
    }
}
