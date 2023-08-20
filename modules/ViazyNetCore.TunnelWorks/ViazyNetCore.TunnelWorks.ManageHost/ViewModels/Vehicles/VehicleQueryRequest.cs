using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.TunnelWorks.ManageHost.ViewModels
{
    public class VehicleQueryRequest
    {
        /// <summary>
        /// 分类
        /// </summary>
        public long CatId { get; set; }

        /// <summary>
        /// 机构
        /// </summary>
        public long OrgId { get; set; }

        /// <summary>
        /// 单件装备编码
        /// </summary>
        public string Code { get; set; }

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
    }
}
