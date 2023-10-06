using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ViazyNetCore.Filter.Descriptor
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
        List<string> GetPermissionKeys();

        /// <summary>
        /// 获取所有Api列表缓存key
        /// </summary>
        string GetAllApiKey { get; }
        /// <summary>
        /// 根据ApiId获取缓存Key
        /// </summary>
        string AnyByIdKey { get; }
    }
}
