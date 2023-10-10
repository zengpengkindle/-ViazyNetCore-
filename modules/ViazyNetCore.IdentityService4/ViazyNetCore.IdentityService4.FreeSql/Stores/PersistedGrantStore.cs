using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using IdentityServer4.Models;
using Microsoft.Extensions.Logging;
using static IdentityServer4.Events.TokenIssuedSuccessEvent;

namespace ViazyNetCore.IdentityService4.FreeSql.Stores
{
    public class PersistedGrantStore : IPersistedGrantStore
    {
        private readonly ILogger<PersistedGrantStore> _logger;
        private readonly IMapper _mapper;

        public PersistedGrantStore(IPersistedGrantDbContext context
            , ILogger<PersistedGrantStore> logger
            , IMapper mapper)
        {
            this.Context = context;
            this._logger = logger;
            this._mapper = mapper;
        }

        public IPersistedGrantDbContext Context { get; }

        public Task<IEnumerable<PersistedGrant>> GetAllAsync(PersistedGrantFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<PersistedGrant> GetAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAllAsync(PersistedGrantFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(string key)
        {
            throw new NotImplementedException();
        }

        public async Task StoreAsync(PersistedGrant token)
        {
            var existing = await Context.PersistedGrants
                .Where(x => x.Key == token.Key)
                .ToOneAsync();
            if(existing == null)
            {
                this._logger.LogDebug("{persistedGrantKey} not found in database", token.Key);

                var persistedGrant = this._mapper.Map<PersistedGrant,Entities.PersistedGrant>(token);
                Context.PersistedGrants.Add(persistedGrant);
            }
            else
            {
                this._logger.LogDebug("{persistedGrantKey} found in database", token.Key);
                this._mapper.Map(token, existing);
            }

            try
            {
                await Context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                this._logger.LogError(ex, "exception updating {persistedGrantKey} persisted grant in database: {error}", token.Key, ex.Message);
            }
        }
    }
}
