using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class OpenIddictSteup
    {
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


                    .RegisterScopes(Scopes.Email, Scopes.Profile, Scopes.Roles)

                    //提供给API校验Jwt令牌使用是配置
                    .AddEncryptionKey(new SymmetricSecurityKey(
                                    Convert.FromBase64String("DRjd/nduI3Efze123nvbNUfc/=")))
                    // 加密凭证 、注册签名
                    .AddDevelopmentEncryptionCertificate().AddDevelopmentSigningCertificate()

                    //强制客户端应用程序使用 Proof Key Code Exchange (PKCE)
                    .RequireProofKeyForCodeExchange()
                    .Configure(options =>
                    {
                        options.CodeChallengeMethods.Add(CodeChallengeMethods.Plain);
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