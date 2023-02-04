using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using System.Collections.Generic;

namespace ViazyNetCore.Filter
{
    /// <summary>
    /// API实体。
    /// </summary>
    public class ApiDescriptor
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ApiDescriptor() { }

        /// <summary>
        /// 路由地址(API接口地址)唯一标识Guid初始化构造函数
        /// </summary>
        public ApiDescriptor(string serviceName, string apiVersion, string routeTemplate)
        {
            this.ServiceName = serviceName;
            this.ApiVersion = apiVersion;
            this.RouteTemplate = routeTemplate;
        }

        /// <summary>
        /// 唯一标识Guid（服务名称+路由地址）
        /// </summary>
        public string Id => $"{this.ServiceName}{this.RoutePath}".ToMd5();
        /// <summary>
        /// 服务名称
        /// </summary>
        public string? ServiceName { get; set; }
        /// <summary>
        /// API版本
        /// </summary>
        public string ApiVersion { get; set; }
        /// <summary>
        /// 控制名称。
        /// </summary>
        public string? ControllerName { get; set; }
        /// <summary>
        /// 显示控制器名称
        /// </summary>
        public string? DisplayControllerName { get; set; }
        /// <summary>
        /// 英文控制器名称
        /// </summary>
        public string? EnDisplayControllerName { get; set; }
        /// <summary>
        /// 方法名称。
        /// </summary>
        public string? ActionName { get; set; }
        /// <summary>
        /// 显示方法名称
        /// </summary>
        public string? DisplayActionName { get; set; }
        /// <summary>
        /// 英文显示方法名称
        /// </summary>
        public string? EnDisplayActionName { get; set; }
        /// <summary>
        /// 方法描述
        /// </summary>
        public string? ActionDescriptor { get; set; }
        /// <summary>
        /// 路由模板。
        /// </summary>
        public string? RouteTemplate { get; set; }

        /// <summary>
        /// 路由完整地址
        /// </summary>
        public string? RoutePath => this.ApiVersion.IsNull() ? this.RouteTemplate : this.RouteTemplate?.Replace("{v:apiVersion}", this.ApiVersion);

        /// <summary>
        /// HTTP方法。
        /// </summary>
        public string HttpMethod { get; set; }

    }
    /// <summary>
    /// api分组
    /// </summary>
    public class ApiGroupDescriptor
    {
        /// <summary>
        /// 控制名称。
        /// </summary>
        public string ControllerName { get; set; }
        /// <summary>
        /// 控制显示名称。
        /// </summary>
        public string DisplayControllerName { get; set; }
        /// <summary>
        /// 英文控制器名称
        /// </summary>
        public string EnDisplayControllerName { get; set; }
        /// <summary>
        /// API集合
        /// </summary>
        public List<ApiDescriptor> Apis { get; set; } = new List<ApiDescriptor>();
    }
}