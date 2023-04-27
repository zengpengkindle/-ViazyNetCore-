using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore
{
    public interface IData
    {
        /// <summary>
        /// 拥有者Id
        /// </summary>
        long? OwnerId { get; set; }

        /// <summary>
        /// 拥有者部门Id
        /// </summary>
        long? OwnerOrgId { get; set; }
    }
}
