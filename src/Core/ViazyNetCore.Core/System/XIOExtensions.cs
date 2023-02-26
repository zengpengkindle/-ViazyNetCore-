using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// 表示一个 IO 的扩展方法。
    /// </summary>
    public static class XIOExtensions
    {
        /// <summary>
        /// 读取所有流的所有字节。
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="encoding">编号。</param>
        /// <returns>所有字节。</returns>
        public static async Task<string> ReadToEndAsync(this IO.Stream stream, Encoding? encoding = null)
        {
            if(stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if(encoding is null) encoding = Encoding.UTF8;
            using var reader = new IO.StreamReader(stream
                 , encoding
                 , detectEncodingFromByteOrderMarks: false
                 , bufferSize: 4096
                 , leaveOpen: true);
            return await reader.ReadToEndAsync();
        }

        /// <summary>
        /// 读取所有流的所有字节。
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="encoding">编号。</param>
        /// <returns>所有字节。</returns>
        public static string ReadToEnd(this IO.Stream stream, Encoding? encoding = null)
        {
            if(stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if(encoding is null) encoding = Encoding.UTF8;
            using var reader = new IO.StreamReader(stream
                 , encoding
                 , detectEncodingFromByteOrderMarks: false
                 , bufferSize: 4096
                 , leaveOpen: true);
            return reader.ReadToEnd();
        }

        /// <summary>
        /// 将所有字节写入流中。
        /// </summary>
        /// <param name="stream">流。</param>
        /// <param name="bytes">字节。</param>
        public static void WriteBytes(this IO.Stream stream, byte[] bytes)
        {
            if(stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if(bytes is null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            stream.Write(bytes, 0, bytes.Length);
        }

    }
}
