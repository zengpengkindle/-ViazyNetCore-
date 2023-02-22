using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
    public class ControllerGroupAttribute : Attribute
    {
        public bool NonGroup { get; set; }

        /// <summary>
        /// 分组名称列表
        /// </summary>
        public string[] GroupNames { get; set; }

        public ControllerGroupAttribute(params string[] groupNames)
        {
            GroupNames = groupNames;
        }
    }
}
