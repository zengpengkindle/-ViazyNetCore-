using System;
using System.Threading.Tasks;
using IdentityServer4.Services;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using FreeSql;

namespace ViazyNetCore.IdentityService4.FreeSql
{
    public class CorsPolicyService : ICorsPolicyService
    {
        private readonly IConfigurationDbContext _context;
        private readonly IHttpContextAccessor _accessor;
        private readonly ILogger<CorsPolicyService> _logger;

        public CorsPolicyService(IHttpContextAccessor accessor
            , ILogger<CorsPolicyService> logger
            , IConfigurationDbContext context)
        {
            _accessor = accessor ?? throw new ArgumentNullException(nameof(IHttpContextAccessor));
            _logger = logger ?? throw new ArgumentNullException(nameof(ILogger<CorsPolicyService>));
            _context = context ?? throw new ArgumentNullException(nameof(IConfigurationDbContext));
        }

        public async Task<bool> IsOriginAllowedAsync(string origin)
        {
            try
            {
                if (string.IsNullOrEmpty(origin))
                    return false;
                origin = origin.ToLowerInvariant();
                bool result = await _context.ClientCorsOrigins.Where(p => p.Origin == origin).AnyAsync();
                if (result)
                    _logger.LogDebug("Origin {origin} is allowed: {originAllowed}", origin, result);
                return result;
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex, $"Check origin '{origin}' failed, {ex.Message}");
                return false;
            }

        }
    }
}
