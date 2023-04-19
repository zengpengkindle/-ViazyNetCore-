using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.TaskScheduler
{
    public class QuartzReflectionItem
    {
        /// <summary>
        /// 命名空间
        /// </summary>
        public string NameSpace { get; set; }
        /// <summary>
        /// 类名
        /// </summary>
        public string NameClass { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }
}
