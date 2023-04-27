namespace ViazyNetCore.Model
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Member
    {

        /// <summary>
        /// 设置或获取一个值，表示主键。
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示账号。
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示姓名。
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示密码。
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示加盐值。
        /// </summary>
        public Guid Salt { get; set; }

        /// <summary>
        /// 绑定的手机号 可用于登陆
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 关联手机  不能用于登陆
        /// </summary>
        public string RelatedMobile { get; set; }

        [Column(IsIgnore = true)]
        public bool IsBindMobile
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Mobile))
                {
                    return !string.IsNullOrWhiteSpace(RelatedMobile);
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// 设置或获取一个值，表示创建时间。
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示最后登录时间。
        /// </summary>
        public DateTime LastLoginTime { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示状态（0禁用，1启用）。
        /// </summary>
        public ComStatus Status { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示扩展数据。
        /// </summary>
        public string Exdata { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示用户头像地址。
        /// </summary>
        public string AvatarUrl { get; set; }

    }
}
