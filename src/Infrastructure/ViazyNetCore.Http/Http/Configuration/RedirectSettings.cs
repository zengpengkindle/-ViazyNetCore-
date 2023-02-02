namespace ViazyNetCore.Http
{
	/// <summary>
	/// A set of properties that affect Caesar.Http behavior specific to auto-redirecting.
	/// </summary>
	public class RedirectSettings
	{
		private readonly CaesarHttpSettings _settings;

		/// <summary>
		/// Creates a new instance of RedirectSettings.
		/// </summary>
		public RedirectSettings(CaesarHttpSettings settings) {
			_settings = settings;
		}

		/// <summary>
		/// If false, all of Caesar's mechanisms for handling redirects, including raising the OnRedirect event,
		/// are disabled entirely. This could also impact cookie functionality. Default is true. If you don't
		/// need Caesar's redirect or cookie functionality, or you are providing an HttpClient whose HttpClientHandler
		/// is providing these services, then it is safe to set this to false.
		/// </summary>
		public bool Enabled {
			get => _settings.Get<bool>("Redirects_Enabled");
			set => _settings.Set(value, "Redirects_Enabled");
		}

		/// <summary>
		/// If true, redirecting from HTTPS to HTTP is allowed. Default is false, as this behavior is considered
		/// insecure.
		/// </summary>
		public bool AllowSecureToInsecure {
			get => _settings.Get<bool>("Redirects_AllowSecureToInsecure");
			set => _settings.Set(value, "Redirects_AllowSecureToInsecure");
		}

		/// <summary>
		/// If true, any Authorization header sent in the original request is forwarded in the redirect.
		/// Default is false, as this behavior is considered insecure.
		/// </summary>
		public bool ForwardAuthorizationHeader {
			get => _settings.Get<bool>("Redirects_ForwardAuthorizationHeader");
			set => _settings.Set(value, "Redirects_ForwardAuthorizationHeader");
		}

		/// <summary>
		/// Maximum number of redirects that Caesar will automatically follow in a single request. Default is 10.
		/// </summary>
		public int MaxAutoRedirects {
			get => _settings.Get<int>("Redirects_MaxRedirects");
			set => _settings.Set(value, "Redirects_MaxRedirects");
		}
	}
}
