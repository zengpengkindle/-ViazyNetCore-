using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ViazyNetCore.Data.FreeSql.Extensions
{
    public static class UserTokenExtensions
    {
        /// <summary>
        /// AuthUser用户信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static long GetUserId(this IHttpContextAccessor context)
        {
            return context.HttpContext?.User?.Claims.GetUserId() ?? 0;
        }

        /// <summary>
        /// 获取当前登录用户的Id（ids4用户主键guid）。
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public static long GetUserId(this IEnumerable<Claim> claims)
        {
            var sub = claims.FirstOrDefault(c => c.Type == JwtClaimTypes.SessionId)?.Value;
            //ids4用户的唯一标识符
            return sub?.ParseTo<long>() ?? 0;
        }

        /// <summary>
        /// AuthUser用户信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string? GetUserName(this IHttpContextAccessor context)
        {
            return context.HttpContext?.User?.Claims.GetUserName() ;
        }
        /// <summary>
        /// 获取当前用户的用户名称
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public static string? GetUserName(this IEnumerable<Claim> claims)
        {
            return claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Name)?.Value;
        }

        /// <summary>
        /// AuthUser用户信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string? GetNickName(this IHttpContextAccessor context)
        {
            return context.HttpContext?.User?.Claims.GetNickName();
        }
        /// <summary>
        /// 获取当前用户的用户名称
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public static string? GetNickName(this IEnumerable<Claim> claims)
        {
            return claims.FirstOrDefault(c => c.Type == JwtClaimTypes.NickName)?.Value;
        }
        /// <summary>
        /// AuthUser用户信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static AuthUserType? GetAuthUserType(this IHttpContextAccessor context)
        {
            return context.HttpContext?.User?.Claims.GetAuthUserType();
        }
        /// <summary>
        /// 获取用户类型
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public static AuthUserType? GetAuthUserType(this IEnumerable<Claim> claims)
        {
            var value = claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Type)?.Value;
            if (string.IsNullOrEmpty(value))
                return null;
            return Enum.Parse<AuthUserType>(value);
        }
    }
}
