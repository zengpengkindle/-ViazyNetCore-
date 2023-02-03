using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
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

        public async Task<JwtTokenResult> IssueToken(long userId, object[] roleIds)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var expires = DateTime.UtcNow.Add(TimeSpan.FromSeconds(_option.ExpiresIn));

            var jti = Guid.NewGuid().ToShortId();
            var clientName = this._option.AppName ?? "app";

            var tokenDescripor = new SecurityTokenDescriptor
            {
                Expires = expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey
                (
                    Encoding.UTF8.GetBytes(_option.Secret)),
                    SecurityAlgorithms.HmacSha256Signature
                ),
                Subject = new ClaimsIdentity(new Claim[] {
                     new Claim(JwtRegisteredClaimNames.Jti, jti),
                     new Claim(JwtRegisteredClaimNames.Sid, userId.ToString()),
                     new Claim(JwtRegisteredClaimNames.Aud, clientName),
                     new Claim(JwtRegisteredClaimNames.Iss, _option.Issuer),
                     new Claim(ClaimTypes.Role, string.Join(",", roleIds.Select(t => t.CastTo<int>()))),
                     //new Claim(JwtRegisteredClaimNames.Typ, )
                })
            };

            var token = tokenHandler.CreateJwtSecurityToken(tokenDescripor);
            var tokenString = tokenHandler.WriteToken(token);
            var result = new JwtTokenResult()
            {
                AccessToken = tokenString,
                ExpiresIn = _option.ExpiresIn,
            };

            if (this._option.UseDistributedCache)
            {
                var redisFieldKey = GenerateCacheKey(userId);
                if (this._cacheService != null)
                {
                    await this._cacheService.HashSetAsync(HashCachePrefix + clientName, redisFieldKey, jti);
                }
            }
            return result;
        }

        public async Task ValidToken(JwtSecurityToken token)
        {
            var jti = token.Id;
            var sid = token.Claims.FirstOrDefault(t => t.Type == JwtRegisteredClaimNames.Sid)?.Value;
            var clientName = token.Claims.FirstOrDefault(t => t.Type == JwtRegisteredClaimNames.Aud)?.Value;

            if (jti.IsNullOrEmpty() || sid.IsNullOrEmpty() || clientName.IsNullOrEmpty())
            {
                throw new UnauthorizedAccessException();
            }

            _ = int.TryParse(sid, out var userId);
            if (userId <= 0)
            {
                throw new UnauthorizedAccessException();
            }

            var type = token.Claims.FirstOrDefault(t => t.Type == JwtRegisteredClaimNames.Typ)?.Value ?? "";

            //模拟登录
            if (type == "mock")
            {
                return;
            }

            var redisFieldKey = this.GenerateCacheKey(userId);
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
                    //throw new SingleSignOnException();
                }
            }
        }

        private string GenerateCacheKey(long userId)
        {
            return $"Caesar:JwtToken:{userId}";
        }

        public void RemoveToken(long userId)
        {
            var key = GenerateCacheKey(userId);
            //redis.Remove(key);
        }
    }
}
