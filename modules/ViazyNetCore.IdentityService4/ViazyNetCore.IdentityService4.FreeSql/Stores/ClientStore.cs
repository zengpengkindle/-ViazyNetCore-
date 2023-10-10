using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.Extensions.Logging;
using ViazyNetCore.Caching;

namespace ViazyNetCore.IdentityService4.FreeSql.Stores
{
    public class ClientStore : IClientStore
    {
        private readonly ICacheService _cacheService;
        private readonly ILogger<ClientStore> _logger;
        private readonly IMapper _mapper;

        public IConfigurationDbContext Context { get; }

        public ClientStore(IConfigurationDbContext context
            , ICacheService cacheService
            , ILogger<ClientStore> logger
            , IMapper mapper)
        {
            this.Context = context;
            this._cacheService = cacheService;
            this._logger = logger;
            this._mapper = mapper;
        }

        public async Task<Client> FindClientByIdAsync(string clientId)
        { //使用缓存
            var model = await _cacheService.GetAsync(StorageCaches.GetClientCacheKey(clientId), async () =>
            {
                //get client
                Entities.Client client = await Context.Clients
                     .Where(p => p.ClientId == clientId)
                     .IncludeMany(c => c.AllowedCorsOrigins.Where(p => p.ClientId == c.Id))
                     .IncludeMany(c => c.AllowedGrantTypes.Where(p => p.ClientId == c.Id))
                     .IncludeMany(c => c.AllowedScopes.Where(p => p.ClientId == c.Id))
                     .IncludeMany(c => c.Claims.Where(p => p.ClientId == c.Id))
                     .IncludeMany(c => c.ClientSecrets.Where(p => p.ClientId == c.Id))
                     .IncludeMany(c => c.IdentityProviderRestrictions.Where(p => p.ClientId == c.Id))
                     .IncludeMany(c => c.PostLogoutRedirectUris.Where(p => p.ClientId == c.Id))
                     .IncludeMany(c => c.Properties.Where(p => p.ClientId == c.Id))
                     .IncludeMany(c => c.RedirectUris.Where(p => p.ClientId == c.Id))
                     .ToOneAsync();
                if(client == null)
                    return null;
                var model = this._mapper.Map<Entities.Client, Client>(client);
                return model;
            }, CachingExpirationType.SingleObject);

            if(model == null)
            {
                _logger.LogWarning($"Query client by clientId '{clientId}' failed. ");
            }
            else
            {
                _logger.LogInformation($"Query client by clientId '{clientId}' successed. ");
            }
            return model;
        }
    }
}
