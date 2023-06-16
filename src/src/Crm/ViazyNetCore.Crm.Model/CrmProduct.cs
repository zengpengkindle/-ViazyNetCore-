using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Crm.Model
{
    /// <summary>
    /// 产品表
    /// </summary>
    public class CrmProduct : EntityUpdate
    {
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 产品编码
        /// </summary>
        public string Num { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
        public decimal Price { get; set; }
        /// <summary>
        /// 状态 1 上架 0 下架 3 删除
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 产品分类ID
        /// </summary>
        public long CategoryId { get; set; }
        public string Description { get; set; }
        public long OwnerUserId { get; set; }
        public string BatchId { get; set; }
    }
}
