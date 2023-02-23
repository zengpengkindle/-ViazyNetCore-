using System;
using System.Net.Http;


namespace ViazyNetCore.Http
{
	/// <summary>
	/// A static container for global configuration settings affecting Caesar.Http behavior.
	/// </summary>
	public static class EasyHttp
	{
		private static readonly object _configLock = new();

		private static Lazy<GlobalCaesarHttpSettings> _settings =
			new(() => new GlobalCaesarHttpSettings());

		/// <summary>
		/// Globally configured Caesar.Http settings. Should normally be written to by calling CaesarHttp.Configure once application at startup.
		/// </summary>
		public static GlobalCaesarHttpSettings GlobalSettings => _settings.Value;

		/// <summary>
		/// Provides thread-safe access to Caesar.Http's global configuration settings. Should only be called once at application startup.
		/// </summary>
		/// <param name="configAction">the action to perform against the GlobalSettings.</param>
		public static void Configure(Action<GlobalCaesarHttpSettings> configAction) {
			lock (_configLock) {
				configAction(GlobalSettings);
			}
		}


	}
}