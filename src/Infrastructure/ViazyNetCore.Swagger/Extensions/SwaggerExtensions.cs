using System.Net;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using ViazyNetCore;
using ViazyNetCore.Swagger;
using ViazyNetCore.Swagger.Filters;
using ViazyNetCore.Swagger.Knife4jUI;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SwaggerExtensions
    {
        private const string AUTHENTICATION_SCHEME = "Bearer";
        public static void AddSwagger(this IServiceCollection services, string apiName, Action<SwaggerConfig> action)
        {
            var swaggerConfig = new SwaggerConfig();
            action?.Invoke(swaggerConfig);
            services.Configure(action);
            services.TryAddEnumerable(ServiceDescriptor.Transient<IApplicationModelProvider, ProduceResponseTypeModelProvider>());

            services.AddSwaggerGen(options =>
            {
                //options.DocumentFilter<DynamicDocumentFilter>();
                //支持多分组
                options.DocInclusionPredicate((docName, apiDescription) =>
                {
                    var nonGroup = false;
                    var groupNames = new List<string>();
                    var dynamicApiAttribute = apiDescription.ActionDescriptor.EndpointMetadata.FirstOrDefault(x => x is DynamicApiAttribute);
                    if (dynamicApiAttribute != null)
                    {
                        var dynamicApi = dynamicApiAttribute as DynamicApiAttribute;
                        if (dynamicApi?.GroupNames?.Length > 0)
                        {
                            groupNames.AddRange(dynamicApi.GroupNames);
                        }
                    }
                    var controllerGroupAttributes = apiDescription.ActionDescriptor.EndpointMetadata.FirstOrDefault(x => x is ControllerGroupAttribute);
                    if (controllerGroupAttributes is ControllerGroupAttribute controllerGroup)
                    {
                        if (controllerGroup.GroupNames?.Length > 0)
                        {
                            groupNames.AddRange(controllerGroup.GroupNames);
                        }
                        nonGroup = controllerGroup.NonGroup;
                    }

                    return docName == apiDescription.GroupName || groupNames.Any(a => a == docName) || nonGroup;
                });

                //options.ResolveConflictingActions(apiDescription => apiDescription.First());

                foreach (var project in swaggerConfig.Projects)
                {
                    options.SwaggerDoc(project.Code.ToLower(), new OpenApiInfo
                    {
                        Title = project.Name,
                        Version = project.Version,
                        Description = project.Description
                    });
                }

                options.SchemaFilter<AutoRestSchemaFilter>();
                var dir = new DirectoryInfo(AppContext.BaseDirectory);
                var files = dir.GetFiles("*.xml", SearchOption.TopDirectoryOnly);
                foreach (var file in files)
                {
                    options.IncludeXmlComments(file.FullName, true);
                }
                if (swaggerConfig.EnableEnumSchemaFilter)
                {
                    options.DocumentFilter<SwaggerEnumFilter>();
                }
                // 启用 SwaggerResponse 备注
                options.EnableAnnotations();
                options.CustomOperationIds(apiDesc =>
                {
                    if (apiDesc.ActionDescriptor is ControllerActionDescriptor controllerAction)
                        return controllerAction.ControllerName.Replace("Controller", "") + controllerAction.ActionName;
                    else
                        return string.Empty;
                });

                #region Jwt Authentication

                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authorization",
                    Description = "Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = AUTHENTICATION_SCHEME,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme, Array.Empty<string>()}
                });

                #endregion

            }).AddSwaggerGenNewtonsoftSupport();
        }

        public static void UseSwaggerAndUI(this WebApplication app)
        {

            if (app.Environment.IsDevelopment())
            {
                var swaggerConfig = app.Services.GetService<IOptions<SwaggerConfig>>();
                if (swaggerConfig == null)
                    throw new NotImplementedException($"{nameof(SwaggerConfig)} 未注入");

                var swaggerGenOptions = app.Services.GetService<IOptions<SwaggerGenOptions>>();
                var routePrefix = swaggerConfig.Value.RoutePrefix;
                var routePath = routePrefix.IsNotNull() ? $"{routePrefix}/" : "";

                app.UseSwagger(options =>
                {
                    options.PreSerializeFilters.Add((swagger, httpReq) =>
                    {
                        var servers = new List<OpenApiServer>();
                        if (httpReq.IsLocal())
                        {
                            servers.Add(new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}", Description = "本地环境" });
                        }
                        //action?.Invoke(servers);
                        foreach (var item in swaggerConfig.Value.ApiServer)
                        {
                            servers.Add(new OpenApiServer { Url = $"{item.Scheme}://{item.Host.Value}", Description = item.Description });
                        }
                        swagger.Servers = servers;
                    });
                });
                app.UseKnife4UI(options =>
                {
                    options.RoutePrefix = routePrefix;
                    swaggerConfig.Value.Projects?.ForEach(project =>
                    {
                        options.SwaggerEndpoint($"/{project.Code.ToLower()}/swagger.json", project.Name);
                    });
                });

                //app.MapSwagger("/k4/{documentName}/swagger.json");
            }
        }
    }

    internal static class SwaggerUtilHelper
    {
        public static bool IsLocal(this HttpRequest req)
        {
            ConnectionInfo connection = req.HttpContext.Connection;
            if (connection.RemoteIpAddress == null) return false;
            if (connection.RemoteIpAddress.IsSet())
            {
                if (connection.LocalIpAddress?.IsSet() == false)
                {
                    return IPAddress.IsLoopback(connection.RemoteIpAddress);
                }

                return connection.RemoteIpAddress!.Equals(connection.LocalIpAddress);
            }

            return true;
        }

        private static bool IsSet(this IPAddress address)
        {
            if (address != null)
            {
                return address.ToString() != "::1";
            }

            return false;
        }
    }
}
