using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.CmsKit.Models
{
    public class BmsPermissionApi : EntityAdd
    {
        /// <summary>
        /// 设置或获取一个值，表示名称。
        /// </summary>
        public string PermissionItemKey { get; set; }

        /// <summary>
        /// 请求地址
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 请求方法
        /// </summary>
        public string HttpMethod { get; set; }
    }
}
