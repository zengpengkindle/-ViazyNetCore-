using System;

namespace ViazyNetCore.Dtos
{
    /// <summary>
    /// 表示一个用户的查询参数。
    /// </summary>
    public class UserFindQueryDto : PaginationSort
    {
        /// <summary>
        /// 获取或设置一个值，表示用户账号通配符。
        /// </summary>
        public string? UsernameLike { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示用户角色。
        /// </summary>
        public string? RoleId { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示状态，为空表示查询所有。
        /// </summary>
        public ComStatus? Status { get; set; }

        public long? OrgId { get; set; }
    }
}