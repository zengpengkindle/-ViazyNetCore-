using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Model.Crm
{
    /// <summary>
    /// 客户表
    /// </summary>
    public class CrmCustomer : EntityUpdate
    {
        public string CustomerName { get; set; }
        /// <summary>
        /// 跟进状态 0未跟进1已跟进
        /// </summary>
        public int Followup { get; set; }
        /// <summary>
        /// 1锁定
        /// </summary>
        public int IsLock { get; set; }
        /// <summary>
        /// 下次联系时间
        /// </summary>
        public DateTime NextTime { get; set; }
        /// <summary>
        /// 成交状态 0未成交 1已成交
        /// </summary>
        public int DealStatus { get; set; }
        public string Mobile { get; set; }
        public string Telephone { get; set; }
        /// <summary>
        /// 网址
        /// </summary>
        public string Website { get; set; }
        public string Remark { get; set; }
        public long OwnerUserId { get; set; }
        /// <summary>
        /// 只读权限
        /// </summary>
        public long RoUserId { get; set; }
        /// <summary>
        /// 读写权限
        /// </summary>
        public long RwUserId { get; set; }
        /// <summary>
        /// 省市区
        /// </summary>
        public string Address { get; set; }
        public string Location { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string DetailAddress { get; set; }
        /// <summary>
        /// 地理位置经度
        /// </summary>
        public string Lng { get; set; }
        /// <summary>
        /// 地理位置维度
        /// </summary>
        public string Lat { get; set; }
        /// <summary>
        /// 批次 比如附件批次
        /// </summary>
        public string BatchId { get; set; }
        public string LastContent { get; set; }

    }
}
