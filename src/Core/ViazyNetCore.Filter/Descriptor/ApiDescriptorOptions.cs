using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.APIs
{
    /// <summary>
    /// API描述配置
    /// </summary>
    public class ApiDescriptorOptions
    {
        /// <summary>
        /// 缓存前缀
        /// </summary>
        [NotNull]
        public string CachePrefix { get; set; }
        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }
        /// <summary>
        /// 默认校验API接口权限
        /// </summary>
        public bool IsVerify { get; set; } = true;
    }
}
