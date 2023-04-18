using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class SnowflakeAttribute : Attribute
    {
        public bool Enable { get; set; } = true;
    }
}
