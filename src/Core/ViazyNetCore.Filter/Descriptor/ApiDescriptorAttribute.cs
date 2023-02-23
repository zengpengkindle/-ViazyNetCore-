using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore
{
    /// <summary>
    /// Api接口描述
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiDescriptorAttribute : Attribute
    {
        /// <summary>
        /// API备注
        /// </summary>
        public string Descriptor { get; }
        /// <summary>
        /// API显示名称
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// 初始化构造函数
        /// </summary>
        /// <param name="displayName">显示名称</param>
        public ApiDescriptorAttribute([NotNull] string displayName)
        {
            this.DisplayName = displayName;
        }

        /// <summary>
        /// 初始化构造函数
        /// </summary>
        /// <param name="displayName">显示名称</param>
        /// <param name="descriptor">描述</param>
        public ApiDescriptorAttribute([NotNull] string displayName, string descriptor)
        {
            DisplayName = displayName;
            Descriptor = descriptor;
        }
    }
}
