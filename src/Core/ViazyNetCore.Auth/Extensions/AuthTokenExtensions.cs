using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using ViazyNetCore;
using ViazyNetCore.Auth;

namespace Microsoft.AspNetCore.Http
{

    /// <summary>
    /// 用户票据扩展类型。
    /// </summary>
    public static class AuthTokenExtensions
    {
        #region AuthUser用户信息

        /// <summary>
        /// AuthUser用户信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static AuthUser? GetAuthUser(this IHttpContextAccessor context)
        {
            if (context.HttpContext == null)
                return null;
            return context.HttpContext.GetAuthUser();
        }

        /// <summary>
        /// AuthUser用户信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static AuthUser GetAuthUser(this HttpContext context)
        {
            var status = context.Request.Headers.TryGetValue("Authorization",
                out Microsoft.Extensions.Primitives.StringValues token);
            if (!status)
                return null;
            var tokenValue = token.ToString();
            if (string.IsNullOrEmpty(tokenValue))
                return null;
            var rawToken = tokenValue.Replace("Bearer ", "").Replace("bearer", "");
            Claim[] claims;
            try
            {
                claims = new JwtSecurityTokenHandler().ReadJwtToken(rawToken).Claims.ToArray();
            }
            catch (Exception e)
            {
                throw new UnauthorizedAccessException("Token format error, parsing failed", e);
            }
            return new AuthUser
            {
                IdentityType = claims.GetAuthUserType() ?? AuthUserType.Unknown,
                Id = claims.GetUserId(),
                ClientId = claims.GetClientId(),
                Exp = claims.GetExp(),
                Nbf = claims.GetNbf(),
                Username = claims.GetUserName(),
            };
        }

        public static int[] GetAuthUserRoleIds(this HttpContext context)
        {
            var roles = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (roles.IsNull())
                return new int[0];
            else
                return roles!.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(p => p.ParseTo<int>()).ToArray();
        }
        #endregion

        #region AuthUser用户信息
        /// <summary>
        /// AuthUser用户信息
        /// </summary>
        /// <param name="jwtToken">jwtToken</param>
        /// <param name="errorAction">错误处理</param>
        /// <returns></returns>
        public static AuthUser GetAuthUser(this string jwtToken, Action<Exception> errorAction)
        {
            Claim[] claims;
            string amr;
            try
            {
                var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(jwtToken);
                amr = jwtSecurityToken.Payload?.Amr.FirstOrDefault(); ;
                claims = jwtSecurityToken.Claims.ToArray();
            }
            catch (Exception e)
            {
                errorAction(e);
                return null;
            }
            return new AuthUser
            {
                IdentityType = claims.GetAuthUserType() ?? AuthUserType.Unknown,
                Id = claims.GetUserId(),
                ClientId = claims.GetClientId(),
                Exp = claims.GetExp(),
                Nbf = claims.GetNbf(),
                Username = claims.GetUserName(),
                Nickname = claims.GetNickName(),
                Amr = amr
            };
        }
        #endregion

        #region 获取当前登录用户的Id（ids4用户主键guid）。
        /// <summary>
        /// 获取当前登录用户的Id（ids4用户主键guid）。
        /// </summary>
        /// <param name="claims">当前用户接口实例。</param>
        /// <returns>返回用户Id，如果未登录则返回null。</returns>
        public static long GetUserId(this ClaimsPrincipal claims)
        {
            //ids4用户的唯一标识符
            var userid = claims.GetFirstValue(JwtRegisteredClaimNames.Sid);
            return userid?.ParseTo<long>() ?? 0;
        }

        /// <summary>
        /// 获取当前登录用户的Id（ids4用户主键guid）。
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public static long GetUserId(this IEnumerable<Claim> claims)
        {
            var sub = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sid)?.Value;
            //ids4用户的唯一标识符
            return sub?.ParseTo<long>() ?? 0;
        }
        /// <summary>
        /// 获取当前登录用户的Id（ids4用户主键guid）。
        /// </summary>
        /// <param name="request">当前用户接口实例。</param>
        /// <returns>返回用户Id，如果未登录则返回null。</returns>
        public static long GetUserId(this HttpRequest request)
        {
            return request.Claims().GetUserId();
        }
        #endregion

        #region 获取当前用户的用户名称。
        /// <summary>
        /// 获取当前用户的用户名称。
        /// </summary>
        /// <param name="claims">当前用户接口实例。</param>
        /// <returns>返回用户名称</returns>
        public static string GetUserName(this ClaimsPrincipal claims)
        {
            return claims.GetFirstValue(ClaimTypes.Name);
        }
        /// <summary>
        /// 获取当前用户的用户名称。
        /// </summary>
        /// <param name="request">当前用户接口实例。</param>
        /// <returns>返回用户名称。</returns>
        public static string? GetUserName(this HttpRequest request)
        {
            return request.Claims().FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        }

        /// <summary>
        /// 获取当前用户的用户名称
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public static string? GetUserName(this IEnumerable<Claim> claims)
        {
            return claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        }
        /// <summary>
        /// 获取当前用户的用户名称
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public static string? GetNickName(this IEnumerable<Claim> claims)
        {
            return claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;
        }
        #endregion

        #region 获取用户类型
        /// <summary>
        /// 获取用户类型
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public static AuthUserType? GetAuthUserType(this Claim[] claims)
        {
            var value = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Typ)?.Value;
            if (string.IsNullOrEmpty(value))
                return null;
            return Enum.Parse<AuthUserType>(value);
        }
        /// <summary>
        /// 获取用户类型
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public static AuthUserType? GetAuthUserType(this IEnumerable<Claim> claims)
        {
            var value = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Typ)?.Value;
            if (string.IsNullOrEmpty(value))
                return null;
            return Enum.Parse<AuthUserType>(value);
        }

        /// <summary>
        /// 获取用户类型
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static AuthUserType? GetAuthUserType(this HttpRequest request)
        {
            return request.Claims().GetAuthUserType();
        }
        #endregion

        #region 获取 Client Id
        /// <summary>
        /// 获取 Client Id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetClientId(this HttpRequest request)
        {
            return request.Claims().FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Aud)?.Value;
        }
        /// <summary>
        /// 获取 Client Id
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public static string GetClientId(this IEnumerable<Claim> claims)
        {
            return claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Aud)?.Value;
        }
        #endregion

        #region Token签发时间(秒时间搓)
        /// <summary>
        /// Token签发时间(秒时间搓)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static long GetNbf(this HttpRequest request)
        {
            var ngf = request.Claims().FirstOrDefault(c => c.Type == "nbf")?.Value;
            if (!string.IsNullOrEmpty(ngf))
                return long.Parse(ngf);
            return default;
        }
        /// <summary>
        /// Token签发时间(秒时间搓)
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public static long GetNbf(this Claim[] claims)
        {
            var ngf = claims.FirstOrDefault(c => c.Type == "nbf")?.Value;
            if (!string.IsNullOrEmpty(ngf))
                return long.Parse(ngf);
            return default;
        }
        #endregion

        #region Token过期时间(秒时间搓)
        /// <summary>
        /// Token过期时间(秒时间搓)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static long GetExp(this HttpRequest request)
        {
            var exp = request.Claims().FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;
            if (!string.IsNullOrEmpty(exp))
                return long.Parse(exp);
            return default;
        }

        /// <summary>
        /// Token过期时间(秒时间搓)
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public static long GetExp(this IEnumerable<Claim> claims)
        {
            var exp = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;
            if (!string.IsNullOrEmpty(exp))
                return long.Parse(exp);
            return default;
        }
        #endregion

        #region 获取token
        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetToken(this HttpRequest request)
        {
            try
            {
                return request.Headers["Authorization"];
            }
            catch (Exception)
            {
                throw new UnauthorizedAccessException();
            }
        }
        #endregion

        #region 获取claim 值
        /// <summary>
        /// 获取claim 值
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="claimType"></param>
        /// <returns></returns>
        private static string GetFirstValue(this ClaimsPrincipal claims, string claimType)
        {
            return claims.FindFirst(claimType)?.Value;
        }
        /// <summary>
        /// 获取claim 值
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="claimType"></param>
        /// <returns></returns>
        private static string GetFirstValue(this IEnumerable<Claim> claims, string claimType)
        {
            return claims.FirstOrDefault(c => c.Type == claimType)?.Value;
        }
        #endregion

        #region 手动获取 Headers claim 值
        /// <summary>
        /// 手动获取 Headers claim 值
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static IEnumerable<Claim> Claims(this HttpRequest request)
        {
            try
            {
                var headerCode = request.Headers["Authorization"];
                if (string.IsNullOrEmpty(headerCode))
                    return new List<Claim>();

                var code = headerCode.ToString().Replace("Bearer ", "");
                // 这里从 jwt token 里解析出用户id，默认认为已经通过权限验证了
                return new JwtSecurityTokenHandler().ReadJwtToken(code)?.Claims;
            }
            catch (Exception ex)
            {
                throw new UnauthorizedAccessException(ex.ToString());
            }
        }
        #endregion
    }
}
