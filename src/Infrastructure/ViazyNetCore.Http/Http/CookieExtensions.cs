

using System.Linq;

namespace ViazyNetCore.Http
{
	/// <summary>
	/// Fluent extension methods for working with HTTP cookies.
	/// </summary>
	public static class CookieExtensions
	{
		/// <summary>
		/// Adds or updates a name-value pair in this request's Cookie header.
		/// To automatically maintain a cookie "session", consider using a CookieJar or CookieSession instead.
		/// </summary>
		/// <param name="request">The ICaesarRequest.</param>
		/// <param name="name">The cookie name.</param>
		/// <param name="value">The cookie value.</param>
		/// <returns>This ICaesarClient instance.</returns>
		public static IEasyRequest WithCookie(this IEasyRequest request, string name, object value) {
			var cookies = new NameValueList<string>(request.Cookies, true); // cookie names are case-sensitive https://stackoverflow.com/a/11312272/62600
			cookies.AddOrReplace(name, value.ToInvariantString());
			return request.WithHeader("Cookie", CookieCutter.ToRequestHeader(cookies));
		}

        public static IEasyRequest RemoveCookie(this IEasyRequest request, string name)
        {
            var cookies = new NameValueList<string>(request.Cookies, true); // cookie names are case-sensitive https://stackoverflow.com/a/11312272/62600
            cookies.Remove(name);
			return request.WithHeader("Cookie", CookieCutter.ToRequestHeader(cookies));
		}

		/// <summary>
		/// Adds or updates name-value pairs in this request's Cookie header, based on property names/values
		/// of the provided object, or keys/values if object is a dictionary.
		/// To automatically maintain a cookie "session", consider using a CookieJar or CookieSession instead.
		/// </summary>
		/// <param name="request">The ICaesarRequest.</param>
		/// <param name="values">Names/values of HTTP cookies to set. Typically an anonymous object or IDictionary.</param>
		/// <returns>This ICaesarClient.</returns>
		public static IEasyRequest WithCookies(this IEasyRequest request, object values) {
			var cookies = new NameValueList<string>(request.Cookies, true); // cookie names are case-sensitive https://stackoverflow.com/a/11312272/62600
			// although rare, we need to accommodate the possibility of multiple cookies with the same name
			foreach (var group in values.ToKeyValuePairs().GroupBy(x => x.Key)) {
				// add or replace the first one (by name)
				cookies.AddOrReplace(group.Key, group.First().Value.ToInvariantString());
				// append the rest
				foreach (var kv in group.Skip(1))
					cookies.Add(kv.Key, kv.Value.ToInvariantString());
			}
			return request.WithHeader("Cookie", CookieCutter.ToRequestHeader(cookies));
		}

		/// <summary>
		/// Sets the CookieJar associated with this request, which will be updated with any Set-Cookie headers present
		/// in the response and is suitable for reuse in subsequent requests.
		/// </summary>
		/// <param name="request">The ICaesarRequest.</param>
		/// <param name="cookieJar">The CookieJar.</param>
		/// <returns>This ICaesarClient instance.</returns>
		public static IEasyRequest WithCookies(this IEasyRequest request, CookieJar cookieJar) {
			request.CookieJar = cookieJar;
			return request;
		}

		/// <summary>
		/// Creates a new CookieJar and associates it with this request, which will be updated with any Set-Cookie
		/// headers present in the response and is suitable for reuse in subsequent requests.
		/// </summary>
		/// <param name="request">The ICaesarRequest.</param>
		/// <param name="cookieJar">The created CookieJar, which can be reused in subsequent requests.</param>
		/// <returns>This ICaesarClient instance.</returns>
		public static IEasyRequest WithCookies(this IEasyRequest request, out CookieJar cookieJar) {
			cookieJar = new CookieJar();
			return request.WithCookies(cookieJar);
		}
	}
}
