using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql;

namespace ViazyNetCore.TunnelWorks.Modules.Repository
{
    [Injection]
    public interface IVehicleCategoryRepository : IBaseRepository<VehicleCategory, long>
    {

    }
}
