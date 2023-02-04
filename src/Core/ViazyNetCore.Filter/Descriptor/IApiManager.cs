﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace ViazyNetCore.APIs
{
    /// <summary>
    /// 接口管理实例。
    /// </summary>
    public interface IApiManager
    {
        /// <summary>
        /// 获取所有API描述。
        /// </summary>
        /// <returns>返回API描述列表。</returns>
        List<ApiGroupDescriptor> GetApiDescriptors();

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="id">Url地址Md5</param>
        /// <returns></returns>
        Task<bool> AnyAsync(string id);
        /// <summary>
        /// 获取所有Api列表缓存key
        /// </summary>
        string GetAllApiKey { get; }
        /// <summary>
        /// 根据ApiId获取缓存Key
        /// </summary>
        string AnyByIdKey { get; }
    }
    /// <summary>
    /// 接口管理实现类。
    /// </summary>
    public class ApiManager : IApiManager
    {
        /// <summary>
        /// 内存缓存
        /// </summary>
        private readonly IMemoryCache _cache;
        /// <summary>
        /// 控制器
        /// </summary>
        private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;
        /// <summary>
        /// 配置
        /// </summary>
        private readonly ApiDescriptorOptions _options;
        /// <summary>
        /// 获取所有Api列表缓存key
        /// </summary>
        public string GetAllApiKey => $"{_options.CachePrefix}:GetApiDescriptors";

        /// <summary>
        /// 根据ApiId获取缓存Key
        /// </summary>
        public string AnyByIdKey => $"{_options.CachePrefix}:AnyAsync:";
        /// <summary>
        /// 初始化构造函数
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="actionDescriptorCollectionProvider"></param>
        /// <param name="options"></param>
        public ApiManager(IMemoryCache cache, IActionDescriptorCollectionProvider actionDescriptorCollectionProvider, ApiDescriptorOptions options)
        {
            _cache = cache;
            _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
            _options = options;
        }

        /// <summary>
        /// 获取所有API描述。
        /// </summary>
        /// <value>缓存前缀</value>
        /// <returns>返回API描述列表。</returns>
        public List<ApiGroupDescriptor> GetApiDescriptors()
        {
            var data = _cache.GetOrCreate(GetAllApiKey, options =>
            {
                options.SlidingExpiration = TimeSpan.FromDays(1);
                var apiGroupDescriptors = _actionDescriptorCollectionProvider.ActionDescriptors.Items
               .Select(x => x as ControllerActionDescriptor)
               .Where(x =>
               {
                   if (x == null)
                       return false;

                   if (x.ControllerTypeInfo.IsDefined(typeof(AllowAnonymousAttribute)))
                       return false;
                   if (x.MethodInfo.IsDefined(typeof(AllowAnonymousAttribute)))
                       return false;
                   if (!x.ControllerTypeInfo.IsDefined(typeof(ApiControllerAttribute)))
                       return false;
                   var settings = x.ControllerTypeInfo.GetCustomAttribute<ApiExplorerSettingsAttribute>();
                   return settings?.IgnoreApi != true;
               })
               .Select(x =>
               {
                   string? apiVersion = null;
                   //版本控制优先级Action(方法)->Controller(控制器)
                   if (x.MethodInfo.IsDefined(typeof(ApiVersionAttribute)))
                   {
                       var version = x.MethodInfo.GetCustomAttribute<ApiVersionAttribute>();
                       var v = version!.Versions.FirstOrDefault();
                       //添加业务逻辑带小版本位数为0,去除位数0。
                       apiVersion = v!.MinorVersion == 0 ? v.MajorVersion.ToString() : v?.ToString();
                   }
                   else if (x.ControllerTypeInfo.IsDefined(typeof(ApiVersionAttribute)))
                   {
                       var version = x.ControllerTypeInfo.GetCustomAttribute<ApiVersionAttribute>();
                       var v = version!.Versions.FirstOrDefault();
                       //添加业务逻辑带小版本位数为0,去除位数0。
                       apiVersion = v!.MinorVersion == 0 ? v.MajorVersion.ToString() : v?.ToString();
                   }
                   var controller = x.ControllerTypeInfo.GetCustomAttribute<ApiDescriptorAttribute>();
                   string controllerDisplayName = controller?.DisplayName ?? x.ControllerName;
                   string enControllerDisplayName = controller?.EnDisplayName ?? x.ControllerName;

                   var action = x.MethodInfo.GetCustomAttribute<ApiDescriptorAttribute>();
                   string actionDisplayName = action?.DisplayName ?? x.ActionName;
                   string enActionDisplayName = action?.EnDisplayName ?? x.ActionName;
                   var descriptor = new ApiDescriptor
                   {
                       ControllerName = x.ControllerName,
                       DisplayControllerName = controllerDisplayName,
                       ActionName = x.ActionName,
                       DisplayActionName = actionDisplayName,
                       RouteTemplate = x.AttributeRouteInfo?.Template,
                       HttpMethod = x.MethodInfo.GetHttpMethod(),
                       EnDisplayActionName = enActionDisplayName,
                       EnDisplayControllerName = enControllerDisplayName,
                       ApiVersion = apiVersion!,
                       ServiceName = _options.ServiceName
                   };
                   return descriptor;
               }).GroupBy(c => c.ControllerName).Select(c =>
               {
                   var first = c.FirstOrDefault();
                   var descriptor = new ApiGroupDescriptor()
                   {
                       ControllerName = c.Key,
                       DisplayControllerName = first?.DisplayControllerName,
                       EnDisplayControllerName = first?.EnDisplayControllerName,
                       Apis = c.ToList()
                   };
                   return descriptor;
               })
               .OrderBy(x => x.ControllerName)
               .ToList();
                return apiGroupDescriptors;
            });
            return data;
        }

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="id">Url地址Md5</param>
        /// <returns></returns>
        public Task<bool> AnyAsync(string id)
        {
            var apis = GetApiDescriptors();
            var status = apis.Any(a => a.Apis.Any(c => c.Id == id));
            return Task.FromResult(status);
        }


    }
}
