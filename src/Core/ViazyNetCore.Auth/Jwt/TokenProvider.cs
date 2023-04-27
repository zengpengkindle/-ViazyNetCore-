using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ViazyNetCore.Caching;

namespace ViazyNetCore.Auth.Jwt
{
    public class TokenProvider
    {
        private readonly JwtOption _option;
        private readonly IDistributedHashCache? _cacheService;

        public TokenProvider(JwtOption option, IServiceProvider serviceProvider)
        {
            this._option = option;
            this._cacheService = serviceProvider.GetService<IDistributedHashCache>();
        }

#pragma warning disable IDE1006 // 命名样式
        private const string HashCachePrefix = "JwtToken:";
#pragma warning restore IDE1006 // 命名样式

        public async Task<JwtTokenResult> IssueToken(long userId, string username, object[]? roleIds)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var expires = DateTime.Now.Add(TimeSpan.FromSeconds(_option.ExpiresIn));

            var jti = Guid.NewGuid().ToShortId();
            var clientName = this._option.AppName ?? "app";

            var tokenDescripor = new SecurityTokenDescriptor
            {
                Expires = expires,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_option.Secret)),
                    SecurityAlgorithms.HmacSha256Signature
                ),
                Subject = new ClaimsIdentity(new Claim[] {
                     new Claim(JwtRegisteredClaimNames.Jti, jti),
                     new Claim(JwtRegisteredClaimNames.Sid, userId.ToString()),
                     new Claim(JwtRegisteredClaimNames.Aud, clientName),
                     new Claim(ClaimTypes.Name, username),
                     new Claim(JwtRegisteredClaimNames.Iss, this._option.Issuer),
                     new Claim(JwtRegisteredClaimNames.Exp, expires.ConvertToJsTime().ToString()),
                     new Claim(JwtRegisteredClaimNames.Nbf, DateTime.Now.ConvertToJsTime().ToString())
                     //new Claim(JwtRegisteredClaimNames.Typ, )
                })
            };
            if (roleIds != null)
            {
                tokenDescripor.Subject.AddClaim(new Claim(ClaimTypes.Role, string.Join(",", roleIds.Select(t => t?.ToString()))));
            }

            var token = tokenHandler.CreateJwtSecurityToken(tokenDescripor);
            var tokenString = tokenHandler.WriteToken(token);
            var result = new JwtTokenResult()
            {
                AccessToken = tokenString,
                ExpiresIn = expires.ConvertToJsTime(),
            };

            if (this._option.UseDistributedCache)
            {
                if (this._cacheService != null)
                {
                    var redisFieldKey = this.GenerateCacheKey(userId);
                    await this._cacheService.HashSetAsync(HashCachePrefix + clientName, redisFieldKey, jti.Object2Bytes(), new Microsoft.Extensions.Caching.Distributed.DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(this._option.ExpiresIn)
                    });
                }
            }
            return result;
        }

        public async Task ValidToken(JwtSecurityToken token)
        {
            var jti = token.Id;
            var sid = token.Claims.GetUserId();
            var clientName = token.Claims.GetClientId();

            if (jti.IsNullOrEmpty() || sid == 0 || clientName.IsNullOrEmpty())
            {
                throw new UnauthorizedAccessException();
            }

            if (sid == 0)
            {
                throw new UnauthorizedAccessException();
            }

            var redisFieldKey = this.GenerateCacheKey(sid!);
            if (this._option.UseDistributedCache && this._cacheService != null)
            {
                var currentJti = await this._cacheService.HashGetAsync<string>(HashCachePrefix + clientName, redisFieldKey);

                if (currentJti.IsNullOrEmpty())
                {
                    throw new UnauthorizedAccessException();
                }
                //用户在其他设备登录
                if (!this._option.MultiPort && currentJti != jti)
                {
                    throw new SingleSignOnException();
                }
            }
        }

        private string GenerateCacheKey(long userId)
        {
            return $"JwtToken:{userId}";
        }

        public void RemoveToken(long userId)
        {
            if (this._option.UseDistributedCache)
            {
                var redisFieldKey = GenerateCacheKey(userId);
                if (this._cacheService != null)
                {
                    var clientName = this._option.AppName ?? "app";
                    this._cacheService.HashRemove(HashCachePrefix + clientName, redisFieldKey);
                }
            }
        }
    }
}
