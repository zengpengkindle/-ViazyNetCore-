using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class DynamicApiAttribute : Attribute
    {
        /// <summary>
        /// Equivalent to AreaName
        /// </summary>
        public string? Area { get; set; }

        public int Order { get; set; } = 99;

        /// <summary>
        /// 分组名称列表
        /// </summary>
        public string[]? GroupNames { get; set; }

        public DynamicApiAttribute(string? area = null)
        {
            this.Area = area;
        }
    }
}
