using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ViazyNetCore.Model.Crm;
using ViazyNetCore.Modules.Models;

namespace ViazyNetCore.Modules.Mapper
{
    public class BusinessProfile : Profile
    {
        public BusinessProfile()
        {
            this.CreateMap<BusinessEditDto, CrmBusiness>();
            this.CreateMap<BusinessProductDto, CrmBusinessProduct>();
            this.CreateMap<CrmBusiness, BusinessRecordDto>();
        }
    }
}
