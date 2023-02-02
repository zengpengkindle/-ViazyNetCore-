using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;


namespace ViazyNetCore.Http
{
	/// <summary>
	/// Fluent extension methods for sending multipart/form-data requests.
	/// </summary>
	public static class MultipartExtensions
	{
        /// <summary>
		/// Sends an asynchronous multipart/form-data POST request.
		/// </summary>
		/// <param name="buildContent">A delegate for building the content parts.</param>
		/// <param name="request">The ICaesarRequest.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
		/// <returns>A Task whose result is the received ICaesarResponse.</returns>
		public static Task<ICaesarResponse> PostMultipartAsync(this ICaesarRequest request, Action<CapturedMultipartContent> buildContent, CancellationToken cancellationToken = default(CancellationToken)) {
			var cmc = new CapturedMultipartContent(request.Settings);
			buildContent(cmc);
            request.Content = cmc;
			return request.SendAsync(HttpMethod.Post, cancellationToken);
		}
	}
}
