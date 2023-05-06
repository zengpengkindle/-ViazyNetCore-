using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql.DataAnnotations;

namespace ViazyNetCore.Authorization.Models
{
    [Index("idx_{tablename}_01", nameof(ParentId) + "," + nameof(Name) + "," + nameof(TenantId), true)]
    public class BmsOrg : EntityTenantBase
    {
        /// <summary>
        /// 父级
        /// </summary>
        public long ParentId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Column(StringLength = 50)]
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Column(StringLength = 50)]
        public string Code { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        [Column(StringLength = 50)]
        public string Value { get; set; }

        /// <summary>
        /// 成员数
        /// </summary>
        public int MemberCount { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
        public ComStatus Status { get; set; }

        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Column(StringLength = 500)]
        public string Description { get; set; }

        /// <summary>
        /// 子级列表
        /// </summary>
        [Navigate(nameof(ParentId))]
        public List<BmsOrg> Childs { get; set; }
    }
}
