using System;
using Microsoft.Extensions.Logging;

namespace ViazyNetCore.AttachmentProvider.Hosts
{
    /// <summary>
    /// 网络服务接口的配置
    /// </summary>
    public class NetStoreHostOptions : IStoreHostOptions
    {
        /// <summary>
        /// 服务器地址
        /// </summary>
        public string ServerUrl { get; set; }

        /// <summary>
        /// 接口地址
        /// </summary>
        public string ActionUrl { get; set; }

        /// <summary>
        /// 上传类型限制
        /// </summary>
        public List<MediaType> MediaTypes { get; set; } = new List<MediaType> { MediaType.Image };

        /// <summary>
        /// 上传大小限制，单位B
        /// </summary>
        public long FileMaxSize { get; set; } = 5 * 1024 * 1024;

        /// <summary>
        /// 附件服务器空间限制，单位MB
        /// </summary>
        public long StroeMaxSize { get; set; } = -1;

        /// <summary>
        /// 允许上传图片格式
        /// </summary>
        public List<string> ImageAllowFiles { get; set; } = DefaultStoreHostOptions.ImageAllowFiles;

        /// <summary>
        /// 允许上传文件格式
        /// </summary>
        public List<string> FileAllowFiles { get; set; } = DefaultStoreHostOptions.FileAllowFiles;
    }

    /// <summary>
    /// 网络存储
    /// </summary>
    public class NetStoreHost : IStoreHost
    {
        private readonly IStoreProvider _storeProvider;
        private readonly ILogger<NetStoreHost> _logger;
        private readonly NetStoreHostOptions _options;

        string IStoreHost.Name => "NetStore";

        IStoreHostOptions IStoreHost.Options => this._options;

        /// <summary>
        /// 设置存储器已使用空间
        /// </summary>
        public long UsedStoreSpace { set; private get; }

        /// <summary>
        /// 实例化一个本地附件存储
        /// </summary>
        /// <param name="storeProvider"></param>
        /// <param name="logger"></param>
        /// <param name="options"></param>
        public NetStoreHost(IStoreProvider storeProvider, ILogger<NetStoreHost> logger, NetStoreHostOptions options)
        {
            this._options = options;
            this._storeProvider = storeProvider;
            this._logger = logger;
        }

        /// <summary>
        /// 获取存储空间
        /// </summary>
        /// <returns></returns>
        public Task<long> GetStoreSpaceSize()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task<string?> UploadAsync(string fileName, Stream stream)
        {
            var client = this._storeProvider.GetHttpClient();

            var request = new HttpRequestMessage(HttpMethod.Post, new Uri(new Uri(this._options.ServerUrl), this._options.ActionUrl));
            request.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36");
            request.Content = new MultipartFormDataContent
            {
                { new StreamContent(stream), "imagefile", System.IO.Path.GetFileName(fileName) },
            };
            var response = await client.SendAsync(request);
            if(!response.IsSuccessStatusCode)
            {
                this._logger.LogError($"图片上传失败，HTTP STATUS {response.StatusCode} 非预期响应结果。");
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();

            var result = JSON.Parse(json);
            var status = result.GetValue("status").ToObject<string>();
            if(status != "0")
            {
                this._logger.LogError($"图片上传失败，返回结果 {json} 非预期结果。");
                return null;
            }
            return result.GetValue("value").ToObject<string>();
        }
    }
}
