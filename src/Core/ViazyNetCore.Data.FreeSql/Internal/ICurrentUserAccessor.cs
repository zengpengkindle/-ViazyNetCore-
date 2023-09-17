using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.MultiTenancy
{
    public interface ICurrentUserAccessor
    {
        IUser Current { get; set; }
    }
}
