using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ViazyNetCore.Authorization.Models;
using ViazyNetCore.Identity.Domain;

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
