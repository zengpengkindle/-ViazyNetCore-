using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Model.Crm
{
    /// <summary>
    /// 合同产品关系表
    /// </summary>
    public class CrmContractProduct : EntityUpdate
    {
        /// <summary>
        /// 合同ID
        /// </summary>
        public long ContractId { get; set; }
        /// <summary>
        /// 产品ID
        /// </summary>
        public long ProductId { get; set; }
        /// <summary>
        /// 产品单价
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 销售价格
        /// </summary>
        public decimal SalesPrice { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 折扣
        /// </summary>
        public decimal Discount { get; set; }
        /// <summary>
        /// 小计（折扣后价格）
        /// </summary>
        public decimal Subtotal { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
    }
}
