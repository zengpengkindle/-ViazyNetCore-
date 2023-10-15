using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FreeSql;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using Microsoft.Extensions.Logging;

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

        public async Task<IEnumerable<PersistedGrant>> GetAllAsync(PersistedGrantFilter filter)
        {
            filter.Validate();
            var persistedGrants = await Filter(Context.PersistedGrants.Select, filter)
                .ToListAsync();
            //persistedGrants = Filter(persistedGrants.AsSelect((Context as PersistedGrantDbContext).Orm), filter).ToList();
            var model = persistedGrants.Select(x => this._mapper.Map<Entities.PersistedGrant, PersistedGrant>(x));
            _logger.LogDebug("{persistedGrantCount} persisted grants found for {@filter}", persistedGrants.Count, filter);
            return model;
        }

        public async Task<PersistedGrant> GetAsync(string key)
        {
            var persistedGrant = await Context.PersistedGrants
                .Where(x => x.Key == key)
                .NoTracking()
                .ToOneAsync();
            var model = this._mapper.Map<Entities.PersistedGrant, PersistedGrant>(persistedGrant);
            _logger.LogDebug("{persistedGrantKey} found in database: {persistedGrantKeyFound}", key, model != null);
            return model;
        }

        public async Task RemoveAllAsync(PersistedGrantFilter filter)
        {
            filter.Validate();
            var persistedGrants = await Filter(Context.PersistedGrants.Select, filter)
                .ToListAsync();
            _logger.LogDebug("Removing {persistedGrantCount} persisted grants from database for {@filter}", persistedGrants.Count, filter);
            Context.PersistedGrants.RemoveRange(persistedGrants);
            try
            {
                await Context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogInformation("Removing {persistedGrantCount} persisted grants from database for subject {@filter}: {error}", persistedGrants.Count, filter, ex.Message);
            }
        }

        public async Task RemoveAsync(string key)
        {
            var persistedGrant = await Context.PersistedGrants
                .Where(x => x.Key == key)
                .ToOneAsync();
            if(persistedGrant != null)
            {
                _logger.LogDebug("removing {persistedGrantKey} persisted grant from database", key);
                Context.PersistedGrants.Remove(persistedGrant);
                try
                {
                    await Context.SaveChangesAsync();
                }
                catch(Exception ex)
                {
                    _logger.LogInformation("exception removing {persistedGrantKey} persisted grant from database: {error}", key, ex.Message);
                }
            }
            else
            {
                _logger.LogDebug("no {persistedGrantKey} persisted grant found in database", key);
            }
        }

        public async Task StoreAsync(PersistedGrant token)
        {
            var existing = await Context.PersistedGrants
                .Where(x => x.Key == token.Key)
                .ToOneAsync();
            if(existing == null)
            {
                this._logger.LogDebug("{persistedGrantKey} not found in database", token.Key);

                var persistedGrant = this._mapper.Map<PersistedGrant, Entities.PersistedGrant>(token);
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

        private ISelect<Entities.PersistedGrant> Filter(ISelect<Entities.PersistedGrant> query, PersistedGrantFilter filter)
        {
            if(!string.IsNullOrWhiteSpace(filter.ClientId))
            {
                query = query.Where(x => x.ClientId == filter.ClientId);
            }
            if(!string.IsNullOrWhiteSpace(filter.SessionId))
            {
                query = query.Where(x => x.SessionId == filter.SessionId);
            }
            if(!string.IsNullOrWhiteSpace(filter.SubjectId))
            {
                query = query.Where(x => x.SubjectId == filter.SubjectId);
            }
            if(!string.IsNullOrWhiteSpace(filter.Type))
            {
                query = query.Where(x => x.Type == filter.Type);
            }

            return query;
        }
    }
}
