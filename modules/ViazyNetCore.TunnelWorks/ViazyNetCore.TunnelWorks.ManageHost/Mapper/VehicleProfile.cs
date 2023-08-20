using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.TunnelWorks.ManageHost.Mapper
{
    public class VehicleProfile : Profile
    {
        public VehicleProfile()
        {
            CreateMap<VehicleEditRequest, VehicleEditDto>();
            CreateMap<VehicleQueryRequest, VehicleQueryDto>();
            CreateMap<VehicleListItemDto, VehicleListItemResult>();
        }
    }
}
