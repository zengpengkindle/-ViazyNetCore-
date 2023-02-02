using System.Net;
using System.Net.Http;

namespace ViazyNetCore.Http
{
    /// <summary>
    /// Interface defining creation of HttpClient and HttpMessageHandler used in all Caesar HTTP calls.
    /// Implementation can be added via CaesarHttp.Configure. However, in order not to lose much of
    /// Caesar.Http's functionality, it's almost always best to inherit DefaultHttpClientFactory and
    /// extend the base implementations, rather than implementing this interface directly.
    /// </summary>
    public interface IHttpClientFactory
    {
        /// <summary>
        /// Defines how HttpClient should be instantiated and configured by default. Do NOT attempt
        /// to cache/reuse HttpClient instances here - that should be done at the CaesarClient level
        /// via a custom CaesarClientFactory that gets registered globally.
        /// </summary>
        /// <param name="proxy"></param>
        /// <returns></returns>
        HttpClient CreateClient(CaesarProxy proxy);

        /// <summary>
        /// Defines how the 
        /// </summary>
        /// <returns></returns>
        HttpMessageHandler CreateHandler(CaesarProxy proxy);
    }
}