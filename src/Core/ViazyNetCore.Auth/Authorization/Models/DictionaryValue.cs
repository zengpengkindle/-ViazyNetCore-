using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql.DataAnnotations;

namespace ViazyNetCore.Authorization.Models
{
    /// <summary>
    /// 数据字典
    /// </summary>
    public class DictionaryValue : EntityBase
    {

        /// <summary>
        /// 字典名称
        /// </summary>
        [Column(StringLength = 50)]
        public string Name { get; set; }
        public long DictionaryTypeId { get; set; }

        /// <summary>
        /// 字典编码
        /// </summary>
        [Column(StringLength = 50)]
        public string Code { get; set; }

        public ComStatus Status { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Column(StringLength = 500)]
        public string Description { get; set; }


        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }
}
