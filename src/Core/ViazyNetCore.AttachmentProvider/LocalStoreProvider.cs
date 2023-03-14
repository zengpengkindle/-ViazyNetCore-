using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ViazyNetCore.AttachmentProvider
{
    /// <summary>
    /// 本地文件存储提供者
    /// </summary>
    public class LocalStoreProvider
    {
        private readonly string _storeRootPath;

        private readonly Regex ValidPathPattern;
        private readonly Regex ValidFileNamePattern;


        /// <summary>
        /// 构造函数
        /// </summary>
        public LocalStoreProvider(string storeRootPath)
        {
            this._storeRootPath = storeRootPath;

            var stringBuilder = new StringBuilder();
            stringBuilder.Append("^[^");
            char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
            foreach(char c in invalidFileNameChars)
            {
                stringBuilder.Append(Regex.Escape(new string(c, 1)));
            }
            stringBuilder.Append("]{1,255}$");
            this.ValidFileNamePattern = new Regex(stringBuilder.ToString(), RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
            stringBuilder = new StringBuilder();
            stringBuilder.Append("^[^");
            char[] invalidPathChars = Path.GetInvalidPathChars();
            foreach(char c2 in invalidPathChars)
            {
                stringBuilder.Append(Regex.Escape(new string(c2, 1)));
            }
            this.ValidPathPattern = new Regex(stringBuilder.ToString() + "]{0,769}$", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
        }


        /// <summary>
        /// 创建或更新一个文件
        /// </summary>
        /// <param name="relativePath">相对文件路径</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="contentStream">The stream containing the content of the file.</param>
        public async Task<IStoreFile> AddOrUpdateFileAsync(string relativePath, string fileName, Stream contentStream)
        {
            if(contentStream is null || !contentStream.CanRead)
            {
                return null;
            }
            if(!this.IsValidPathAndFileName(relativePath, fileName))
            {
                throw new InvalidOperationException("The provided path and/or file name is invalid.");
            }
            string fullLocalPath = this.GetFullLocalPath(relativePath, fileName);
            this.EnsurePathExists(fullLocalPath, true);
            contentStream.Position = 0L;
            using(var fileStream = File.OpenWrite(fullLocalPath))
            {
                byte[] array = new byte[(contentStream.Length > 65536) ? 65536 : contentStream.Length];
                int count;
                while((count = await contentStream.ReadAsync(array, 0, array.Length)) > 0)
                {
                    await fileStream.WriteAsync(array, 0, count);
                }
                fileStream.Flush();
                fileStream.Close();
            }
            return new DefaultStoreFile(relativePath, new FileInfo(fullLocalPath));
        }
        
        /// <summary>
        /// 获取各种尺寸图片的名称
        /// </summary>
        /// <param name="filename">文件名称</param>
        /// <param name="size">图片尺寸</param>
        /// <param name="resizeMethod">图片缩放方式</param>
        public string GetSizeImageName(string filename, Size size, ResizeMethod resizeMethod)
        {
            return $"{filename}-{((resizeMethod != ResizeMethod.KeepAspectRatio) ? resizeMethod.ToString() : string.Empty)}-{size.Width}x{size.Height}{Path.GetExtension(filename)}";
        }

        /// <summary>
        /// 验证文件路径是否合法
        /// </summary>
        private bool IsValidPath(string path)
        {
            return this.ValidPathPattern.IsMatch(path);
        }

        /// <summary>
        /// 验证文件路径以及文件名称是否合法
        /// </summary>
        private bool IsValidPathAndFileName(string path, string fileName)
        {
            if(this.IsValidPath(path) && this.IsValidFileName(fileName))
            {
                return Encoding.UTF8.GetBytes(path + "." + fileName).Length <= 1024;
            }
            return false;
        }

        /// <summary>
        /// 获取完整的本地物理路径
        /// </summary>
        /// <param name="relativePath">相对文件路径</param>
        /// <param name="fileName">文件名称</param>
        public string GetFullLocalPath(string relativePath, string fileName)
        {
            string text = this._storeRootPath;
            if(text.EndsWith(":"))
            {
                text += "\\";
            }
            if(!string.IsNullOrEmpty(relativePath))
            {
                relativePath = relativePath.TrimStart(new char[] {  Path.DirectorySeparatorChar,Path.AltDirectorySeparatorChar });
                text = Path.Combine(text, relativePath);
            }
            if(!string.IsNullOrEmpty(fileName))
            {
                text = Path.Combine(text, fileName);
            }
            return text;
        }


        /// <summary>
        /// 验证文件名称路径是否合法
        /// </summary>
        private bool IsValidFileName(string fileName)
        {
            if(this.ValidFileNamePattern != null)
            {
                return this.ValidFileNamePattern.IsMatch(fileName);
            }
            return true;
        }

        /// <summary>
        /// 确保建立文件目录
        /// </summary>
        /// <param name="fullLocalPath">文件完整路径</param>
        /// <param name="pathIncludesFilename">文件路径是否包含文件名称</param>
        private void EnsurePathExists(string fullLocalPath, bool pathIncludesFilename)
        {
            string path = pathIncludesFilename ? fullLocalPath.Substring(0, fullLocalPath.LastIndexOf(Path.DirectorySeparatorChar)) : fullLocalPath;
            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
    /// <summary>
    /// 图像缩放方式
    /// </summary>
    /// <remarks>
    /// 不执行放大操作
    /// </remarks>
    public enum ResizeMethod
    {
        /// <summary>
        /// 按绝对尺寸缩放
        /// </summary>
        /// <remarks>
        /// 按指定的尺寸进行缩放,不保证宽高比率，可能导致图像失真
        /// </remarks>
        Absolute = 0,
        /// <summary>
        /// 保持原图像宽高比缩放
        /// </summary>
        /// <remarks>
        /// 保持原图像宽高比进行缩放，不超出指定宽高构成的矩形范围
        /// </remarks>
        KeepAspectRatio = 1,
        /// <summary>
        /// 裁剪图像
        /// </summary>
        /// <remarks>
        /// 保持原图像宽高比
        /// </remarks>        
        Crop = 3
    }
}
