using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ViazyNetCore.Identity.Domain;
using ViazyNetCore.Modules;
using IdentityUser = ViazyNetCore.Identity.Domain.IdentityUser;

namespace ViazyNetCore.IdentityService4.FreeSql
{
    public class DefaultProfileService : IProfileService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<DefaultProfileService> _logger;

        public DefaultProfileService(IdentityUserManager userManager
            , IHttpContextAccessor httpContextAccessor
            , ILogger<DefaultProfileService> logger)
        {
            this._userManager = userManager;
            this._httpContextAccessor = httpContextAccessor;
            this._logger = logger;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            _logger.LogDebug("验证用户是否有效：{caller}", context.Caller);

            var sub = context.Subject?.GetSubjectId();
            if(sub == null)
                throw new Exception("获取用户信息失败，用户Id为空");

            var user = await _userManager.FindByIdAsync(sub);

            List<string> requestClaims = RequestClaims(context)?.Distinct().ToList();
            if(requestClaims == null || requestClaims.Count == 0)
                return;
            List<Claim> claims = new List<Claim>();
            foreach(var item in requestClaims)
            {
                switch(item)
                {
                    case JwtClaimTypes.Id:
                    case JwtClaimTypes.Subject:
                        {
                            claims.Add(new Claim(item, user.Id.ToString()));
                        }
                        break;
                    case JwtClaimTypes.Name:
                        {
                            claims.Add(new Claim(item, user.Username));
                        }
                        break;
                    case JwtClaimTypes.NickName:
                        {
                            claims.Add(new Claim(item, user.Nickname));
                        }
                        break;
                    case JwtClaimTypes.Role:
                        {
                            claims.Add(new Claim(item, user.Roles.Join()));
                        }
                        break;
                    case JwtClaimTypes.PhoneNumber:
                        {
                            claims.Add(new Claim(item, user.PhoneNumber));
                        }
                        break;
                    case JwtClaimTypes.Picture:
                        {
                            claims.Add(new Claim(item, user.Avatar ?? ""));
                        }
                        break;
                }
            }
            //可以添加自定义用户信息
            //claims.Add(new Claim("custum", "testvalue"));
            context.AddRequestedClaims(claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            _logger.LogDebug("验证用户是否有效：{caller}", context.Caller);

            var sub = context.Subject?.GetSubjectId();
            if(sub == null)
                throw new Exception("获取用户信息失败，用户Id为空");

            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null && !user.IsModerated;
        }

        /// <summary>
        /// 获取授权访问的claims
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private List<string> RequestClaims(ProfileDataRequestContext context)
        {
            ICollection<ParsedScopeValue> requestScope = context.RequestedResources.ParsedScopes;
            List<string> allowClaims = new List<string>();
            List<string> result = new List<string>();

            //userinfo request
            if(_httpContextAccessor.HttpContext.Request.Path.Value?.ToLower().StartsWith("/connect/userinfo") == true)
            {
                var userClaims = context.Subject.Claims?
                    .GroupBy(p => p.Type)
                    .Select(q => q.Key).ToList();
                result.AddRange(userClaims);
            }
            if(context.RequestedResources.Resources.ApiResources == null
                || context.RequestedResources.Resources.ApiResources.Count == 0)
            {
                return result;
            }
            //从允许的认证资源中取出允许的UserClaims
            foreach(var pItem in context.RequestedResources.Resources.IdentityResources)
            {
                //判断资源是否在请求的资源中
                if(!requestScope.Any(p => p.ParsedName == pItem.Name))
                    continue;
                if(pItem.UserClaims == null || pItem.UserClaims.Count == 0)
                    continue;
                foreach(var item in pItem.UserClaims)
                {
                    if(allowClaims.Contains(item))
                        continue;
                    allowClaims.Add(item);
                }
            }
            //在ApiResources中包含的UserClaims，且在允许的UserClaims中
            foreach(var pItem in context.RequestedResources.Resources.ApiResources)
            {
                if(pItem.UserClaims == null || pItem.UserClaims.Count == 0)
                    continue;
                foreach(var item in pItem.UserClaims)
                {
                    if(!allowClaims.Contains(item))
                        continue;
                    if(result.Contains(item))
                        continue;
                    result.Add(item);
                }
            }
            //在ApiScopes中包含的UserClaims，且在允许的UserClaims中
            foreach(var pItem in context.RequestedResources.Resources.ApiScopes)
            {
                if(pItem.UserClaims == null || pItem.UserClaims.Count == 0)
                    continue;
                foreach(var item in pItem.UserClaims)
                {
                    if(!allowClaims.Contains(item))
                        continue;
                    if(result.Contains(item))
                        continue;
                    result.Add(item);
                }
            }
            //如果请求包含了Profile，则将Profile包含的ClaimType添加到result中
            if(allowClaims.Any(p => p == JwtClaimTypes.Profile
            || result.Any(p => p == JwtClaimTypes.Profile)))
            {
                if(!result.Contains(JwtClaimTypes.Picture))
                    result.Add(JwtClaimTypes.Picture);
                if(!result.Contains(JwtClaimTypes.Gender))
                    result.Add(JwtClaimTypes.Gender);
            }
            return result;
        }
    }
}
