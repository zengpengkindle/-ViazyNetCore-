namespace ViazyNetCore.AttachmentProvider.Hosts
{
    /// <summary>
    /// 表示一个本地附件配置项
    /// </summary>
    public class LocalStoreOptions : IStoreHostOptions
    {
        /// <summary>
        /// 服务器地址
        /// </summary>
        public string? ServerUrl { get; set; }

        /// <summary>
        /// 服务器地址
        /// </summary>
        public string? StoreRootPath { get; set; }

        /// <summary>
        /// 上传保存路径
        /// </summary>
        public string PathFormat { get; set; } = "/{area}/{media}/{yyyy}/{mm}/{dd}";

        /// <summary>
        /// 上传类型限制
        /// </summary>
        public List<MediaType> MediaTypes { get; set; } = new List<MediaType> { MediaType.Image };

        /// <summary>
        /// 上传大小限制，单位B
        /// </summary>
        public long FileMaxSize { get; set; }

        /// <summary>
        /// 附件服务器空间限制，单位MB
        /// </summary>
        public long StroeMaxSize { get; set; }

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
    /// 表示一个本地附件存储
    /// </summary>
    public class LocalStoreHost : ILocalStoreHost
    {
        private readonly LocalStoreProvider _defaultStoreProvider;
        private readonly LocalStoreOptions _options;
        private readonly IPathFormatter _pathFormatter;

        string IStoreHost.Name => "Local";

        IStoreHostOptions IStoreHost.Options => this._options;

        /// <summary>
        /// 设置存储器已使用空间
        /// </summary>
        public long UsedStoreSpace { set; get; }

        /// <summary>
        /// 实例化一个本地附件存储
        /// </summary>
        /// <param name="defaultStoreProvider"></param>
        /// <param name="options"></param>
        /// <param name="pathFormatter"></param>
        public LocalStoreHost(LocalStoreProvider defaultStoreProvider, LocalStoreOptions options, IPathFormatter pathFormatter)
        {
            this._defaultStoreProvider = defaultStoreProvider;
            this._options = options;
            this._pathFormatter = pathFormatter;
        }

        /// <summary>
        /// 上传附件至本地
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task<string?> UploadAsync(string fileName, Stream stream)
        {
            var storeFile = await this._defaultStoreProvider.AddOrUpdateFileAsync(fileName, fileName, stream) as DefaultStoreFile;
            if (storeFile == null)
                throw new NotImplementedException(nameof(storeFile));
            return storeFile.FullLocalPath;
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
        /// 
        /// </summary>
        /// <param name="attachment"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public Task<IStoreFile> LocalUploadAsync(Attachment attachment, Stream stream)
        {
            var relativePath = this._pathFormatter.Format(this._options.PathFormat).Replace("{media}", attachment.GetMediaTypeText());
            return this._defaultStoreProvider.AddOrUpdateFileAsync(relativePath, attachment.FileName, stream);
        }
    }
}
