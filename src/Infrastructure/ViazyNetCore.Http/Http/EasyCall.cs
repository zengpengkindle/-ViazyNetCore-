﻿using System;
using System.Net.Http;


namespace ViazyNetCore.Http
{
	/// <summary>
	/// Encapsulates request, response, and other details associated with an HTTP call. Useful for diagnostics and available in
	/// global event handlers and CaesarHttpException.Call.
	/// </summary>
	public class EasyCall
	{
		/// <summary>
		/// The ICaesarRequest associated with this call.
		/// </summary>
		public IEasyRequest Request { get; set; }

		/// <summary>
		/// The raw HttpRequestMessage associated with this call.
		/// </summary>
		public HttpRequestMessage HttpRequestMessage { get; set; }

		/// <summary>
		/// Captured request body. Available ONLY if HttpRequestMessage.Content is a Caesar.Http.Content.CapturedStringContent.
		/// </summary>
		public string RequestBody => (HttpRequestMessage.Content as CapturedStringContent)?.Content;

		/// <summary>
		/// The ICaesarResponse associated with this call if the call completed, otherwise null.
		/// </summary>
		public IEasyResponse Response { get; set; }

		/// <summary>
		/// The CaesarCall that received a 3xx response and automatically triggered this call.
		/// </summary>
		public EasyCall RedirectedFrom { get; set; }

		/// <summary>
		/// If this call has a 3xx response and Location header, contains information about how to handle the redirect.
		/// Otherwise null.
		/// </summary>
		public CaesarRedirect Redirect { get; set; }

		/// <summary>
		/// The raw HttpResponseMessage associated with the call if the call completed, otherwise null.
		/// </summary>
		public HttpResponseMessage HttpResponseMessage { get; set; }

		/// <summary>
		/// Exception that occurred while sending the HttpRequestMessage.
		/// </summary>
		public Exception Exception { get; set; }
	
		/// <summary>
		/// User code should set this to true inside global event handlers (OnError, etc) to indicate
		/// that the exception was handled and should not be propagated further.
		/// </summary>
		public bool ExceptionHandled { get; set; }

		/// <summary>
		/// DateTime the moment the request was sent.
		/// </summary>
		public DateTime StartedUtc { get; set; }

		/// <summary>
		/// DateTime the moment a response was received.
		/// </summary>
		public DateTime? EndedUtc { get; set; }

		/// <summary>
		/// Total duration of the call if it completed, otherwise null.
		/// </summary>
		public TimeSpan? Duration => EndedUtc - StartedUtc;

		/// <summary>
		/// True if a response was received, regardless of whether it is an error status.
		/// </summary>
		public bool Completed => HttpResponseMessage != null;

		/// <summary>
		/// True if response was received with any success status or a match with AllowedHttpStatusRange setting.
		/// </summary>
		public bool Succeeded =>
			HttpResponseMessage == null ? false :
			(int)HttpResponseMessage.StatusCode < 400 ? true :
			string.IsNullOrEmpty(Request?.Settings?.AllowedHttpStatusRange) ? false :
			HttpStatusRangeParser.IsMatch(Request.Settings.AllowedHttpStatusRange, HttpResponseMessage.StatusCode);

		/// <summary>
		/// Returns the verb and absolute URI associated with this call.
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return $"{HttpRequestMessage.Method:U} {Request.Url}";
		}
	}

	/// <summary>
	/// An object containing information about if/how an automatic redirect request will be created and sent.
	/// </summary>
	public class CaesarRedirect
	{
		/// <summary>
		/// The URL to redirect to, from the response's Location header.
		/// </summary>
		public Url Url { get; set; }

		/// <summary>
		/// The number of auto-redirects that have already occurred since the original call, plus 1 for this one.
		/// </summary>
		public int Count { get; set; }

		/// <summary>
		/// If true, Caesar will automatically send a redirect request. Set to false to prevent auto-redirect.
		/// </summary>
		public bool Follow { get; set; }

		/// <summary>
		/// If true, the redirect request will use the GET verb and will not forward the original request body.
		/// Otherwise, the original verb and body will be preserved in the redirect.
		/// </summary>
		public bool ChangeVerbToGet { get; set; }
	}
}
