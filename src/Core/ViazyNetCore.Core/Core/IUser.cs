using System.ComponentModel;

namespace ViazyNetCore
{
    public interface IUser<TKey>
    {
        public TKey Id { get; }

        public string Username { get; }
        public string Nickname { get; }

        public AuthUserType IdentityType { get; }
        /// <summary>
        /// 是否被管制
        /// </summary>
        bool IsModerated { get; }
    }

    public interface IUser : IUser<long>
    { }


    public enum AuthUserType
    {
        Unknown=0,
        /// <summary>
        /// 会员
        /// </summary>
        Member = 2,
        /// <summary>
        /// 普通用户
        /// </summary>
        [Description("普通用户")]
        Normal = 1,
        /// <summary>
        /// 管理员
        /// </summary>
        [Description("管理员")]
        PlatformAdmin = 99
    }
}
