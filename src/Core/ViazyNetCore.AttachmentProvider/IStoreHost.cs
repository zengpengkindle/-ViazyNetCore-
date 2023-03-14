using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.AttachmentProvider
{
    /// <summary>
    /// 定义服务服务器
    /// </summary>
    public interface IStoreHost
    {
        /// <summary>
        /// 
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 
        /// </summary>
        IStoreHostOptions Options { get; }

        /// <summary>
        /// 设置存储器已使用空间
        /// </summary>
        long UsedStoreSpace { set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        Task<string?> UploadAsync(string fileName, Stream stream);

        /// <summary>
        /// 获取存储剩余空间
        /// </summary>
        /// <returns></returns>
        Task<long> GetStoreSpaceSize();
    }

    /// <summary>
    /// 
    /// </summary>
    public interface ILocalStoreHost : IStoreHost
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attachment"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        Task<IStoreFile> LocalUploadAsync(Attachment attachment, Stream stream);
    }
}
