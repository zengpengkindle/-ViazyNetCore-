using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace ViazyNetCore.Modules.Mapper
{
    public class ProductCatProfile : Profile
    {
        public ProductCatProfile()
        {
            CreateMap<ProductCat, ProductCatDto>();
            CreateMap<ProductCatAddDto, ProductCat>();
            CreateMap<ProductCat, ProductCatDto>();
            CreateMap<ProductCatUpdateDto, ProductCat>();
        }
    }
}
