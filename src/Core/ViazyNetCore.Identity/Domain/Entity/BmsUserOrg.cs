using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Authorization.Models
{
    public class BmsUserOrg : Entity<long>, ITenant
    {
        /// <summary>
        /// 设置或获取一个值，表示部门编号。
        /// </summary>
        [Required]
        public long OrgId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示用户编号。
        /// </summary>
        [Required]
        public long UserId { get; set; }

        /// <summary>
        /// 是否主管
        /// </summary>
        public bool IsManager { get; set; } = false;

        public DateTime CreateTime { get; set; }

        public DateTime ModifyTime { get; set; }
        public long TenantId { get; set; }
    }
}
