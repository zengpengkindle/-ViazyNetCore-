using System.Collections.Generic;
using System.IO;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace System
{
    /// <summary>
    /// 表示目录提供程序的扩展库。
    /// </summary>
    public static class DirectoryProviderExtensions
    {
        /// <summary>
        /// 获取子目录。
        /// </summary>
        /// <param name="provider">文件提供程序。</param>
        /// <param name="subpath">子路径。</param>
        /// <param name="createWithNotExists">当文件夹不存在时是否自动创建。</param>
        /// <returns>目录提供程序。</returns>
        public static IDirectoryInfo GetDirectoryInfo(this IFileProvider provider, string subpath, bool createWithNotExists = false)
        {
            if(provider is null)
                throw new ArgumentNullException(nameof(provider));

            if(subpath is null)
                throw new ArgumentNullException(nameof(subpath));

            if(subpath.Length == 0) return provider.ToDirectoryInfo();

            if(subpath[0] == '\\' || subpath[0] == '/') subpath = subpath.Substring(1);
            var directoryInfo = provider.ToDirectoryInfo();
            if(directoryInfo is null) return null;
            if(provider is IDirectoryProvider dp) return dp.GetDirectoryInfo(subpath, createWithNotExists);
            return new PhysicalDirectoryInfo(Path.Combine(directoryInfo.PhysicalPath, subpath), createWithNotExists);
        }

        ///// <summary>
        ///// 获取子目录。
        ///// </summary>
        ///// <param name="provider">文件提供程序。</param>
        ///// <param name="searchPattern">目录名称匹配的搜索字符串成。 此参数可以包含有效文本路径和通配符（* 和 ?）的组合，但不支持正则表达式。</param>
        ///// <returns>目录提供程序。</returns>
        //public static IEnumerable<IDirectoryInfo> GetDirectoryInfos(this IDirectoryInfo provider, string searchPattern = null)
        //{
        //    if(provider is null)
        //        throw new ArgumentNullException(nameof(provider));

        //    foreach(var item in Directory.EnumerateDirectories(provider.PhysicalPath, searchPattern, SearchOption.TopDirectoryOnly))
        //    {
        //        yield return new PhysicalDirectoryInfo(item, false);
        //    }
        //}

        /// <summary>
        /// 转换为目录提供程序。
        /// </summary>
        /// <param name="provider">文件提供程序。</param>
        /// <returns>目录提供程序。</returns>
        public static IDirectoryInfo ToDirectoryInfo(this IFileProvider provider)
        {
            if(provider is IDirectoryInfo di) return di;
            if(provider is PhysicalFileProvider pfp) return new PhysicalDirectoryInfo(pfp.Root, false);
            throw new NotSupportedException($"Not supported convertion type '{provider.GetType().FullName}' to directory provider."); //- 不支持类型 {provider.GetType().FullName} 转换为目录提供程序。
        }

        /// <summary>
        /// 遍历子目录。
        /// </summary>
        /// <param name="provider">文件提供程序。</param>
        /// <returns>子目录。</returns>
        public static IEnumerable<IDirectoryInfo> EnumerateDirectories(this IFileProvider provider)
        {
            if(provider is PhysicalFileProvider pfp)
            {
                foreach(var item in Directory.EnumerateDirectories(pfp.Root))
                {
                    yield return new PhysicalDirectoryInfo(item, false);
                }
            }
            else if(provider is PhysicalDirectoryInfo pdi)
            {
                foreach(var item in Directory.EnumerateDirectories(pdi.PhysicalPath))
                {
                    yield return new PhysicalDirectoryInfo(item, false);
                }
            }
            else if(provider is IDirectoryProvider dp)
            {
                foreach(var item in dp.EnumerateDirectories())
                {
                    yield return item;
                }
            }
            else
            {
                throw new NotSupportedException($"Not supported type '{provider.GetType().FullName}' enumerate directories.");
            }
        }

        /// <summary>
        /// 遍历子文件。
        /// </summary>
        /// <param name="provider">文件提供程序。</param>
        /// <returns>子文件。</returns>
        public static IEnumerable<IFileInfo> EnumerateFiles(this IFileProvider provider)
        {
            if(provider is PhysicalFileProvider pfp)
            {
                foreach(var item in Directory.EnumerateFiles(pfp.Root))
                {
                    yield return new Microsoft.Extensions.FileProviders.Physical.PhysicalFileInfo(new FileInfo(item));
                }
            }
            else if(provider is IDirectoryProvider dp)
            {
                foreach(var item in dp.EnumerateFiles())
                {
                    yield return item;
                }
            }
            else
            {
                throw new NotSupportedException($"Not supported type '{provider.GetType().FullName}' enumerate directories.");
            }
        }

        /// <summary>
        /// 获取基于根目录的子文件。
        /// </summary>
        /// <param name="context">HTTP 上下文。</param>
        /// <param name="subpath">路径。</param>
        /// <returns>子文件。</returns>
        public static IFileInfo Content(this HttpContext context, string subpath)
        {
            return context.RequestServices.GetRequiredService<IWebHostEnvironment>().ContentRootFileProvider.GetFileInfo(subpath);
        }

        /// <summary>
        /// 获取基于根目录的子目录。
        /// </summary>
        /// <param name="context">HTTP 上下文。</param>
        /// <param name="subpath">路径。</param>
        /// <returns>子目录。</returns>
        public static IDirectoryInfo ContentFolder(this HttpContext context, string subpath)
        {
            return context.RequestServices.GetRequiredService<IWebHostEnvironment>().ContentRootFileProvider.GetDirectoryInfo(subpath);
        }

        /// <summary>
        /// 获取基于 wwwroot 的子文件。
        /// </summary>
        /// <param name="context">HTTP 上下文。</param>
        /// <param name="subpath">路径。</param>
        /// <returns>子文件。</returns>
        public static IFileInfo Web(this HttpContext context, string subpath)
        {
            return context.RequestServices.GetRequiredService<IWebHostEnvironment>().WebRootFileProvider.GetFileInfo(subpath);
        }

        /// <summary>
        /// 获取基于 wwwroot 的子目录。
        /// </summary>
        /// <param name="context">HTTP 上下文。</param>
        /// <param name="subpath">路径。</param>
        /// <returns>子目录。</returns>
        public static IDirectoryInfo WebFolder(this HttpContext context, string subpath)
        {
            return context.RequestServices.GetRequiredService<IWebHostEnvironment>().WebRootFileProvider.GetDirectoryInfo(subpath);
        }
    }
}
