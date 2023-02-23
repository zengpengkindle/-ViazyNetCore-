using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ViazyNetCore.Http
{
	/// <summary>
	/// A set of properties that affect Caesar.Http behavior
	/// </summary>
	public class EasyHttpSettings
	{
		// Values are dictionary-backed so we can check for key existence. Can't do null-coalescing
		// because if a setting is set to null at the request level, that should stick.
		private readonly IDictionary<string, object> _vals = new Dictionary<string, object>();

		private EasyHttpSettings _defaults;

		/// <summary>
		/// Creates a new CaesarHttpSettings object.
		/// </summary>
		public EasyHttpSettings() {
			Redirects = new RedirectSettings(this);
			ResetDefaults();
		}
		/// <summary>
		/// Gets or sets the default values to fall back on when values are not explicitly set on this instance.
		/// </summary>
		public virtual EasyHttpSettings Defaults {
			get => _defaults ?? EasyHttp.GlobalSettings;
			set => _defaults = value;
		}

		/// <summary>
		/// Gets or sets the HTTP request timeout.
		/// </summary>
		public TimeSpan? Timeout {
			get => Get<TimeSpan?>();
			set => Set(value);
		}

        /// <summary>
		/// Gets or sets a pattern representing a range of HTTP status codes which (in addtion to 2xx) will NOT result in Caesar.Http throwing an Exception.
		/// Examples: "3xx", "100,300,600", "100-299,6xx", "*" (allow everything)
		/// 2xx will never throw regardless of this setting.
		/// </summary>
		public string AllowedHttpStatusRange {
			get => Get<string>();
			set => Set(value);
		}

		/// <summary>
		/// Gets or sets object used to serialize and deserialize JSON. Default implementation uses Newtonsoft Json.NET.
		/// </summary>
		public ISerializer JsonSerializer {
			get => Get<ISerializer>();
			set => Set(value);
		}

		/// <summary>
		/// Gets or sets object used to serialize URL-encoded data. (Deserialization not supported in default implementation.)
		/// </summary>
		public ISerializer UrlEncodedSerializer {
			get => Get<ISerializer>();
			set => Set(value);
		}

		/// <summary>
		/// Gets object whose properties describe how Caesar.Http should handle redirect (3xx) responses.
		/// </summary>
		public RedirectSettings Redirects { get; }

		/// <summary>
		/// Gets or sets a callback that is invoked immediately before every HTTP request is sent.
		/// </summary>
		public Action<EasyCall> BeforeCall {
			get => Get<Action<EasyCall>>();
			set => Set(value);
		}

		/// <summary>
		/// Gets or sets a callback that is invoked asynchronously immediately before every HTTP request is sent.
		/// </summary>
		public Func<EasyCall, Task> BeforeCallAsync {
			get => Get<Func<EasyCall, Task>>();
			set => Set(value);
		}

		/// <summary>
		/// Gets or sets a callback that is invoked immediately after every HTTP response is received.
		/// </summary>
		public Action<EasyCall> AfterCall {
			get => Get<Action<EasyCall>>();
			set => Set(value);
		}

		/// <summary>
		/// Gets or sets a callback that is invoked asynchronously immediately after every HTTP response is received.
		/// </summary>
		public Func<EasyCall, Task> AfterCallAsync {
			get => Get<Func<EasyCall, Task>>();
			set => Set(value);
		}

		/// <summary>
		/// Gets or sets a callback that is invoked when an error occurs during any HTTP call, including when any non-success
		/// HTTP status code is returned in the response. Response should be null-checked if used in the event handler.
		/// </summary>
		public Action<EasyCall> OnError {
			get => Get<Action<EasyCall>>();
			set => Set(value);
		}

		/// <summary>
		/// Gets or sets a callback that is invoked asynchronously when an error occurs during any HTTP call, including when any non-success
		/// HTTP status code is returned in the response. Response should be null-checked if used in the event handler.
		/// </summary>
		public Func<EasyCall, Task> OnErrorAsync {
			get => Get<Func<EasyCall, Task>>();
			set => Set(value);
		}

		/// <summary>
		/// Gets or sets a callback that is invoked when any 3xx response with a Location header is received.
		/// You can inspect/manipulate the call.Redirect object to determine what will happen next.
		/// An auto-redirect will only happen if call.Redirect.Follow is true upon exiting the callback.
		/// </summary>
		public Action<EasyCall> OnRedirect {
			get => Get<Action<EasyCall>>();
			set => Set(value);
		}

		/// <summary>
		/// Gets or sets a callback that is invoked asynchronously when any 3xx response with a Location header is received.
		/// You can inspect/manipulate the call.Redirect object to determine what will happen next.
		/// An auto-redirect will only happen if call.Redirect.Follow is true upon exiting the callback.
		/// </summary>
		public Func<EasyCall, Task> OnRedirectAsync {
			get => Get<Func<EasyCall, Task>>();
			set => Set(value);
		}

		/// <summary>
		/// Resets all overridden settings to their default values. For example, on a CaesarRequest,
		/// all settings are reset to CaesarClient-level settings.
		/// </summary>
		public virtual void ResetDefaults() {
			_vals.Clear();
		}

		/// <summary>
		/// Gets a settings value from this instance if explicitly set, otherwise from the default settings that back this instance.
		/// </summary>
		internal T Get<T>([CallerMemberName]string propName = null) {
			//var testVals = HttpTest.Current?.Settings._vals;
			return
				//testVals?.ContainsKey(propName) == true ? (T)testVals[propName] :
				_vals.ContainsKey(propName) ? (T)_vals[propName] :
				Defaults != null ? (T)Defaults.Get<T>(propName) :
				default(T);
		}

		/// <summary>
		/// Sets a settings value for this instance.
		/// </summary>
		internal void Set<T>(T value, [CallerMemberName]string propName = null) {
			_vals[propName] = value;
		}
	}

	/// <summary>
	/// Client-level settings for Caesar.Http
	/// </summary>
	public class EasyClientHttpSettings : EasyHttpSettings
	{
		/// <summary>
		/// Specifies the time to keep the underlying HTTP/TCP connection open. When expired, a Connection: close header
		/// is sent with the next request, which should force a new connection and DSN lookup to occur on the next call.
		/// Default is null, effectively disabling the behavior.
		/// </summary>
		public TimeSpan? ConnectionLeaseTimeout {
			get => Get<TimeSpan?>();
			set => Set(value);
		}

		/// <summary>
		/// Gets or sets a factory used to create the HttpClient and HttpMessageHandler used for HTTP calls.
		/// Whenever possible, custom factory implementations should inherit from DefaultHttpClientFactory,
		/// only override the method(s) needed, call the base method, and modify the result.
		/// </summary>
		public IHttpClientFactory HttpClientFactory {
			get => Get<IHttpClientFactory>();
			set => Set(value);
		}
	}

	/// <summary>
	/// Global default settings for Caesar.Http
	/// </summary>
	public class GlobalCaesarHttpSettings : EasyClientHttpSettings
	{
		internal GlobalCaesarHttpSettings() {
			ResetDefaults();
		}

		/// <summary>
		/// Defaults at the global level do not make sense and will always be null.
		/// </summary>
		public override EasyHttpSettings Defaults {
			get => null;
			set => throw new Exception("Global settings cannot be backed by any higher-level defauts.");
		}



		/// <summary>
		/// Resets all global settings to their default values.
		/// </summary>
		public override void ResetDefaults() {
			base.ResetDefaults();
			Timeout = TimeSpan.FromSeconds(5); // same as HttpClient
			JsonSerializer = new NewtonsoftJsonSerializer(null);
			UrlEncodedSerializer = new DefaultUrlEncodedSerializer();
			HttpClientFactory = new DefaultHttpClientFactory();
			Redirects.Enabled = true;
			Redirects.AllowSecureToInsecure = false;
			Redirects.ForwardAuthorizationHeader = false;
			Redirects.MaxAutoRedirects = 10;
		}
	}

	///// <summary>
	///// Settings overrides within the context of an HttpTest
	///// </summary>
	//public class TestCaesarHttpSettings : ClientCaesarHttpSettings
	//{
	//	/// <summary>
	//	/// Resets all test settings to their Caesar.Http-defined default values.
	//	/// </summary>
	//	public override void ResetDefaults() {
	//		base.ResetDefaults();
	//		HttpClientFactory = new TestHttpClientFactory();
	//	}
	//}
}
