using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using ViazyNetCore.Caching;

namespace ViazyNetCore.IdentityService4.FreeSql.Stores
{
    public class ResourceStore : IResourceStore
    {
        private readonly IMapper _mapper;

        public ResourceStore(IConfigurationDbContext context
            , ICacheService cacheService
            , ILogger<ResourceStore> logger
            , IMapper mapper)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            this.CacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            this.Logger = logger;
            this._mapper = mapper;
        }

        public IConfigurationDbContext Context { get; }
        public ICacheService CacheService { get; }
        public ILogger<ResourceStore> Logger { get; }


        public async Task<IEnumerable<ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames)
        {
            Check.NotNull(apiResourceNames, nameof(apiResourceNames));

            List<Entities.ApiResource> resources = await Context.ApiResources.Where(p => apiResourceNames.Contains(p.Name))
               .IncludeMany(x => x.Secrets.Where(q => q.ApiResourceId == x.Id))
               .IncludeMany(x => x.Scopes.Where(q => q.ApiResourceId == x.Id))
               .IncludeMany(x => x.UserClaims.Where(q => q.ApiResourceId == x.Id))
               .IncludeMany(x => x.Properties.Where(q => q.ApiResourceId == x.Id))
               .NoTracking()
               .ToListAsync();
            if(resources == null)
                return new List<ApiResource>();

            var result = resources.Select(x => this._mapper.Map<Entities.ApiResource, IdentityServer4.Models.ApiResource>(x)).ToList();
            if(result.Any())
            {
                Logger.LogDebug("Found {apis} API resource in database", result.Select(x => x.Name));
            }
            else
            {
                Logger.LogDebug("Did not find {apis} API resource in database", apiResourceNames);
            }
            return result;
        }

        public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            var resources = await Context.ApiResources
                .Where(p => p.Scopes.AsSelect().Any(x => scopeNames.Contains(x.Scope) && p.Id == x.ApiResourceId))
                .IncludeMany(x => x.Secrets.Where(q => q.ApiResourceId == x.Id))
                .IncludeMany(x => x.Scopes.Where(q => q.ApiResourceId == x.Id))
                .IncludeMany(x => x.UserClaims.Where(q => q.ApiResourceId == x.Id))
                .IncludeMany(x => x.Properties.Where(q => q.ApiResourceId == x.Id))
                .NoTracking()
                .ToListAsync();
            if(resources == null)
                return new List<ApiResource>();

            var models = resources.Select(x => this._mapper.Map<Entities.ApiResource, IdentityServer4.Models.ApiResource>(x));
            return models;
        }

        public async Task<IEnumerable<ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
        {
            var resources = await Context.ApiScopes
                .Where(p => scopeNames.Contains(p.Name))
                .IncludeMany(x => x.UserClaims.Where(q => q.ScopeId == x.Id))
                .IncludeMany(x => x.Properties.Where(q => q.ScopeId == x.Id))
                .NoTracking()
                .ToListAsync();
            if(resources == null)
                return new List<ApiScope>();

            return resources.Select(x => this._mapper.Map<Entities.ApiScope, IdentityServer4.Models.ApiScope>(x)).ToArray();
        }

        public async Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            var resources = await Context.IdentityResources.Where(p => scopeNames.Contains(p.Name))
                .IncludeMany(x => x.UserClaims.Where(q => q.IdentityResourceId == x.Id))
                .IncludeMany(x => x.Properties.Where(q => q.IdentityResourceId == x.Id))
                .NoTracking()
                .ToListAsync();
            if(resources == null)
                return new List<IdentityResource>();

            return resources.Select(x => this._mapper.Map<Entities.IdentityResource, IdentityServer4.Models.IdentityResource>(x)).ToArray();
        }

        public async Task<Resources> GetAllResourcesAsync()
        {
            var result = await this.CacheService.GetAsync(StorageCaches.CLIENT_ALLRESOURCES, async () =>
            {
                var identity = Context.IdentityResources.Select
                    .IncludeMany(x => x.UserClaims.Where(q => q.IdentityResourceId == x.Id))
                    .IncludeMany(x => x.Properties.Where(q => q.IdentityResourceId == x.Id))
                    .NoTracking();

                var apis = Context.ApiResources.Select
                    .IncludeMany(x => x.Secrets.Where(q => q.ApiResourceId == x.Id))
                    .IncludeMany(x => x.Scopes.Where(q => q.ApiResourceId == x.Id))
                    .IncludeMany(x => x.UserClaims.Where(q => q.ApiResourceId == x.Id))
                    .IncludeMany(x => x.Properties.Where(q => q.ApiResourceId == x.Id))
                    .NoTracking();

                var scopes = Context.ApiScopes.Select
                    .IncludeMany(x => x.UserClaims.Where(q => q.ScopeId == x.Id))
                    .IncludeMany(x => x.Properties.Where(q => q.ScopeId == x.Id))
                    .NoTracking();

                var result = new IdentityServer4.Models.Resources(
                    (await identity.ToListAsync()).Select(x => this._mapper.Map<Entities.IdentityResource, IdentityServer4.Models.IdentityResource>(x)),
                    (await apis.ToListAsync()).Select(x => this._mapper.Map<Entities.ApiResource, IdentityServer4.Models.ApiResource>(x)),
                    (await scopes.ToListAsync()).Select(x => this._mapper.Map<Entities.ApiScope, IdentityServer4.Models.ApiScope>(x))
                );
                return result;
            }, CachingExpirationType.SingleObject);
            return result;
        }
    }
}
