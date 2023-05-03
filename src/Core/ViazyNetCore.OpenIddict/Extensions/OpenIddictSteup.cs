using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;
using ViazyNetCore.OpenIddict.AspNetCore;
using ViazyNetCore.OpenIddict.AspNetCore.ExtensionGrantTypes;
using ViazyNetCore.OpenIddict.Domain;
using ViazyNetCore.OpenIddict;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Security.Principal;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class OpenIddictSteup
    {   
        public static OpenIddictBuilder AddOpenIddictServer(this IServiceCollection services)
        {
            services.AddSingleton<OpenIddictClaimDestinationsManager>();

            services.AddScoped<IOpenIddictApplicationRepository, OpenIddictApplicationRepository>();
            services.AddScoped<IOpenIddictAuthorizationRepository, OpenIddictAuthorizationRepository>();
            services.AddScoped<IOpenIddictScopeRepository, OpenIddictScopeRepository>();
            services.AddScoped<IOpenIddictTokenRepository, OpenIddictTokenRepository>();

            var openIddictBuilder = services.AddOpenIddict()
             .AddServer(builder =>
             {
                 builder
                   .SetAuthorizationEndpointUris("connect/authorize", "connect/authorize/callback")
                   // .well-known/oauth-authorization-server
                   // .well-known/openid-configuration
                   //.SetConfigurationEndpointUris()
                   // .well-known/jwks
                   //.SetCryptographyEndpointUris()
                   .SetDeviceEndpointUris("device")
                   .SetIntrospectionEndpointUris("connect/introspect")
                   .SetLogoutEndpointUris("connect/logout")
                   .SetRevocationEndpointUris("connect/revocat")
                   .SetTokenEndpointUris("connect/token")
                   .SetUserinfoEndpointUris("connect/userinfo")
                   .SetVerificationEndpointUris("connect/verify");

                 builder
                     .AllowAuthorizationCodeFlow()
                     .AllowHybridFlow()
                     .AllowImplicitFlow()
                     .AllowPasswordFlow()
                     .AllowClientCredentialsFlow()
                     .AllowRefreshTokenFlow()
                     .AllowDeviceCodeFlow()
                     .AllowNoneFlow();

                 builder.RegisterScopes(new[]
                      {
                            OpenIddictConstants.Scopes.OpenId,
                            OpenIddictConstants.Scopes.Email,
                            OpenIddictConstants.Scopes.Profile,
                            OpenIddictConstants.Scopes.Phone,
                            OpenIddictConstants.Scopes.Roles,
                            OpenIddictConstants.Scopes.Address,
                            OpenIddictConstants.Scopes.OfflineAccess
                        });

                 builder.UseAspNetCore()
                   .EnableAuthorizationEndpointPassthrough()
                   .EnableTokenEndpointPassthrough()
                   .EnableUserinfoEndpointPassthrough()
                   .EnableLogoutEndpointPassthrough()
                   .EnableVerificationEndpointPassthrough()
                   .EnableStatusCodePagesIntegration();

                 //builder.AddDevelopmentEncryptionCertificate();

                 using (var algorithm = RSA.Create(keySizeInBits: 2048))
                 {
                     var subject = new X500DistinguishedName("CN=Fabrikam Encryption Certificate");
                     var request = new CertificateRequest(subject, algorithm, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                     request.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature, critical: true));
                     var certificate = request.CreateSelfSigned(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddYears(2));
                     builder.AddSigningCertificate(certificate);
                 }

                 using (var algorithm = RSA.Create(keySizeInBits: 2048))
                 {
                     var subject = new X500DistinguishedName("CN=Fabrikam Signing Certificate");
                     var request = new CertificateRequest(subject, algorithm, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                     request.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.KeyEncipherment, critical: true));
                     var certificate = request.CreateSelfSigned(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddYears(2));
                     builder.AddEncryptionCertificate(certificate);
                 }

                 builder.Configure(openIddictServerOptions =>
                 {
                     openIddictServerOptions.GrantTypes.Add(MyTokenExtensionGrant.ExtensionGrantName);
                 });
             })
             .AddCore(builder => {

                 builder
                     .SetDefaultApplicationEntity<OpenIddictApplicationDto>()
                     .SetDefaultAuthorizationEntity<OpenIddictAuthorizationDto>()
                     .SetDefaultScopeEntity<OpenIddictScopeDto>()
                     .SetDefaultTokenEntity<OpenIddictTokenDto>();
                 builder
                   .AddApplicationStore<OpenIddictApplicationStore>()
                   .AddAuthorizationStore<OpenIddictAuthorizationStore>()
                   .AddScopeStore<OpenIddictScopeStore>()
                   .AddTokenStore<OpenIddictTokenStore>();

                 builder.ReplaceApplicationManager(typeof(ApplicationManager));
                 //builder.ReplaceAuthorizationManager(typeof(AuthorizationManager));
                 //builder.ReplaceScopeManager(typeof(ScopeManager));
                 //builder.ReplaceTokenManager(typeof(TokenManager));
             });

            openIddictBuilder.AddValidation(options =>
            {
                options.UseLocalServer();
                //强制授权条目验证 出于性能原因，OpenIddict 3.0在接收API请求时默认不检查授权条目的状态:即使附加的授权被撤销
                //，访问令牌也被认为是有效的
                options.EnableAuthorizationEntryValidation();
                options.UseAspNetCore();
            });

            return openIddictBuilder;
        }

        public static void ConfigureOpenIddictServices(this IServiceCollection services)
        {
            VaizyClaimTypes.UserId = OpenIddictConstants.Claims.Subject;
            VaizyClaimTypes.Role = OpenIddictConstants.Claims.Role;
            VaizyClaimTypes.UserName = OpenIddictConstants.Claims.PreferredUsername;
            VaizyClaimTypes.Name = OpenIddictConstants.Claims.GivenName;
            VaizyClaimTypes.SurName = OpenIddictConstants.Claims.FamilyName;
            VaizyClaimTypes.PhoneNumber = OpenIddictConstants.Claims.PhoneNumber;
            VaizyClaimTypes.PhoneNumberVerified = OpenIddictConstants.Claims.PhoneNumberVerified;
            VaizyClaimTypes.Email = OpenIddictConstants.Claims.Email;
            VaizyClaimTypes.EmailVerified = OpenIddictConstants.Claims.EmailVerified;
            VaizyClaimTypes.ClientId = OpenIddictConstants.Claims.ClientId;

            services.AddOpenIddictServer();

            services.Configure<OpenIddictOptions>(options =>
            {
                options.DbKey = "master";
            });
            services.Configure<OpenIddictClaimDestinationsOptions>(options =>
            {
                options.ClaimDestinationsProvider.Add<DefaultOpenIddictClaimDestinationsProvider>();
            });
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationFormats.Add("/Views/{1}/{0}.cshtml");
            });
        }

        public static IApplicationBuilder UseViazyOpenIddictValidation(this IApplicationBuilder app, string schema = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
        {
            return app.Use(async (ctx, next) =>
            {
                if (ctx.User.Identity?.IsAuthenticated != true)
                {
                    var result = await ctx.AuthenticateAsync(schema);
                    if (result.Succeeded && result.Principal != null)
                    {
                        ctx.User = result.Principal;
                    }
                }

                await next();
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="constr"></param>
        /// <returns></returns>
        public static IServiceCollection AddOpenIddictAuth(this IServiceCollection services, string constr)
        {
            //services.AddDbContext<OpenIdDbContext>(options =>
            //{

            //    options.UseMySql(constr, ServerVersion.AutoDetect(constr), builder =>
            //    {
            //        builder.UseRelationalNulls();
            //        builder.MigrationsAssembly("OpenIdService");

            //    });
            //    options.UseOpenIddict();
            //}).AddQuartz(options =>
            //{
            //    options.UseMicrosoftDependencyInjectionJobFactory();
            //    options.UseSimpleTypeLoader();
            //    options.UseInMemoryStore();
            //})
            //.AddQuartzHostedService(options => options.WaitForJobsToComplete = true)
            ;

            services.AddOpenIddict()
                .AddCore(options =>
                {
                    //配置OpenIddict以使用EntityFrameworkCore存储和模型。 注意: 调用replacedefaultenentities()来替换默认的OpenIddict实体。
                    //options.UseEntityFrameworkCore().UseDbContext<OpenIdDbContext>();
                    //喜欢使用MongoDB的开发人员可以删除前面的代码行并配置OpenIddict使用指定的MongoDB数据库:
                    // options.UseMongoDb() .UseDatabase(new MongoClient().GetDatabase("openiddict"));
                    options.UseQuartz();
                })
                .AddServer(options =>
                {

                    //配置交互服务地址
                    options.SetAuthorizationEndpointUris("/connect/authorize")
                    .SetDeviceEndpointUris("/connect/device")
                    .SetIntrospectionEndpointUris("/connect/introspect")
                    .SetRevocationEndpointUris("/connect/revocat")
                    .SetUserinfoEndpointUris("/connect/userinfo")
                    .SetVerificationEndpointUris("/connect/verify")
                    .SetLogoutEndpointUris("/connect/logout")
                    .SetTokenEndpointUris("/connect/token")

                    //这是允许的模式
                    .AllowAuthorizationCodeFlow()
                    .AllowClientCredentialsFlow()
                    .AllowDeviceCodeFlow()
                    .AllowHybridFlow()
                    .AllowImplicitFlow()
                    .AllowPasswordFlow()
                    .AllowRefreshTokenFlow()


                    .RegisterScopes(OpenIddictConstants.Scopes.Email, OpenIddictConstants.Scopes.Profile, OpenIddictConstants.Scopes.Roles)

                    //提供给API校验Jwt令牌使用是配置
                    .AddEncryptionKey(new SymmetricSecurityKey(
                                    Convert.FromBase64String("DRjd/nduI3Efze123nvbNUfc/=")))
                    // 加密凭证 、注册签名
                    .AddDevelopmentEncryptionCertificate().AddDevelopmentSigningCertificate()

                    //强制客户端应用程序使用 Proof Key Code Exchange (PKCE)
                    .RequireProofKeyForCodeExchange()
                    .Configure(options =>
                    {
                        options.CodeChallengeMethods.Add(OpenIddictConstants.CodeChallengeMethods.Plain);
                    })
                    //配置 启用通过后的后续处理
                    .UseAspNetCore().EnableStatusCodePagesIntegration()
                                    .EnableAuthorizationEndpointPassthrough()
                                    .EnableLogoutEndpointPassthrough()
                                    .EnableTokenEndpointPassthrough()
                                    .EnableUserinfoEndpointPassthrough()
                                    .EnableVerificationEndpointPassthrough()
                                    .DisableTransportSecurityRequirement(); // 禁用HTTPS 在开发测试环境

                    #region 禁用忽略选项配置
                    //禁用授权信息存储
                    // options.DisableAuthorizationStorage();
                    // options.AcceptAnonymousClients();
                    // options.DisableScopeValidation();
                    // options.IgnoreEndpointPermissions()
                    //        .IgnoreGrantTypePermissions()
                    //        .IgnoreResponseTypePermissions()
                    //        .IgnoreScopePermissions();
                    options.DisableAccessTokenEncryption();
                    #endregion

                }).AddValidation(options =>
                {
                    options.UseLocalServer();
                    //强制授权条目验证 出于性能原因，OpenIddict 3.0在接收API请求时默认不检查授权条目的状态:即使附加的授权被撤销
                    //，访问令牌也被认为是有效的
                    options.EnableAuthorizationEntryValidation();
                    options.UseAspNetCore();

                });

            return services;
        }
    }
}