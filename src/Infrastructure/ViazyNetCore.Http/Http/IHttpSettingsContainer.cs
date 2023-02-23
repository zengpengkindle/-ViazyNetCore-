


namespace ViazyNetCore.Http
{
	/// <summary>
	/// Defines stateful aspects (headers, cookies, etc) common to both ICaesarClient and ICaesarRequest
	/// </summary>
	public interface IHttpSettingsContainer
	{
	    /// <summary>
	    /// Gets or sets the CaesarHttpSettings object used by this client or request.
	    /// </summary>
	    EasyHttpSettings Settings { get; set; }

		/// <summary>
		/// Collection of headers sent on this request or all requests using this client.
		/// </summary>
		INameValueList<string> Headers { get; }
    }
}
