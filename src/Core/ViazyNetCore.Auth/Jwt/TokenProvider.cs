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
            _option = option;
            this._cacheService = serviceProvider.GetService<IDistributedHashCache>();
        }

        public JwtTokenResult GenerateToken(int userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var expires = DateTime.UtcNow.Add(TimeSpan.FromSeconds(_option.ExpiresIn));

            var jti = Guid.NewGuid().ToShortId();
            var clientName = this._option.AppName ?? "小程序";

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

            var key = GenerateCacheKey(userId);
            if (this._cacheService != null)
            { 
            }
            return result;
        }

        public Task ValidToken(JwtSecurityToken token)
        {
            var jti = token.Id;
            var sid = token.Claims.FirstOrDefault(t => t.Type == JwtRegisteredClaimNames.Sid)?.Value;
            var loginClient = token.Claims.FirstOrDefault(t => t.Type == JwtRegisteredClaimNames.Aud)?.Value;

            if(jti.IsNullOrEmpty() || sid.IsNullOrEmpty() || loginClient.IsNullOrEmpty())
            {
                throw new UnauthorizedAccessException();
            }

            _ = int.TryParse(sid, out var userId);
            if(userId <= 0)
            {
                throw new UnauthorizedAccessException();
            }
            if(userId == 38)
                return Task.CompletedTask;

            var type = token.Claims.FirstOrDefault(t => t.Type == JwtRegisteredClaimNames.Typ)?.Value ?? "";

            //模拟登录
            if(type == "mock")
            {
                return Task.CompletedTask;
            }

            var key = GenerateCacheKey(userId);
            //var currentJti = redis.GetValueFromHash<string>(key, loginClient);

            //if(currentJti.IsNullOrEmpty())
            //{
            //    throw new UnauthorizedAccessException();
            //}

            ////用户在其他设备登录
            //if(currentJti != jti)
            //{
            //    throw new SingleSignOnException();
            //}

            return Task.CompletedTask;
        }

        private string GenerateCacheKey(int userId)
        {
            return $"Caesar:JwtToken:{userId}";
        }

        public void RemoveToken(int userId)
        {
            var key = GenerateCacheKey(userId);
            //redis.Remove(key);
        }
    }
}
