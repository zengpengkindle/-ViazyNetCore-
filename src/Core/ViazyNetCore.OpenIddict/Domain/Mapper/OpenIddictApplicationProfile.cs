using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace ViazyNetCore.OpenIddict.Domain.Mapper
{
    public class OpenIddictApplicationProfile : Profile
    {
        public OpenIddictApplicationProfile()
        {
            CreateMap<OpenIddictApplication, OpenIddictApplicationDto>().ReverseMap();
            CreateMap<OpenIddictAuthorization, OpenIddictAuthorizationDto>().ReverseMap();
            CreateMap<OpenIddictScope, OpenIddictScopeDto>().ReverseMap();
            CreateMap<OpenIddictToken, OpenIddictTokenDto>().ReverseMap();
        }
    }
}
