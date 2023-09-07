using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using ViazyNetCore.AttachmentProvider;
using ViazyNetCore.AttachmentProvider.Hosts;

namespace ViazyNetCore.OSS
{
    public class MinioStoreHost : IStoreHost
    {
        private readonly IMinioOSSService _minioOSSService;
        private readonly OSSOptions _options;

        public string Name => "Minio";

        public IStoreHostOptions Options { get; }

        public long UsedStoreSpace { set => throw new NotImplementedException(); }

        public MinioStoreHost(IMinioOSSService minioOSSService, IOptions<OSSOptions> options)
        {
            this._minioOSSService = minioOSSService;
            this._options = options.Value;
            Options = new NetStoreHostOptions
            {
                MediaTypes = this._options.MediaTypes
            };
        }

        public Task<long> GetStoreSpaceSize()
        {
            return Task.FromResult(long.MaxValue);
        }

        public async Task<string?> UploadAsync(string fileName, Stream stream)
        {
            await this._minioOSSService.PutObjectAsync(this._options.DefaultBucketName, fileName, stream);
            return await this._minioOSSService.PresignedGetObjectAsync(this._options.DefaultBucketName, fileName, 24 * 3600);
        }
    }
}
