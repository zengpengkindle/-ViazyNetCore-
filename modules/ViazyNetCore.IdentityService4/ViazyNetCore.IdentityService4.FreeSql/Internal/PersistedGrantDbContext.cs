using System;
using System.Threading.Tasks;
using FreeSql;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using ViazyNetCore.IdentityService4.FreeSql.Options;
using ViazyNetCore.IdentityService4.FreeSql.Entities;

namespace ViazyNetCore.IdentityService4.FreeSql
{
    public class PersistedGrantDbContext : PersistedGrantDbContext<PersistedGrantDbContext>
    {
        public PersistedGrantDbContext(IFreeSql freeSql, OperationalStoreOptions storeOptions)
            : base(freeSql, storeOptions)
        {
        }
    }

    public class PersistedGrantDbContext<TContext> : DbContext, IPersistedGrantDbContext
        where TContext : DbContext, IPersistedGrantDbContext
    {
        private readonly IFreeSql _freeSql;
        private readonly OperationalStoreOptions _storeOptions;

        public PersistedGrantDbContext(IFreeSql freeSql, OperationalStoreOptions storeOptions)
            : base(freeSql, null)
        {
            this._freeSql = freeSql;
            if(storeOptions == null) throw new ArgumentNullException(nameof(storeOptions));
            this._storeOptions = storeOptions;

            OnModelCreating(_freeSql.CodeFirst);
        }

        public DbSet<PersistedGrant> PersistedGrants { get; set; }

        public DbSet<DeviceFlowCodes> DeviceFlowCodes { get; set; }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }


        protected new void OnModelCreating(ICodeFirst codefirst)
        {
            if(codefirst.IsAutoSyncStructure)
            {
                var items = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.PropertyType.FullName.StartsWith(typeof(DbSet<object>).FullName.Split('`')[0])).ToList();
                List<Type> entityTypes = new List<Type>();
                foreach(var item in items)
                {
                    if(item.PropertyType.GenericTypeArguments == null
                        || item.PropertyType.GenericTypeArguments.Length != 1)
                        continue;
                    entityTypes.Add(item.PropertyType.GenericTypeArguments[0]);
                }
                codefirst.SyncStructure(entityTypes.ToArray());
            }
        }
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseFreeSql(orm: _freeSql.UseDb(_storeOptions.DbName));
            base.OnConfiguring(builder);
        }
    }

}
