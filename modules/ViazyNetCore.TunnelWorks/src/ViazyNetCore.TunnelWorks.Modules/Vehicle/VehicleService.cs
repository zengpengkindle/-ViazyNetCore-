using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.TunnelWorks.Modules
{
    [Injection]
    public class VehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IMapper _mapper;

        public VehicleService(IVehicleRepository vehicleRepository, IMapper mapper)
        {
            this._vehicleRepository = vehicleRepository;
            this._mapper = mapper;
        }

        public async Task UpsertAsync(VehicleEditDto editDto)
        {
            var entity = this._mapper.Map<VehicleEditDto, Vehicle>(editDto);
            await this._vehicleRepository.UpsertAsync(entity);
        }

        public async Task<PageData<VehicleListItemDto>> GetPageListAsync(PaginationSort pagination, VehicleQueryDto queryDto)
        {
            return await this._vehicleRepository.PageListAsync(pagination, queryDto);
        }

        public Task<VehicleInfoDto> GetInfoAsync(long id)
        {
            return this._vehicleRepository.GetInfoAsync(id);
        }
    }
}
