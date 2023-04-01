using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace ViazyNetCore.Manage.WebApi.Mapper
{
    public class ProdcutCatProfile : Profile
    {
        public ProdcutCatProfile()
        {
            CreateMap<CatEditReq, ProductCatAddDto>();
            CreateMap<CatEditReq, ProductCatUpdateDto>();
            CreateMap<ProductCatDto, CatRes>();
        }
    }
}
