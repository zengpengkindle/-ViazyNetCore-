using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql;

namespace ViazyNetCore.TunnelWorks.Modules.Repository
{
    [Injection]
    public interface IVehicleRepository : IBaseRepository<Vehicle, long>
    {
        Task<VehicleInfoDto> GetInfoAsync(long id);
        Task<PageData<VehicleListItemDto>> PageListAsync(PaginationSort pagination, VehicleQueryDto queryDto);
        Task UpsertAsync(Vehicle enitity);
    }
}
