using System.Collections.Generic;

namespace Microsoft.Extensions.FileProviders
{
    /// <summary>
    /// 定义一个目录提供程序。
    /// </summary>
    public interface IDirectoryProvider: IDirectoryInfo
    {
        /// <summary>
        /// 获取子目录。
        /// </summary>
        /// <param name="subpath">子路径。</param>
        /// <param name="createWithNotExists">当文件夹不存在时是否自动创建。</param>
        /// <returns>目录提供程序。</returns>
        IDirectoryInfo GetDirectoryInfo(string subpath, bool createWithNotExists = false);
        /// <summary>
        /// 遍历子目录。
        /// </summary>
        /// <returns>子目录。</returns>
        IEnumerable<IDirectoryInfo> EnumerateDirectories();
        /// <summary>
        /// 遍历子文件。
        /// </summary>
        /// <returns>子文件。</returns>
        IEnumerable<IFileInfo> EnumerateFiles();
    }
}
