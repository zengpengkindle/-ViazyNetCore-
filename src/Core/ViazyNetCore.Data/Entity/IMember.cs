using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore
{
    public interface IMember
    {
        /// <summary>
        /// 会员Id
        /// </summary>
        long? MemberId { get; set; }
    }
}
