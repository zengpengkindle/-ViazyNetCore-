using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace VizayNetCore.WebApi.Remote
{
    /// <summary>
    /// 表示一个远端接口请求的异常响应。
    /// </summary>
    public class RemoteApiHttpException : Exception
    {
        /// <summary>
        /// 初始化一个 <see cref="RemoteApiHttpException"/> 类的新实例。
        /// </summary>
        /// <param name="message">错误消息。</param>
        /// <param name="httpResponse">HTTP 响应。</param>
        /// <param name="innerException">内部错误。</param>
        public RemoteApiHttpException(string message, HttpResponseMessage httpResponse, Exception innerException = null)
            : base(message, innerException)
        {
            this.HttpResponse = httpResponse;
        }

        /// <summary>
        /// 获取 HTTP 响应。
        /// </summary>
        public HttpResponseMessage HttpResponse { get; }
    }
}
