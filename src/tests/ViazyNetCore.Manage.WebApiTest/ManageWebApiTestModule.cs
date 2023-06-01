using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ViazyNetCore.AspNetCore;
using ViazyNetCore.Authorization;
using ViazyNetCore.AutoMapper;
using ViazyNetCore.Identity;
using ViazyNetCore.Modules;

namespace ViazyNetCore.Manage.WebApiTest
{
    [DependsOn(typeof(AutoMapperModule)
        , typeof(AuthorizationModule)
        , typeof(IdentityModule)
        , typeof(ShopMallModulesModule)
        )]
    public class ManageWebApiTestModule : InjectionModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var services = context.Services;
            var configuration = services.GetConfiguration();

            var httpContextAccessor = new Mock<IHttpContextAccessor>();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
               {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToShortId()),
                     new Claim(JwtRegisteredClaimNames.Sid, "38"),//用户对象
                     new Claim(JwtRegisteredClaimNames.Aud, "app"),
                     new Claim(ClaimTypes.Name, "test"),
                     new Claim(JwtRegisteredClaimNames.Iss, "viazy netcor"),
                     new Claim(JwtRegisteredClaimNames.Typ,"Normal")
               }));
            var httpcontext = new DefaultHttpContext()
            {
                User = user
            };
            httpContextAccessor.Setup(a => a.HttpContext).Returns(httpcontext);

            services.AddSingleton(httpContextAccessor.Object);
        }

        public override void OnApplicationInitialization([NotNull] ApplicationInitializationContext context)
        {
        }
    }
}
