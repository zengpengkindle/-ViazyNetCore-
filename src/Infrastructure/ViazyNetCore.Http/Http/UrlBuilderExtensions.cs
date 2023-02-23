using System;
using System.Collections.Generic;

namespace ViazyNetCore.Http
{
	/// <summary>
	/// URL builder extension methods on CaesarRequest
	/// </summary>
	public static class UrlBuilderExtensions
	{
		/// <summary>
		/// Appends a segment to the URL path, ensuring there is one and only one '/' character as a seperator.
		/// </summary>
		/// <param name="request">The ICaesarRequest associated with the URL</param>
		/// <param name="segment">The segment to append</param>
		/// <param name="fullyEncode">If true, URL-encodes reserved characters such as '/', '+', and '%'. Otherwise, only encodes strictly illegal characters (including '%' but only when not followed by 2 hex characters).</param>
		/// <returns>This ICaesarRequest</returns>
		/// <exception cref="ArgumentNullException"><paramref name="segment"/> is <see langword="null" />.</exception>
		public static IEasyRequest AppendPathSegment(this IEasyRequest request, object segment, bool fullyEncode = false) {
			request.Url.AppendPathSegment(segment, fullyEncode);
			return request;
		}

		/// <summary>
		/// Appends multiple segments to the URL path, ensuring there is one and only one '/' character as a seperator.
		/// </summary>
		/// <param name="request">The ICaesarRequest associated with the URL</param>
		/// <param name="segments">The segments to append</param>
		/// <returns>This ICaesarRequest</returns>
		public static IEasyRequest AppendPathSegments(this IEasyRequest request, params object[] segments) {
			request.Url.AppendPathSegments(segments);
			return request;
		}

		/// <summary>
		/// Appends multiple segments to the URL path, ensuring there is one and only one '/' character as a seperator.
		/// </summary>
		/// <param name="request">The ICaesarRequest associated with the URL</param>
		/// <param name="segments">The segments to append</param>
		/// <returns>This ICaesarRequest</returns>
		public static IEasyRequest AppendPathSegments(this IEasyRequest request, IEnumerable<object> segments) {
			request.Url.AppendPathSegments(segments);
			return request;
		}

		/// <summary>
		/// Adds a parameter to the URL query, overwriting the value if name exists.
		/// </summary>
		/// <param name="request">The ICaesarRequest associated with the URL</param>
		/// <param name="name">Name of query parameter</param>
		/// <param name="value">Value of query parameter</param>
		/// <param name="nullValueHandling">Indicates how to handle null values. Defaults to Remove (any existing)</param>
		/// <returns>This ICaesarRequest</returns>
		public static IEasyRequest SetQueryParam(this IEasyRequest request, string name, object value, NullValueHandling nullValueHandling = NullValueHandling.Remove) {
			request.Url.SetQueryParam(name, value, nullValueHandling);
			return request;
		}

		/// <summary>
		/// Adds a parameter to the URL query, overwriting the value if name exists.
		/// </summary>
		/// <param name="request">The ICaesarRequest associated with the URL</param>
		/// <param name="name">Name of query parameter</param>
		/// <param name="value">Value of query parameter</param>
		/// <param name="isEncoded">Set to true to indicate the value is already URL-encoded</param>
		/// <param name="nullValueHandling">Indicates how to handle null values. Defaults to Remove (any existing)</param>
		/// <returns>This ICaesarRequest</returns>
		/// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null" />.</exception>
		public static IEasyRequest SetQueryParam(this IEasyRequest request, string name, string value, bool isEncoded = false, NullValueHandling nullValueHandling = NullValueHandling.Remove) {
			request.Url.SetQueryParam(name, value, isEncoded, nullValueHandling);
			return request;
		}

		/// <summary>
		/// Adds a parameter without a value to the URL query, removing any existing value.
		/// </summary>
		/// <param name="request">The ICaesarRequest associated with the URL</param>
		/// <param name="name">Name of query parameter</param>
		/// <returns>This ICaesarRequest</returns>
		public static IEasyRequest SetQueryParam(this IEasyRequest request, string name) {
			request.Url.SetQueryParam(name);
			return request;
		}

		/// <summary>
		/// Parses values (usually an anonymous object or dictionary) into name/value pairs and adds them to the URL query, overwriting any that already exist.
		/// </summary>
		/// <param name="request">The ICaesarRequest associated with the URL</param>
		/// <param name="values">Typically an anonymous object, ie: new { x = 1, y = 2 }</param>
		/// <param name="nullValueHandling">Indicates how to handle null values. Defaults to Remove (any existing)</param>
		/// <returns>This ICaesarRequest</returns>
		public static IEasyRequest SetQueryParams(this IEasyRequest request, object values, NullValueHandling nullValueHandling = NullValueHandling.Remove) {
			request.Url.SetQueryParams(values, nullValueHandling);
			return request;
		}

		/// <summary>
		/// Adds multiple parameters without values to the URL query.
		/// </summary>
		/// <param name="request">The ICaesarRequest associated with the URL</param>
		/// <param name="names">Names of query parameters.</param>
		/// <returns>This ICaesarRequest</returns>
		public static IEasyRequest SetQueryParams(this IEasyRequest request, IEnumerable<string> names) {
			request.Url.SetQueryParams(names);
			return request;
		}

		/// <summary>
		/// Adds multiple parameters without values to the URL query.
		/// </summary>
		/// <param name="request">The ICaesarRequest associated with the URL</param>
		/// <param name="names">Names of query parameters</param>
		/// <returns>This ICaesarRequest</returns>
		public static IEasyRequest SetQueryParams(this IEasyRequest request, params string[] names) {
			request.Url.SetQueryParams(names as IEnumerable<string>);
			return request;
		}

		/// <summary>
		/// Removes a name/value pair from the URL query by name.
		/// </summary>
		/// <param name="request">The ICaesarRequest associated with the URL</param>
		/// <param name="name">Query string parameter name to remove</param>
		/// <returns>This ICaesarRequest</returns>
		public static IEasyRequest RemoveQueryParam(this IEasyRequest request, string name) {
			request.Url.RemoveQueryParam(name);
			return request;
		}

		/// <summary>
		/// Removes multiple name/value pairs from the URL query by name.
		/// </summary>
		/// <param name="request">The ICaesarRequest associated with the URL</param>
		/// <param name="names">Query string parameter names to remove</param>
		/// <returns>This ICaesarRequest</returns>
		public static IEasyRequest RemoveQueryParams(this IEasyRequest request, params string[] names) {
			request.Url.RemoveQueryParams(names);
			return request;
		}

		/// <summary>
		/// Removes multiple name/value pairs from the URL query by name.
		/// </summary>
		/// <param name="request">The ICaesarRequest associated with the URL</param>
		/// <param name="names">Query string parameter names to remove</param>
		/// <returns>This ICaesarRequest</returns>
		public static IEasyRequest RemoveQueryParams(this IEasyRequest request, IEnumerable<string> names) {
			request.Url.RemoveQueryParams(names);
			return request;
		}

		/// <summary>
		/// Set the URL fragment fluently.
		/// </summary>
		/// <param name="request">The ICaesarRequest associated with the URL</param>
		/// <param name="fragment">The part of the URL afer #</param>
		/// <returns>This ICaesarRequest</returns>
		public static IEasyRequest SetFragment(this IEasyRequest request, string fragment) {
			request.Url.SetFragment(fragment);
			return request;
		}

		/// <summary>
		/// Removes the URL fragment including the #.
		/// </summary>
		/// <param name="request">The ICaesarRequest associated with the URL</param>
		/// <returns>This ICaesarRequest</returns>
		public static IEasyRequest RemoveFragment(this IEasyRequest request) {
			request.Url.RemoveFragment();
			return request;
		}

        public static IEasyRequest SetHost(this IEasyRequest request, string host)
        {
            request.Url.SetHost(host);
            return request;
        }
        public static IEasyRequest SetPort(this IEasyRequest request, int? port)
        {
            request.Url.SetPort(port);
            return request;
        }
        public static IEasyRequest SetScheme(this IEasyRequest request, string scheme)
        {
            request.Url.SetScheme(scheme);
            return request;
        }
        public static IEasyRequest SetDomain(this IEasyRequest request, string domain)
        {
            request.Url.SetDomain(domain);
            return request;
        }
	}
}
