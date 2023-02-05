﻿using System.Net;
using System.Reflection;
using IGeekFan.AspNetCore.Knife4jUI;
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
using ViazyNetCore.Swagger;
using ViazyNetCore.Swagger.Filters;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SwaggerExtensions
    {
        private const string AUTHENTICATION_SCHEME = "Bearer";
        public static void AddSwagger(this IServiceCollection services, string apiName)
        {
            services.TryAddEnumerable(ServiceDescriptor.Transient<IApplicationModelProvider, ProduceResponseTypeModelProvider>());

            services.AddSwaggerGen(options =>
            {
                var versionProvider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var description in versionProvider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc($"{description.GroupName}", new OpenApiInfo
                    {
                        Title = $"{description.GroupName}-{apiName}-接口文档",
                        Description = $"{apiName} HTTP API {description.GroupName}",
                        Version = $"{description.GroupName}",
                    });
                }
                options.SchemaFilter<AutoRestSchemaFilter>();
                var dir = new DirectoryInfo(AppContext.BaseDirectory);
                var files = dir.GetFiles("*.xml", SearchOption.TopDirectoryOnly);
                foreach (var file in files)
                {
                    options.IncludeXmlComments(file.FullName, true);
                }
                // 启用 SwaggerResponse 备注
                options.EnableAnnotations();
                options.CustomOperationIds(apiDesc =>
                {
                    if (apiDesc.ActionDescriptor is ControllerActionDescriptor controllerAction)
                        return controllerAction.ControllerName + "-" + controllerAction.ActionName;
                    else
                        return string.Empty;
                });

                options.DocumentFilter<SwaggerEnumFilter>();

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

        public static void UseSwaggerAndUI(this WebApplication app, string apiName, Action<List<OpenApiServer>> action = null)
        {

            if (app.Environment.IsDevelopment())
            {
                var versionProvider = app.Services.GetService<IApiVersionDescriptionProvider>();
                app.UseSwagger(options =>
                {
                    options.PreSerializeFilters.Add((swagger, httpReq) =>
                    {
                        var servers = new List<OpenApiServer>();
                        if (httpReq.IsLocal())
                        {
                            servers.Add(new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}", Description = "本地环境" });
                        }
                        action?.Invoke(servers);
                        swagger.Servers = servers;
                    });
                });

                app.UseKnife4UI(options =>
                {
                    options.RoutePrefix = "swagger"; // serve the UI at root
                    foreach (var description in versionProvider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"{description.GroupName}/swagger.json", "HTTP API " + description.GroupName);
                    }
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
            if (connection.RemoteIpAddress.IsSet())
            {
                if (!connection.LocalIpAddress.IsSet())
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

    internal class AutoRestSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            //schema.Extensions.Add("format", null);
            schema.Format = null;
        }
    }
}
