using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;



namespace ViazyNetCore.Http
{
	/// <summary>
	/// Represents an HTTP response.
	/// </summary>
	public interface ICaesarResponse : IDisposable
	{
		/// <summary>
		/// Gets the collection of response headers received.
		/// </summary>
		IReadOnlyNameValueList<string> Headers { get; }

		/// <summary>
		/// Gets the collection of HTTP cookies received in this response via Set-Cookie headers.
		/// </summary>
		IReadOnlyList<CaesarCookie> Cookies { get; }

		/// <summary>
		/// Gets the raw HttpResponseMessage that this ICaesarResponse wraps.
		/// </summary>
		HttpResponseMessage ResponseMessage { get; }

        CaesarCall Call { get; set; }

		/// <summary>
		/// Gets the HTTP status code of the response.
		/// </summary>
		int StatusCode { get; }

		/// <summary>
		/// Deserializes JSON-formatted HTTP response body to object of type T.
		/// </summary>
		/// <typeparam name="T">A type whose structure matches the expected JSON response.</typeparam>
		/// <returns>A Task whose result is an object containing data in the response body.</returns>
		/// <example>x = await url.PostAsync(data).GetJson&lt;T&gt;()</example>
		/// <exception cref="CaesarHttpException">Condition.</exception>
		Task<T> GetJsonAsync<T>();

		/// <summary>
		/// Deserializes JSON-formatted HTTP response body to a dynamic object.
		/// </summary>
		/// <returns>A Task whose result is a dynamic object containing data in the response body.</returns>
		/// <example>d = await url.PostAsync(data).GetJson()</example>
		/// <exception cref="CaesarHttpException">Condition.</exception>
		Task<dynamic> GetJsonAsync();

		/// <summary>
		/// Deserializes JSON-formatted HTTP response body to a list of dynamic objects.
		/// </summary>
		/// <returns>A Task whose result is a list of dynamic objects containing data in the response body.</returns>
		/// <example>d = await url.PostAsync(data).GetJsonList()</example>
		/// <exception cref="CaesarHttpException">Condition.</exception>
		Task<IList<dynamic>> GetJsonListAsync();

		/// <summary>
		/// Returns HTTP response body as a string.
		/// </summary>
		/// <returns>A Task whose result is the response body as a string.</returns>
		/// <example>s = await url.PostAsync(data).GetString()</example>
		Task<string> GetStringAsync();

		/// <summary>
		/// Returns HTTP response body as a stream.
		/// </summary>
		/// <returns>A Task whose result is the response body as a stream.</returns>
		/// <example>stream = await url.PostAsync(data).GetStream()</example>
		Task<Stream> GetStreamAsync();

		/// <summary>
		/// Returns HTTP response body as a byte array.
		/// </summary>
		/// <returns>A Task whose result is the response body as a byte array.</returns>
		/// <example>bytes = await url.PostAsync(data).GetBytes()</example>
		Task<byte[]> GetBytesAsync();
	}

	/// <inheritdoc />
	public class CaesarResponse : ICaesarResponse
	{
		private readonly Lazy<IReadOnlyNameValueList<string>> _headers;
		private readonly Lazy<IReadOnlyList<CaesarCookie>> _cookies;
		private object _capturedBody = null;
		private bool _streamRead = false;
		private ISerializer _serializer = null;

		/// <inheritdoc />
		public IReadOnlyNameValueList<string> Headers => _headers.Value;

		/// <inheritdoc />
		public IReadOnlyList<CaesarCookie> Cookies => _cookies.Value;

		/// <inheritdoc />
		public HttpResponseMessage ResponseMessage { get; }

        public CaesarCall Call { get; set; }

        /// <inheritdoc />
		public int StatusCode => (int)ResponseMessage.StatusCode;

		/// <summary>
		/// Creates a new CaesarResponse that wraps the give HttpResponseMessage.
		/// </summary>
		public CaesarResponse(HttpResponseMessage resp,CaesarCall call, CookieJar cookieJar = null) {
			ResponseMessage = resp;
            Call = call;
			_headers = new Lazy<IReadOnlyNameValueList<string>>(LoadHeaders);
			_cookies = new Lazy<IReadOnlyList<CaesarCookie>>(LoadCookies);
			LoadCookieJar(cookieJar);
		}

		private IReadOnlyNameValueList<string> LoadHeaders() {
			var result = new NameValueList<string>(false);

			foreach (var h in ResponseMessage.Headers)
			foreach (var v in h.Value)
				result.Add(h.Key, v);

			if (ResponseMessage.Content?.Headers == null)
				return result;

			foreach (var h in ResponseMessage.Content.Headers)
			foreach (var v in h.Value)
				result.Add(h.Key, v);

			return result;
		}

		private IReadOnlyList<CaesarCookie> LoadCookies() {
			var url = ResponseMessage.RequestMessage.RequestUri.AbsoluteUri;
			return ResponseMessage.Headers.TryGetValues("Set-Cookie", out var headerValues) ?
				headerValues.Select(hv => CookieCutter.ParseResponseHeader(url, hv)).ToList() :
				new List<CaesarCookie>();
		}

		private void LoadCookieJar(CookieJar jar) {
			if (jar == null) return;
			foreach (var cookie in Cookies)
				jar.TryAddOrReplace(cookie, out _); // not added if cookie fails validation
		}

		/// <inheritdoc />
		public async Task<T> GetJsonAsync<T>() {
			if (_streamRead)
				return _capturedBody is T body ? body : default(T);

			var call = ResponseMessage.RequestMessage.GetCaesarCall();
			_serializer = call.Request.Settings.JsonSerializer;
			using (var stream = await ResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false)) {
				try {
					_capturedBody = _serializer.Deserialize<T>(stream);
					_streamRead = true;
					return (T)_capturedBody;
				}
				catch (Exception ex) {
					_serializer = null;
					_capturedBody = await ResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
					_streamRead = true;
					call.Exception = new CaesarParsingException(call, "JSON", ex);
					await CaesarRequest.HandleExceptionAsync(call, call.Exception, CancellationToken.None).ConfigureAwait(false);
					return default(T);
				}
			}
		}

		/// <inheritdoc />
		public async Task<dynamic> GetJsonAsync() {
			dynamic d = await GetJsonAsync<ExpandoObject>().ConfigureAwait(false);
			return d;
		}

		/// <inheritdoc />
		public async Task<IList<dynamic>> GetJsonListAsync() {
			dynamic[] d = await GetJsonAsync<ExpandoObject[]>().ConfigureAwait(false);
			return d;
		}

		/// <inheritdoc />
		public async Task<string> GetStringAsync() {
			if (_streamRead) {
				return
					(_capturedBody == null) ? null :
					// if GetJsonAsync<T> was called, we streamed the response directly to a T (for memory efficiency)
					// without first capturing a string. it's too late to get it, so the best we can do is serialize the T
					(_serializer != null) ? _serializer.Serialize(_capturedBody) :
					_capturedBody?.ToString();
			}

#if NETSTANDARD2_0
			// https://stackoverflow.com/questions/46119872/encoding-issues-with-net-core-2 (#86)
			System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
#endif
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
			// strip quotes from charset so .NET doesn't choke on them
			// https://github.com/dotnet/corefx/issues/5014
			// https://github.com/tmenier/Caesar/pull/76
			var ct = ResponseMessage.Content.Headers?.ContentType;
			if (ct?.CharSet != null)
				ct.CharSet = ct.CharSet.StripQuotes();

			_capturedBody = await ResponseMessage.Content.ReadAsStringAsync();
			_streamRead = true;
			return (string)_capturedBody;
		}

		/// <inheritdoc />
		public Task<Stream> GetStreamAsync() {
			_streamRead = true;
			return ResponseMessage.Content.ReadAsStreamAsync();
		}

		/// <inheritdoc />
		public async Task<byte[]> GetBytesAsync() {
			if (_streamRead)
				return _capturedBody as byte[];

			_capturedBody = await ResponseMessage.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
			_streamRead = true;
			return (byte[])_capturedBody;
		}

		/// <summary>
		/// Disposes the underlying HttpResponseMessage.
		/// </summary>
		public void Dispose() => ResponseMessage.Dispose();
	}
}
