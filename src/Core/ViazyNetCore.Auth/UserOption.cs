using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Auth
{
    public class UserOption
    {
        /// <summary>
        /// 注册类型分为 邮箱注册和手机注册(默认邮箱注册)
        /// </summary>
        public RegisterType RegisterType { get; set; } = RegisterType.Email;

        /// <summary>
        /// 用户名最短长度
        /// </summary>
        public int MinUserNameLength { get; set; } = 2;

        /// <summary>
        /// 用户名的最大长度
        /// </summary>
        public int MaxUserNameLength { get; set; } = 20;

        /// <summary>
        /// 用户名验证正则表达式
        /// </summary>
        public string UserNameRegex { get; set; } = @"^[\w|\u4e00-\u9fa5]{1,64}$";

        /// <summary>
        /// 手机号验证正则表达式
        /// </summary>
        public string PhoneRegex { get; set; } = @"^(13|14|15|17|18)[0-9]{9}$";

        /// <summary>
        /// 昵称的正则
        /// </summary>
        public string NickNameRegex { get; set; } = @"^[\w|\u4e00-\u9fa5]{1,64}$";

        /// <summary>
        /// 密码最小长度
        /// </summary>
        public int MinPasswordLength { get; set; } = 6;
        /// <summary>
        /// 密码中包含的最少特殊字符数
        /// </summary>
        public int MinRequiredNonAlphanumericCharacters { get; set; } = 0;

        /// <summary>
        /// Email验证正则表达式
        /// </summary>
        public string EmailRegex { get; set; } = @"^([a-zA-Z0-9_.-]+)@([0-9A-Za-z.-]+).([a-zA-Z.]{2,6})$";

        /// <summary>
        /// 是否启用匿名用户跟踪
        /// </summary>
        public bool EnableTrackAnonymous { get; set; } = true;

        /// <summary>
        /// 指定用户在最近一次活动时间之后多长时间视为在线的分钟数
        /// </summary>
        public int UserOnlineTimeWindow { get; set; } = 20;

        /// <summary>
        /// 允许未激活的用户登录
        /// </summary>
        public bool EnableNotActivatedUsersToLogin { get; set; } = false;

        /// <summary>
        /// 用户注册时是否允许手机号重复
        /// </summary>
        public bool RequiresUniqueMobile { get; set; } = false;

        /// <summary>
        /// 用户密码加密方式
        /// </summary>
        public UserPasswordFormat UserPasswordFormat { get; set; } = UserPasswordFormat.MD5;

        /// <summary>
        /// 是否启用昵称
        /// </summary>
        public bool EnableNickname { get; set; } = true;

        /// <summary>
        /// 是否启用电话
        /// </summary>
        public bool EnablePhone { get; set; } = true;

        /// <summary>
        /// 新注册用户是否自动处于管制状态
        /// </summary>
        public bool AutomaticModerated { get; set; } = false;

        ///	<summary>
        ///	用户自动接触管制状态所需的积分（用户综合积分）
        ///	</summary>
        public int NoModeratedUserPoint { get; set; } = 100;

        /// <summary>
        /// 不允许使用的用户名
        /// </summary>
        /// <remarks>
        /// 多个用户名之间用逗号分割
        /// </remarks>
        public string DisallowedUserNames { get; set; } = "administrator,super,admin";

        /// <summary>
        /// 超级管理员角色Id
        /// </summary>
        public long SuperAdministratorRoleId { get; set; } = 101;

        /// <summary>
        /// 匿名用户角色Id
        /// </summary>
        public long AnonymousRoleId { get; set; } = 122;

        /// <summary>
        /// 是否启用人工审核
        /// </summary>
        public bool EnableAudit { get; set; } = true;

        /// <summary>
        /// 最小不需要审核的用户等级
        /// </summary>
        public int MinNoAuditedUserRank { get; set; } = 18;

        public string GetRandomPassword => FastRandom.Instance.NextString(8);
    }

    public enum RegisterType
    {
        /// <summary>
        /// 手机注册
        /// </summary>
        Mobile = 10,

        /// <summary>
        /// 邮箱注册
        /// </summary>
        Email = 20
    }
}
