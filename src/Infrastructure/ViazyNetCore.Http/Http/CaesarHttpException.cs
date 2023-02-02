using System;
using System.Threading.Tasks;

namespace ViazyNetCore.Http
{
	/// <summary>
	/// An exception that is thrown when an HTTP call made by Caesar.Http fails, including when the response
	/// indicates an unsuccessful HTTP status code.
	/// </summary>
	public class CaesarHttpException : Exception
	{
		/// <summary>
		/// An object containing details about the failed HTTP call
		/// </summary>
		public CaesarCall Call { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="CaesarHttpException"/> class.
		/// </summary>
		/// <param name="call">The call.</param>
		/// <param name="message">The message.</param>
		/// <param name="inner">The inner.</param>
		public CaesarHttpException(CaesarCall call, string message, Exception inner) : base(message, inner) {
			Call = call;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CaesarHttpException"/> class.
		/// </summary>
		/// <param name="call">The call.</param>
		/// <param name="inner">The inner.</param>
		public CaesarHttpException(CaesarCall call, Exception inner) : this(call, BuildMessage(call, inner), inner) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="CaesarHttpException"/> class.
		/// </summary>
		/// <param name="call">The call.</param>
		public CaesarHttpException(CaesarCall call) : this(call, BuildMessage(call, null), null) { }

		private static string BuildMessage(CaesarCall call, Exception inner) {
			if (call?.Response != null && !call.Succeeded)
				return $"Call failed with status code {call.Response.StatusCode} ({call.HttpResponseMessage.ReasonPhrase}): {call}";

			var msg = "Call failed";
			if (inner != null) msg += ". " + inner.Message.TrimEnd('.');
			return msg + ((call == null) ? "." : $": {call}");
		}

		/// <summary>
		/// Gets the HTTP status code of the response if a response was received, otherwise null.
		/// </summary>
		public int? StatusCode => Call?.Response?.StatusCode;

		/// <summary>
		/// Gets the response body of the failed call.
		/// </summary>
		/// <returns>A task whose result is the string contents of the response body.</returns>
		public Task<string> GetResponseStringAsync() => Call?.Response?.GetStringAsync() ?? Task.FromResult((string)null);

		/// <summary>
		/// Deserializes the JSON response body to an object of the given type.
		/// </summary>
		/// <typeparam name="T">A type whose structure matches the expected JSON response.</typeparam>
		/// <returns>A task whose result is an object containing data in the response body.</returns>
		public Task<T> GetResponseJsonAsync<T>() => Call?.Response?.GetJsonAsync<T>() ?? Task.FromResult(default(T));

		/// <summary>
		/// Deserializes the JSON response body to a dynamic object.
		/// </summary>
		/// <returns>A task whose result is an object containing data in the response body.</returns>
		public async Task<dynamic> GetResponseJsonAsync() => (Call?.Response == null) ? null :
			await Call.Response.GetJsonAsync().ConfigureAwait(false);
	}

	/// <summary>
	/// An exception that is thrown when an HTTP call made by Caesar.Http times out.
	/// </summary>
	public class CaesarHttpTimeoutException : CaesarHttpException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CaesarHttpTimeoutException"/> class.
		/// </summary>
		/// <param name="call">Details of the HTTP call that caused the exception.</param>
		/// <param name="inner">The inner exception.</param>
		public CaesarHttpTimeoutException(CaesarCall call, Exception inner) : base(call, BuildMessage(call), inner) { }

		private static string BuildMessage(CaesarCall call) =>
			(call == null) ? "Call timed out." :  $"Call timed out: {call.Request?.Proxy}";
	}

	/// <summary>
	/// An exception that is thrown when an HTTP response could not be parsed to a particular format.
	/// </summary>
	public class CaesarParsingException : CaesarHttpException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CaesarParsingException"/> class.
		/// </summary>
		/// <param name="call">Details of the HTTP call that caused the exception.</param>
		/// <param name="expectedFormat">The format that could not be parsed to, i.e. JSON.</param>
		/// <param name="inner">The inner exception.</param>
		public CaesarParsingException(CaesarCall call, string expectedFormat, Exception inner) : base(call, BuildMessage(call, expectedFormat), inner) {
			ExpectedFormat = expectedFormat;
		}

		/// <summary>
		/// The format that could not be parsed to, i.e. JSON.
		/// </summary>
		public string ExpectedFormat { get; }

		private static string BuildMessage(CaesarCall call, string expectedFormat) {
			var msg = $"Response could not be deserialized to {expectedFormat}";
			return msg + ((call == null) ? "." : $": {call}");
		}
	}
}