using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Model.Crm
{
    /// <summary>
    /// 表示一个商机
    /// </summary>
    public class CrmBusiness : EntityUpdate
    {
        /// <summary>
        /// 商机状态组
        /// </summary>
        public int TypeId { get; set; }
        /// <summary>
        /// 销售阶段
        /// </summary>
        public int StatusId { get; set; }

        /// <summary>
        /// 下次联系时间
        /// </summary>
        public DateTime? NextTime { get; set; }

        public long CustomerId { get; set; }

        /// <summary>
        /// 预计成交日期
        /// </summary>
        public DateTime DealTime { get; set; }

        /// <summary>
        /// 商机名称
        /// </summary>
        public string BusinessName { get; set; }

        /// <summary>
        /// 商机金额
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// 整单折扣
        /// </summary>
        public decimal DiscountRate { get; set; }

        /// <summary>
        /// 产品总金额
        /// </summary>
        public decimal TotalPrice { get; set; }

        public string Remark { get; set; }

        /// <summary>
        /// 负责人ID
        /// </summary>
        public long OwnerUserId { get; set; }

        /// <summary>
        /// 批次 比如附件批次
        /// </summary>
        public long BatchId { get; set; }

        /// <summary>
        /// 1赢单2输单3无效
        /// </summary>
        public bool IsEnd { get; set; }

        /// <summary>
        /// 只读权限
        /// </summary>
        public long RoUserId { get; set; }

        /// <summary>
        /// 读写权限
        /// </summary>
        public long RwUserId { get; set; }

        public string StatusRemark { get; set; }
    }
}
