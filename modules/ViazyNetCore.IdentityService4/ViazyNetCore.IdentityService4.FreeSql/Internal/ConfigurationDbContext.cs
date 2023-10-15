using System.Reflection;
using FreeSql;
using ViazyNetCore.IdentityService4.FreeSql.Options;
using ViazyNetCore.IdentityService4.FreeSql.Entities;

namespace ViazyNetCore.IdentityService4.FreeSql
{
    public class ConfigurationDbContext : ConfigurationDbContext<ConfigurationDbContext>
    {
        public ConfigurationDbContext(IFreeSql freeSql, ConfigurationStoreOptions storeOptions)
            : base(freeSql, storeOptions)
        {
        }


    }

    public class ConfigurationDbContext<TContext> : DbContext, IConfigurationDbContext
        where TContext : DbContext, IConfigurationDbContext
    {
        private readonly IFreeSql _freeSql;
        private readonly ConfigurationStoreOptions _storeOptions;

        public ConfigurationDbContext(IFreeSql freeSql, ConfigurationStoreOptions storeOptions)
            : base(freeSql, null)
        {
            this._freeSql = freeSql ?? throw new ArgumentNullException(nameof(IFreeSql));
            this._storeOptions = storeOptions ?? throw new ArgumentNullException(nameof(ConfigurationStoreOptions));

            OnModelCreating(_freeSql.CodeFirst);
        }

        public DbSet<Client> Clients { get; set; }

        public DbSet<ClientCorsOrigin> ClientCorsOrigins { get; set; }

        public DbSet<IdentityResource> IdentityResources { get; set; }

        public DbSet<ApiResource> ApiResources { get; set; }

        public DbSet<ApiScope> ApiScopes { get; set; }

        public DbSet<ApiResourceSecret> ApiResourceSecrets { get; set; }

        public DbSet<ApiResourceScope> ApiResourceScopes { get; set; }

        public DbSet<ApiResourceClaim> ApiResourceClaims { get; set; }

        public DbSet<ApiResourceProperty> ApiResourceProperties { get; set; }

        public DbSet<ApiScopeProperty> ApiScopeProperties { get; set; }

        public DbSet<ApiScopeClaim> ApiScopeClaims { get; set; }

        public DbSet<ClientSecret> ClientSecrets { get; set; }

        public DbSet<ClientGrantType> ClientGrantTypes { get; set; }

        public DbSet<ClientRedirectUri> ClientRedirectUris { get; set; }

        public DbSet<ClientPostLogoutRedirectUri> ClientPostLogoutRedirectUris { get; set; }

        public DbSet<ClientScope> ClientScopes { get; set; }

        public DbSet<ClientIdPRestriction> ClientIdPRestrictions { get; set; }

        public DbSet<ClientClaim> ClientClaims { get; set; }

        public DbSet<ClientProperty> ClientProperties { get; set; }

        public DbSet<IdentityResourceClaim> IdentityResourceClaims { get; set; }

        public DbSet<IdentityResourceProperty> IdentityResourceProperties { get; set; }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        protected new void OnModelCreating(ICodeFirst codefirst)
        {
            #region Create Table

            if (codefirst.IsAutoSyncStructure)
            {
                var items = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.PropertyType.FullName.StartsWith(typeof(DbSet<object>).FullName.Split('`')[0])).ToList();
                List<Type> entityTypes = new List<Type>();
                foreach (var item in items)
                {
                    if (item.PropertyType.GenericTypeArguments == null
                        || item.PropertyType.GenericTypeArguments.Length != 1)
                        continue;
                    entityTypes.Add(item.PropertyType.GenericTypeArguments[0]);
                }
                codefirst.SyncStructure(entityTypes.ToArray());
            }

            #endregion
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseFreeSql(orm: _freeSql.UseDb(_storeOptions.DbName));
            base.OnConfiguring(builder);
        }

    }

}
