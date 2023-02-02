//using System;

//namespace Caesar.Http
//{
//	/// <summary>
//	/// A context where multiple requests use a common CookieJar.
//	/// </summary>
//	public class CookieSession : IDisposable
//	{
//		private readonly string _baseUrl;
//		private readonly ICaesarClient _client;

//		/// <summary>
//		/// Creates a new CookieSession where all requests are made off the same base URL.
//		/// </summary>
//		public CookieSession(string baseUrl = null) {
//			_baseUrl = baseUrl;
//		}

//		/// <summary>
//		/// Creates a new CookieSession where all requests are made using the provided ICaesarClient
//		/// </summary>
//		public CookieSession(ICaesarClient client) {
//			_client = client;
//		}

//		/// <summary>
//		/// The CookieJar used by all requests sent with this CookieSession.
//		/// </summary>
//		public CookieJar Cookies { get; } = new CookieJar();

//		/// <summary>
//		/// Creates a new ICaesarRequest with this session's CookieJar that can be further built and sent fluently.
//		/// </summary>
//		/// <param name="urlSegments">The URL or URL segments for the request.</param>
//		public ICaesarRequest Request(params object[] urlSegments) =>
//			_client?.Request(urlSegments).WithCookies(Cookies) ??
//			new CaesarRequest(_baseUrl, urlSegments).WithCookies(Cookies);

//		/// <summary>
//		/// Not necessary to call. IDisposable is implemented mainly for the syntactic sugar of using statements.
//		/// </summary>
//		public void Dispose() { }
//	}
//}
