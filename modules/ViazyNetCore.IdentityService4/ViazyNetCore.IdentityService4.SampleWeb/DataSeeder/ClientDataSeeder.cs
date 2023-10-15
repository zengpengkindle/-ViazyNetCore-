using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityModel;
using ViazyNetCore.IdentityService4.FreeSql;
using ViazyNetCore.IdentityService4.FreeSql.Entities;

namespace ViazyNetCore.IdentityService4.SampleWeb.DataSeeder
{
    public class ClientDataSeeder
    {
        private readonly IConfigurationDbContext _configurationDbContext;

        public ClientDataSeeder(IConfigurationDbContext configurationDbContext)
        {
            this._configurationDbContext = configurationDbContext;
        }

        public async Task CreateClientAsync()
        {
            List<IdentityResource> resources = new List<IdentityResource>()
                {
                    new IdentityResource()
                    {
                        Name = JwtClaimTypes.Role,
                        Enabled = true,
                        Required = false,
                        DisplayName = "角色信息",
                        UserClaims = new List<IdentityResourceClaim>()
                        {
                            new IdentityResourceClaim()
                            {
                                Type = JwtClaimTypes.Role,
                            }
                        }
                    },
                    new IdentityResource()
                    {
                        Name = "openid",
                        Enabled = true,
                        Required = true,
                        DisplayName = "用户Id",
                        UserClaims = new List<IdentityResourceClaim>()
                        {
                            new IdentityResourceClaim()
                            {
                                Type = JwtClaimTypes.Subject,
                            }
                        }
                    },
                    new IdentityResource()
                    {
                        Name ="profile",
                        Enabled = true,
                        Required = true,
                        DisplayName = "基本信息",
                        UserClaims = new List<IdentityResourceClaim>()
                        {
                            new IdentityResourceClaim()
                            {
                                Type = JwtClaimTypes.Name,
                            },
                            new IdentityResourceClaim()
                            {
                                Type = JwtClaimTypes.FamilyName,
                            },
                            new IdentityResourceClaim()
                            {
                                Type = JwtClaimTypes.GivenName,
                            },
                            new IdentityResourceClaim()
                            {
                                Type = JwtClaimTypes.MiddleName,
                            },
                            new IdentityResourceClaim()
                            {
                                Type = JwtClaimTypes.NickName,
                            },
                            new IdentityResourceClaim()
                            {
                                Type = JwtClaimTypes.PreferredUserName,
                            },
                            new IdentityResourceClaim()
                            {
                                Type = JwtClaimTypes.Profile,
                            },
                            new IdentityResourceClaim()
                            {
                                Type = JwtClaimTypes.Picture,
                            },
                            new IdentityResourceClaim()
                            {
                                Type = JwtClaimTypes.Gender,
                            },
                            new IdentityResourceClaim()
                            {
                                Type = JwtClaimTypes.WebSite,
                            }
                        }
                    }
                };

            List<ApiScope> apiScopes = new List<ApiScope>()
                {
                    new ApiScope()
                    {
                        Name = "api1",
                        DisplayName = "api1",
                        UserClaims = new List<ApiScopeClaim>(),
                    },
                    new ApiScope()
                    {
                        Name = "client_management",
                        DisplayName = "client_management",
                        UserClaims = new List<ApiScopeClaim>(),
                    },
                };

            List<ApiResource> apiResources = new List<ApiResource>()
                {
                    new ApiResource()
                    {
                        Name = "api1",
                        DisplayName = "api1",
                        Scopes = new List<ApiResourceScope>()
                        {
                            new ApiResourceScope()
                            {
                                Scope = apiScopes.Where(p=>p.DisplayName == "api1").FirstOrDefault()?.Name
                            }
                        },
                        UserClaims = new List<ApiResourceClaim>()
                        {
                            new ApiResourceClaim()
                            {
                                Type = JwtClaimTypes.Subject,
                            },
                            new ApiResourceClaim()
                            {
                                Type = JwtClaimTypes.Profile,
                            },
                            new ApiResourceClaim()
                            {
                                Type = JwtClaimTypes.Picture,
                            },
                            new ApiResourceClaim()
                            {
                                Type = JwtClaimTypes.Name,
                            },
                            new ApiResourceClaim()
                            {
                                Type = JwtClaimTypes.NickName,
                            },
                            new ApiResourceClaim()
                            {
                                Type = JwtClaimTypes.Role,
                            },
                            new ApiResourceClaim()
                            {
                                Type = JwtClaimTypes.PhoneNumber,
                            },
                            new ApiResourceClaim()
                            {
                                Type = JwtClaimTypes.PhoneNumberVerified,
                            },
                        }
                    }
                };

            List<Client> clients = new List<Client>()
                {
                    new Client()
                    {
                        Id = Snowflake<Client>.NextId(),
                        ClientId = "5954398486",
                        ClientName = "后台管理系统Vue客户端",
                        ClientUri = "",
                        RequireClientSecret = false,
                        RequireConsent = false,
                        AllowRememberConsent = true,
                        AllowPlainTextPkce = false,
                        AllowAccessTokensViaBrowser = true,
                        BackChannelLogoutSessionRequired = true,
                        AllowOfflineAccess = true,
                        UpdateAccessTokenClaimsOnRefresh = true,
                        AccessTokenType = 0,
                        AllowedCorsOrigins = new List<ClientCorsOrigin>()
                        {
                            new ClientCorsOrigin()
                            {
                                Origin = "http://localhost:8080",
                            }
                        },
                        AllowedGrantTypes = new List<ClientGrantType>()
                        {
                            new ClientGrantType()
                            {
                                GrantType = "authorization_code",
                            }
                        },
                        RequirePkce = true,
                        ClientSecrets = new List<ClientSecret>()
                        {
                            new ClientSecret()
                            {
                                Value = "oYjppIizd29W4eodalgf+Vry0MfyLunBPVZeFmOelvU="
                            }
                        },
                        RedirectUris = new List<ClientRedirectUri>()
                        {
                            new ClientRedirectUri()
                            {
                                RedirectUri = "http://localhost:8080/#/callback"
                            },
                            new ClientRedirectUri()
                            {
                                RedirectUri = "http://localhost:8080/#/refresh"
                            }
                        },
                        FrontChannelLogoutUri = "http://localhost:8080",
                        PostLogoutRedirectUris = new List<ClientPostLogoutRedirectUri>()
                        {
                            new ClientPostLogoutRedirectUri()
                            {
                                PostLogoutRedirectUri = "http://localhost:8080",
                            }
                        },
                        AllowedScopes = new List<ClientScope>()
                        {
                            new ClientScope()
                            {
                                Scope = JwtClaimTypes.Subject,
                            },
                            new ClientScope()
                            {
                                Scope = "openid",
                            },
                            new ClientScope()
                            {
                                Scope = JwtClaimTypes.Email,
                            },
                            new ClientScope()
                            {
                                Scope = JwtClaimTypes.PhoneNumber,
                            },
                            new ClientScope()
                            {
                                Scope = JwtClaimTypes.Profile,
                            },
                            new ClientScope()
                            {
                                Scope = JwtClaimTypes.Role,
                            },

                            new ClientScope()
                            {
                                Scope = "api1",
                            },
                        }
                    }
                };

            if(_configurationDbContext.IdentityResources.Select.Count() == 0)
            {
                foreach(var item in resources)
                {
                    if(item.UserClaims != null && item.UserClaims.Count > 0)
                    {
                        foreach(var q in item.UserClaims)
                        {
                            q.IdentityResourceId = item.Id;
                            _configurationDbContext.IdentityResourceClaims.Add(q);
                        }
                    }
                    _configurationDbContext.IdentityResources.Add(item);
                }
            }

            if(_configurationDbContext.ApiScopes.Select.Count() == 0)
            {
                foreach(var item in apiScopes)
                {
                    if(item.UserClaims != null && item.UserClaims.Count > 0)
                    {
                        foreach(var q in item.UserClaims)
                        {
                            q.ScopeId = item.Id;
                            _configurationDbContext.ApiScopeClaims.Add(q);
                        }
                    }
                    _configurationDbContext.ApiScopes.Add(item);
                }
            }

            if(_configurationDbContext.ApiResources.Select.Count() == 0)
            {
                foreach(var item in apiResources)
                {
                    if(item.Scopes != null && item.Scopes.Count > 0)
                    {
                        foreach(var q in item.Scopes)
                        {
                            q.ApiResourceId = item.Id;
                            _configurationDbContext.ApiResourceScopes.Add(q);
                        }
                    }
                    if(item.UserClaims != null && item.UserClaims.Count > 0)
                    {
                        foreach(var q in item.UserClaims)
                        {
                            q.ApiResourceId = item.Id;
                            _configurationDbContext.ApiResourceClaims.Add(q);
                        }
                    }
                    _configurationDbContext.ApiResources.Add(item);
                }
            }

            if(_configurationDbContext.Clients.Select.Count() == 0)
            {

                foreach(var item in clients)
                {
                    if(item.AllowedGrantTypes != null && item.AllowedGrantTypes.Count > 0)
                    {
                        foreach(var q in item.AllowedGrantTypes)
                        {
                            q.ClientId = item.Id;
                            _configurationDbContext.ClientGrantTypes.Add(q);
                        }
                    }
                    if(item.ClientSecrets != null && item.ClientSecrets.Count > 0)
                    {
                        foreach(var q in item.ClientSecrets)
                        {
                            q.ClientId = item.Id;
                            _configurationDbContext.ClientSecrets.Add(q);
                        }
                    }
                    if(item.RedirectUris != null && item.RedirectUris.Count > 0)
                    {
                        foreach(var q in item.RedirectUris)
                        {
                            q.ClientId = item.Id;
                            _configurationDbContext.ClientRedirectUris.Add(q);
                        }
                    }
                    if(item.PostLogoutRedirectUris != null && item.PostLogoutRedirectUris.Count > 0)
                    {
                        foreach(var q in item.PostLogoutRedirectUris)
                        {
                            q.ClientId = item.Id;
                            _configurationDbContext.ClientPostLogoutRedirectUris.Add(q);
                        }
                    }
                    if(item.AllowedCorsOrigins != null && item.AllowedCorsOrigins.Count > 0)
                    {
                        foreach(var q in item.AllowedCorsOrigins)
                        {
                            q.ClientId = item.Id;
                            _configurationDbContext.ClientCorsOrigins.Add(q);
                        }
                    }
                    if(item.AllowedScopes != null && item.AllowedScopes.Count > 0)
                    {
                        foreach(var q in item.AllowedScopes)
                        {
                            q.ClientId = item.Id;
                            _configurationDbContext.ClientScopes.Add(q);
                        }
                    }
                    _configurationDbContext.Clients.Add(item);
                }
                await _configurationDbContext.SaveChangesAsync();
            }

        }
    }
}
