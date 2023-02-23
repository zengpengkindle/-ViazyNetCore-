using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;



namespace ViazyNetCore.Http
{
    /// <summary>
    /// Represents an HTTP request. Can be created explicitly via new CaesarRequest(), fluently via Url.Request(),
    /// or implicitly when a call is made via methods like Url.GetAsync().
    /// </summary>
    public interface IEasyRequest : IHttpSettingsContainer
    {
        /// <summary>
        /// Gets or sets the ICaesarClient to use when sending the request.
        /// </summary>
        HttpClient Client { get; set; }

        /// <summary>
        /// Gets or sets the HTTP method of the request. Normally you don't need to set this explicitly; it will be set
        /// when you call the sending method, such as GetAsync, PostAsync, etc.
        /// </summary>
        HttpMethod Verb { get; set; }

        /// <summary>
        /// Gets or sets the URL to be called.
        /// </summary>
        Url Url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        EasyHttpProxy Proxy { get; set; }

        /// <summary>
        /// Gets Name/Value pairs parsed from the Cookie request header.
        /// </summary>
        IEnumerable<(string Name, string Value)> Cookies { get; }

        /// <summary>
        /// Gets or sets the collection of HTTP cookies that can be shared between multiple requests. When set, values that
        /// should be sent with this request (based on Domain, Path, and other rules) are immediately copied to the Cookie
        /// request header, and any Set-Cookie headers received in the response will be written to the CookieJar.
        /// </summary>
        CookieJar CookieJar { get; set; }

        HttpContent Content { get; set; }

        byte[] ContentBytes { get; }

        /// <summary>
        /// Asynchronously sends the HTTP request. Mainly used to implement higher-level extension methods (GetJsonAsync, etc).
        /// </summary>
        /// <param name="verb">The HTTP method used to make the request.</param>
        /// <param name="content">Contents of the request body.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <param name="completionOption">The HttpCompletionOption used in the request. Optional.</param>
        /// <returns>A Task whose result is the received ICaesarResponse.</returns>
        Task<IEasyResponse> SendAsync(HttpMethod verb, CancellationToken cancellationToken = default(CancellationToken), HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead);
    }

    /// <inheritdoc />
    public class EasyRequest : IEasyRequest
    {
        private EasyHttpSettings _settings;
        private HttpClient _client;
        private Url _url;
        private EasyCall _redirectedFrom;
        private CookieJar _jar;

        /// <summary>
        /// Initializes a new instance of the <see cref="EasyRequest"/> class.
        /// </summary>
        /// <param name="url">The URL to call with this CaesarRequest instance.</param>
        public EasyRequest(Url url = null)
        {
            _url = url;
        }

        /// <summary>
        /// Used internally by CaesarClient.Request and CookieSession.Request
        /// </summary>
        internal EasyRequest(string baseUrl, object[] urlSegments)
        {
            var parts = new List<string>(urlSegments.Select(s => s.ToInvariantString()));
            if (!Url.IsValid(parts.FirstOrDefault()) && !string.IsNullOrEmpty(baseUrl))
                parts.Insert(0, baseUrl);

            if (!parts.Any())
                throw new ArgumentException("Cannot create a Request. BaseUrl is not defined and no segments were passed.");
            if (!Url.IsValid(parts[0]))
                throw new ArgumentException("Cannot create a Request. Neither BaseUrl nor the first segment passed is a valid URL.");

            _url = Url.Combine(parts.ToArray());
        }

        /// <summary>
        /// Gets or sets the CaesarHttpSettings used by this request.
        /// </summary>
        public EasyHttpSettings Settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = new EasyHttpSettings();
                    //ResetDefaultSettings();
                }
                return _settings;
            }
            set
            {
                _settings = value;
                //ResetDefaultSettings();
            }
        }

        /// <inheritdoc />
        public HttpClient Client
        {
            get =>
                _client ?? ((Url != null) ? EasyHttp.GlobalSettings.HttpClientFactory.CreateClient(Proxy) :
                    null);
            set
            {
                _client = value;
                //ResetDefaultSettings();
            }
        }

        /// <inheritdoc />
        public HttpMethod Verb { get; set; }

        /// <inheritdoc />
        public Url Url
        {
            get => _url;
            set
            {
                _url = value;
                //ResetDefaultSettings();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public EasyHttpProxy Proxy { get; set; } = new EasyHttpProxy();



        /// <inheritdoc />
        public INameValueList<string> Headers { get; } = new NameValueList<string>(false); // header names are case-insensitive https://stackoverflow.com/a/5259004/62600

        /// <inheritdoc />
        public IEnumerable<(string Name, string Value)> Cookies =>
            CookieCutter.ParseRequestHeader(Headers.FirstOrDefault("Cookie"));

        /// <inheritdoc />
        public CookieJar CookieJar
        {
            get => _jar;
            set
            {
                _jar = value;
                this.WithCookies(
                    from c in CookieJar
                    where c.ShouldSendTo(this.Url, out _)
                    // sort by longest path, then earliest creation time, per #2: https://tools.ietf.org/html/rfc6265#section-5.4
                    orderby (c.Path ?? c.OriginUrl.Path).Length descending, c.DateReceived
                    select (c.Name, c.Value));
            }
        }

        private byte[] _contentBytes;
        private bool isParseContent;
        public HttpContent Content { get; set; }
        public byte[] ContentBytes
        {
            get
            {
                if (isParseContent)
                    return _contentBytes;
                isParseContent = true;
                if (Content == null)
                    return null;
                _contentBytes = Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
                return _contentBytes;
            }
        }


        /// <inheritdoc />
		public async Task<IEasyResponse> SendAsync(HttpMethod verb, CancellationToken cancellationToken = default, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead)
        {
            _client = Client; // "freeze" the client at this point to avoid excessive calls to CaesarClientFactory.Get (#374)
            Verb = verb;

            var request = new HttpRequestMessage(verb, Url) { Content = Content };
            SyncHeaders(request);
            var call = new EasyCall
            {
                Request = this,
                RedirectedFrom = _redirectedFrom,
                HttpRequestMessage = request
            };
            request.SetCaesarCall(call);

            await RaiseEventAsync(Settings.BeforeCall, Settings.BeforeCallAsync, call).ConfigureAwait(false);

            // in case URL or headers were modified in the handler above
            request.RequestUri = Url.ToUri();
            SyncHeaders(request);

            call.StartedUtc = DateTime.UtcNow;
            var ct = GetCancellationTokenWithTimeout(cancellationToken, out var cts);

            try
            {

                var response = await Client.SendAsync(request, completionOption, ct).ConfigureAwait(false);
                call.HttpResponseMessage = response;
                call.HttpResponseMessage.RequestMessage = request;
                call.Response = new EasyResponse(call.HttpResponseMessage, call, CookieJar);

                if (call.Succeeded)
                {
                    var redirResponse = await ProcessRedirectAsync(call, cancellationToken, completionOption).ConfigureAwait(false);
                    return redirResponse ?? call.Response;
                }
                else
                    throw new EasyHttpException(call, null);
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(call, ex, cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                request.Dispose();
                cts?.Dispose();
                call.EndedUtc = DateTime.UtcNow;
                await RaiseEventAsync(Settings.AfterCall, Settings.AfterCallAsync, call).ConfigureAwait(false);
            }
        }

        private void SyncHeaders(HttpRequestMessage request)
        {

            // copy headers from CaesarRequest to HttpRequestMessage
            foreach (var header in Headers)
                request.SetHeader(header.Name, header.Value);

            // copy headers from HttpContent to CaesarRequest
            if (request.Content != null)
            {
                foreach (var header in request.Content.Headers)
                    Headers.AddOrReplace(header.Key, string.Join(",", header.Value));
            }
        }

        private CancellationToken GetCancellationTokenWithTimeout(CancellationToken original, out CancellationTokenSource timeoutTokenSource)
        {
            timeoutTokenSource = null;
            if (Settings.Timeout.HasValue)
            {
                timeoutTokenSource = CancellationTokenSource.CreateLinkedTokenSource(original);
                timeoutTokenSource.CancelAfter(Settings.Timeout.Value);
                return timeoutTokenSource.Token;
            }
            else
            {
                return original;
            }
        }

        private async Task<IEasyResponse> ProcessRedirectAsync(EasyCall call, CancellationToken cancellationToken, HttpCompletionOption completionOption)
        {
            if (Settings.Redirects.Enabled)
                call.Redirect = GetRedirect(call);

            if (call.Redirect == null)
                return null;

            await RaiseEventAsync(Settings.OnRedirect, Settings.OnRedirectAsync, call).ConfigureAwait(false);

            if (call.Redirect.Follow != true)
                return null;

            CheckForCircularRedirects(call);

            var redir = new EasyRequest(call.Redirect.Url);
            redir.Client = Client;
            redir._redirectedFrom = call;
            redir.Settings.Defaults = Settings;
            redir.WithHeaders(this.Headers).WithCookies(call.Response.Cookies);

            var changeToGet = call.Redirect.ChangeVerbToGet;

            if (!Settings.Redirects.ForwardAuthorizationHeader)
                redir.Headers.Remove("Authorization");
            if (changeToGet)
                redir.Headers.Remove("Transfer-Encoding");

            var ct = GetCancellationTokenWithTimeout(cancellationToken, out var cts);
            try
            {
                return await redir.SendAsync(
                    changeToGet ? HttpMethod.Get : call.HttpRequestMessage.Method,
                    cancellationToken,
                    completionOption).ConfigureAwait(false);
            }
            finally
            {
                cts?.Dispose();
            }
        }

        // partially lifted from https://github.com/dotnet/runtime/blob/master/src/libraries/System.Net.Http/src/System/Net/Http/SocketsHttpHandler/RedirectHandler.cs
        private CaesarRedirect GetRedirect(EasyCall call)
        {
            if (call.Response.StatusCode < 300 || call.Response.StatusCode > 399)
                return null;

            if (!call.Response.Headers.TryGetFirst("Location", out var location))
                return null;

            var redir = new CaesarRedirect();

            if (Url.IsValid(location))
                redir.Url = new Url(location);
            else if (location.OrdinalStartsWith("/"))
                redir.Url = Url.Combine(this.Url.Root, location);
            else
                redir.Url = Url.Combine(this.Url.Root, this.Url.Path, location);

            // Per https://tools.ietf.org/html/rfc7231#section-7.1.2, a redirect location without a
            // fragment should inherit the fragment from the original URI.
            if (string.IsNullOrEmpty(redir.Url.Fragment))
                redir.Url.Fragment = this.Url.Fragment;

            redir.Count = 1 + (call.RedirectedFrom?.Redirect?.Count ?? 0);

            var isSecureToInsecure = (this.Url.IsSecureScheme && !redir.Url.IsSecureScheme);

            redir.Follow =
                new[] { 301, 302, 303, 307, 308 }.Contains(call.Response.StatusCode) &&
                redir.Count <= Settings.Redirects.MaxAutoRedirects &&
                (Settings.Redirects.AllowSecureToInsecure || !isSecureToInsecure);

            bool ChangeVerbToGetOn(int statusCode, HttpMethod verb)
            {
                switch (statusCode)
                {
                    // 301 and 302 are a bit ambiguous. The spec says to preserve the verb
                    // but most browsers rewrite it to GET. HttpClient stack changes it if
                    // only it's a POST, presumably since that's a browser-friendly verb.
                    // Seems odd, but sticking with that is probably the safest bet.
                    // https://github.com/dotnet/runtime/blob/master/src/libraries/System.Net.Http/src/System/Net/Http/SocketsHttpHandler/RedirectHandler.cs#L140
                    case 301:
                    case 302:
                        return verb == HttpMethod.Post;
                    case 303:
                        return true;
                    default: // 307 & 308 mainly
                        return false;
                }
            }

            redir.ChangeVerbToGet =
                redir.Follow &&
                ChangeVerbToGetOn(call.Response.StatusCode, call.Request.Verb);

            return redir;
        }

        private void CheckForCircularRedirects(EasyCall call, HashSet<string> visited = null)
        {
            if (call == null) return;
            visited ??= new HashSet<string>();
            if (visited.Contains(call.Request.Url))
                throw new EasyHttpException(call, "Circular redirects detected.", null);
            visited.Add(call.Request.Url);
            CheckForCircularRedirects(call.RedirectedFrom, visited);
        }

        internal static async Task<IEasyResponse> HandleExceptionAsync(EasyCall call, Exception ex, CancellationToken token)
        {
            call.Exception = ex;
            await RaiseEventAsync(call.Request.Settings.OnError, call.Request.Settings.OnErrorAsync, call).ConfigureAwait(false);

            if (call.ExceptionHandled)
                return call.Response;

            if (ex is OperationCanceledException && !token.IsCancellationRequested)
                throw new CaesarHttpTimeoutException(call, ex);

            if (ex is EasyHttpException)
                throw ex;

            throw new EasyHttpException(call, ex);
        }

        private static Task RaiseEventAsync(Action<EasyCall> syncHandler, Func<EasyCall, Task> asyncHandler, EasyCall call)
        {
            syncHandler?.Invoke(call);
            if (asyncHandler != null)
                return asyncHandler(call);
            return Task.FromResult(0);
        }
    }
}