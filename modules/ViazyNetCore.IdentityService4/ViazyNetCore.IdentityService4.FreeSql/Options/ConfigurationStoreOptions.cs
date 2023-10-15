using System;
using FreeSql;

namespace ViazyNetCore.IdentityService4.FreeSql.Options
{
    public class ConfigurationStoreOptions : StoreOptionsBase
    {
        public TableConfiguration IdentityResource { get; set; } = new TableConfiguration("IdentityResources");

        public TableConfiguration IdentityResourceClaim { get; set; } = new TableConfiguration("IdentityResourceClaims");

        public TableConfiguration IdentityResourceProperty { get; set; } = new TableConfiguration("IdentityResourceProperties");

        public TableConfiguration ApiResource { get; set; } = new TableConfiguration("ApiResources");

        public TableConfiguration ApiResourceSecret { get; set; } = new TableConfiguration("ApiResourceSecrets");

        public TableConfiguration ApiResourceScope { get; set; } = new TableConfiguration("ApiResourceScopes");

        public TableConfiguration ApiResourceClaim { get; set; } = new TableConfiguration("ApiResourceClaims");

        public TableConfiguration ApiResourceProperty { get; set; } = new TableConfiguration("ApiResourceProperties");

        public TableConfiguration Client { get; set; } = new TableConfiguration("Clients");

        public TableConfiguration ClientGrantType { get; set; } = new TableConfiguration("ClientGrantTypes");

        public TableConfiguration ClientRedirectUri { get; set; } = new TableConfiguration("ClientRedirectUris");

        public TableConfiguration ClientPostLogoutRedirectUri { get; set; } = new TableConfiguration("ClientPostLogoutRedirectUris");

        public TableConfiguration ClientScopes { get; set; } = new TableConfiguration("ClientScopes");

        public TableConfiguration ClientSecret { get; set; } = new TableConfiguration("ClientSecrets");

        public TableConfiguration ClientClaim { get; set; } = new TableConfiguration("ClientClaims");

        public TableConfiguration ClientIdPRestriction { get; set; } = new TableConfiguration("ClientIdPRestrictions");

        public TableConfiguration ClientCorsOrigin { get; set; } = new TableConfiguration("ClientCorsOrigins");

        public TableConfiguration ClientProperty { get; set; } = new TableConfiguration("ClientProperties");

        public TableConfiguration ApiScope { get; set; } = new TableConfiguration("ApiScopes");

        public TableConfiguration ApiScopeClaim { get; set; } = new TableConfiguration("ApiScopeClaims");

        public TableConfiguration ApiScopeProperty { get; set; } = new TableConfiguration("ApiScopeProperties");
    }
}