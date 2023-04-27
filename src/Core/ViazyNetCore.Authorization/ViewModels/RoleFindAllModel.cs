using System;
using ViazyNetCore.Authorization;

namespace ViazyNetCore.Dtos
{
    /// <summary>
    /// 表示一个角色的查询模型。
    /// </summary>
    public class RoleFindAllModel : RoleSimpleModel
    {
        /// <summary>
        /// 设置或获取一个值，表示状态（0禁用，1启用，-1删除）。
        /// </summary>
        public ComStatus Status { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示创建时间。
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示修改时间。
        /// </summary>
        public DateTime? UpdateTime { get; set; }
    }
}