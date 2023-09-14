using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using ViazyNetCore.AttachmentProvider.Hosts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace ViazyNetCore.AttachmentProvider
{
    /// <summary>
    /// 定义附件服务操作的实用工具方法。
    /// </summary>
    public interface IStoreProvider
    {
        /// <summary>
        /// 获取或设置HttpClientName
        /// </summary>
        string HttpClientName { get; set; }

        /// <summary>
        ///  获取HttpClientName
        /// </summary>
        ILocalStoreHost LocalStoreHost { get; }

        /// <summary>
        /// LiteDB 数据库文件路径
        /// </summary>
        string LiteDBFilePath { get; set; }

        /// <summary>
        /// 添加附件服务器操作的Host 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options"></param>
        /// <returns></returns>
        IStoreProvider AddHost<T>(IStoreHostOptions options) where T : IStoreHost;

        /// <summary>
        /// 添加附件服务器操作的本地Host 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options"></param>
        /// <returns></returns>
        IStoreProvider AddLocalHost<T>(IStoreHostOptions options) where T : ILocalStoreHost;

        /// <summary>
        /// 从 <see cref="HttpClientName"/> 创建一个<see cref="HttpClient"/> 对象
        /// </summary>
        /// <returns></returns>
        HttpClient GetHttpClient();


        /// <summary>
        /// 附件上传并保存
        /// </summary>
        /// <param name="formFile"></param>
        /// <param name="contentType"></param>
        Task<Attachment> SaveAsync(IFormFile formFile, string contentType = null);

        /// <summary>
        /// 附件上传并保存
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileName"></param>
        Task<Attachment> SaveAsync(Stream stream, string fileName);
    }

    /// <summary>
    /// 提供附件服务操作的实用工具方法。
    /// </summary>
    public class StoreProvider : IStoreProvider
    {
        private readonly IServiceProvider _services;
        private readonly IPathFormatter _pathFormatter;
        private readonly List<IStoreHost> _hosts;
        /// <summary>
        /// 
        /// </summary>
        public ILocalStoreHost LocalStoreHost { get; private set; }
        private readonly ILogger<IStoreProvider> _logger;

        /// <summary>
        /// 初始化附件服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="pathFormatter"></param>
        /// <param name="logger"></param>
        public StoreProvider(IServiceProvider services, IPathFormatter pathFormatter, ILogger<StoreProvider> logger)
        {
            this._services = services;
            this._pathFormatter = pathFormatter;
            this._hosts = new List<IStoreHost>();
            this._logger = logger;
        }

        /// <summary>
        /// 获取或设置HttpClientName
        /// </summary>
        public string HttpClientName { get; set; } = "ImageClients";

        /// <summary>
        /// LiteDB 数据库文件路径
        /// </summary>
        public string LiteDBFilePath { get; set; }

        /// <summary>
        /// 添加附件服务器操作的本地Host 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options"></param>
        /// <returns></returns>
        public IStoreProvider AddLocalHost<T>(IStoreHostOptions options) where T : ILocalStoreHost
        {
            this.LocalStoreHost = this._services.CreateInstance<T>(this, options);
            return this;
        }


        /// <summary>
        /// 添加附件服务器操作的本地Host 
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public IStoreProvider AddLocalHost(LocalStoreOptions options)
        {
            this.LocalStoreHost = this._services.CreateInstance<LocalStoreHost>(this, new LocalStoreProvider(options.StoreRootPath), options);
            return this;
        }

        /// <summary>
        /// 添加附件服务器操作的Host 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options"></param>
        /// <returns></returns>
        public IStoreProvider AddHost<T>(IStoreHostOptions options) where T : IStoreHost
        {
            this._hosts.Add(this._services.CreateInstance<T>(this, options));
            return this;
        }

        /// <summary>
        /// 从 <see cref="HttpClientName"/> 创建一个<see cref="HttpClient"/> 对象
        /// </summary>
        /// <returns>返回一个 <see cref="HttpClient"/></returns>
        public HttpClient GetHttpClient()
        {
            return this._services.GetService<IHttpClientFactory>().CreateClient(this.HttpClientName);
        }

      
        /// <summary>
        /// 附件上传并保存
        /// </summary>
        /// <param name="formFile"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public async Task<Attachment> SaveAsync(IFormFile formFile, string contentType = null)
        {
            var attachment = new Attachment(formFile, this._pathFormatter, contentType)
            {
                Id = Snowflake<Attachment>.NextIdString()
            };
            if(this.LocalStoreHost != null)
            {
                if(this.LocalStoreHost.Options.MediaTypes != null && !this.LocalStoreHost.Options.MediaTypes.Contains(attachment.MediaType))
                    throw new NotImplementedException("The uploaded file is of invalid media type!");
                var storeFile = (DefaultStoreFile)await this.LocalStoreHost.LocalUploadAsync(attachment, formFile.OpenReadStream());
                attachment.FileLocalUrl = storeFile.FullLocalPath;

                var path = storeFile.RelativePath;
                if(path[0] == '/' || path[0] == '\\') path = path[1..];
                attachment.RelativeUrl = Path.Combine(this.LocalStoreHost.Options.ServerUrl, path).Replace('\\', '/');
            }
            if(this._hosts.Count > 0)
            {
                foreach(var host in this._hosts)
                {
                    if(host.Options.MediaTypes != null && host.Options.MediaTypes.Contains(attachment.MediaType))
                    {
                        var fileUrl = await host.UploadAsync(attachment.FileName, formFile.OpenReadStream());
                        attachment.AddHostFile(fileUrl, host.Name);
                        break;
                    }
                }
            }

            return attachment;
        }

        /// <summary>
        /// 附件上传并保存
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<Attachment> SaveAsync(Stream stream, string fileName)
        {
            var attachment = new Attachment(stream, null, fileName)
            {
                Id = Snowflake<Attachment>.NextIdString()
            };

            if(this.LocalStoreHost != null)
            {
                var fileLocalUrl = await this.LocalStoreHost.UploadAsync(fileName, stream);
                attachment.FileLocalUrl = fileLocalUrl;
            }
            if(this._hosts.Count > 0)
            {
                foreach(var host in this._hosts)
                {
                    if(host.Options.MediaTypes != null && host.Options.MediaTypes.Contains(attachment.MediaType))
                    {
                        var fileUrl = await host.UploadAsync(fileName, stream);
                        attachment.AddHostFile(fileUrl, host.Name);
                    }
                }
            }

            return attachment;
        }

    }
}
