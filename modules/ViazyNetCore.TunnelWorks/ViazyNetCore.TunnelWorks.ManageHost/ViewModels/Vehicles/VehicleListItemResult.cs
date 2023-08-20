using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.TunnelWorks.ManageHost.ViewModels
{
    public class VehicleListItemResult
    {
        public long Id { get; set; }

        /// <summary>
        /// 单件装备编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 机构编码
        /// </summary>
        public long OrgId { get; set; }

        public string OrgName { get; set; }

        /// <summary>
        /// 装备状态
        /// </summary>
        public VehicleStatus VehicleStatus { get; set; }

        /// <summary>
        /// 装备分类
        /// </summary>
        public long CatId { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string CatName { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public string Spec { get; set; }

        /// <summary>
        /// 生产企业
        /// </summary>
        public string OriginEnterprise { get; set; }

        /// <summary>
        /// 产地国别
        /// </summary>
        public string OriginPlace { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        public string LicenseCode { get; set; }

        /// <summary>
        /// 车架号码
        /// </summary>
        public string FrameCode { get; set; }

        /// <summary>
        /// 发动机号
        /// </summary>
        public string EngineCode { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
