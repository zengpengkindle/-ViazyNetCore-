using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore
{
    /// <summary>
    /// 表示一个请求方法的注释。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ApiTitleAttribute : Attribute
    {
        public ApiTitleAttribute(string description)
        {
            this.Description = description;
        }

        public string Description { get; }
    }
}
