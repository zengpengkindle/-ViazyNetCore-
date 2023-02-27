using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Auth.Authorization.Models
{
    public class BmsUserOrg
    {
        /// <summary>
        /// 设置或获取一个值，表示角色编号。
        /// </summary>
        [Required]
        public long OrgId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示页面编号。
        /// </summary>
        [Required]
        public long UserId { get; set; }

        /// <summary>
        /// 是否主管
        /// </summary>
        public bool IsManager { get; set; } = false;

        public DateTime CreateTime { get; set; }

        public DateTime ModifyTime { get; set; }
    }
}
