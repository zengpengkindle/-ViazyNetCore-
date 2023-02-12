using System;

namespace ViazyNetCore.Dtos
{
    /// <summary>
    /// 表示一个所有启用角色的查询参数。
    /// </summary>
    public class RoleFindAllEnabledArgs : PaginationSort
    {
        /// <summary>
        /// 获取或设置一个值，表示角色名称通配符。
        /// </summary>
        public string? NameLike { get; set; }
    }
    
}