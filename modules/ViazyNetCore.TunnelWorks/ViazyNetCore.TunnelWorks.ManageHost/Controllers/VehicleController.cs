using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ViazyNetCore.TunnelWorks.ManageHost.Controllers
{
    [Route("api/vehicle")]
    public class VehicleController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly VehicleService _vehicleService;

        public VehicleController(IMapper mapper, VehicleService vehicleService)
        {
            this._mapper = mapper;
            this._vehicleService = vehicleService;
        }

        [HttpPost, Route("list")]
        public async Task<PageData<VehicleListItemResult>> GetPageData([FromBody] VehicleQueryRequest request, [FromQuery] PaginationSort pagination)
        {
            var queryDto = this._mapper.Map<VehicleQueryRequest, VehicleQueryDto>(request);
            var result = await this._vehicleService.GetPageListAsync(pagination, queryDto);
            return new PageData<VehicleListItemResult>()
            {
                Total = result.Total,
                Rows = this._mapper.Map<List<VehicleListItemDto>, List<VehicleListItemResult>>(result.Rows)
            };
        }

        [HttpPost, Route("edit")]
        public async Task EditAsync(VehicleEditRequest editRequest)
        {
            var editDto = this._mapper.Map<VehicleEditRequest, VehicleEditDto>(editRequest);
            await this._vehicleService.UpsertAsync(editDto);
        }
    }
}
