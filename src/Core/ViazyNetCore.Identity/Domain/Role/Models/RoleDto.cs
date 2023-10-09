using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Identity.Domain.Models
{
    public class RoleDto
    {
        public long Id { get; set; }
        /// <summary>
        /// 设置或获取一个值，表示名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示状态（-1删除，0禁用，1启用）。
        /// </summary>
        public ComStatus Status { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示扩展数据。
        /// </summary>
        public string ExtraData { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long TenantId { get; set; }
    }
}
