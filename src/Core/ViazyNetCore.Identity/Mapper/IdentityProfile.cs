using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ViazyNetCore.Authorization.Models;

namespace ViazyNetCore.OpenIddict.Domain.Mapper
{
    public class IdentityProfile:Profile
    {
        public IdentityProfile()
        {
            CreateMap<BmsUser, IdentityUser>();
        }
    }
}
