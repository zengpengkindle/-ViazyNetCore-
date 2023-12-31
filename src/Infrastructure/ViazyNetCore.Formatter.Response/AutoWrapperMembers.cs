﻿using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ViazyNetCore.Formatter.Response.Extensions;
using ViazyNetCore.Formatter.Response.Helpers;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace ViazyNetCore.Formatter.Response
{
    internal class AutoWrapperMembers
    {

        private readonly ResponseWrapperOptions _options;
        private readonly ILogger<AutoWrapperMiddleware> _logger;
        private readonly JsonSerializerSettings _jsonSettings;

        public AutoWrapperMembers(ResponseWrapperOptions options, ILogger<AutoWrapperMiddleware> logger, JsonSerializerSettings jsonSettings)
        {
            _options = options;
            _logger = logger;
            _jsonSettings = jsonSettings;
        }

        public async Task<string?> GetRequestBodyAsync(HttpRequest request)
        {
            var httpMethodsWithRequestBody = new[] { "POST", "PUT", "PATCH" };
            var hasRequestBody = httpMethodsWithRequestBody.Any(x => x.Equals(request.Method.ToUpper()));
            string? requestBody = default;
            if (hasRequestBody)
            {
                request.EnableBuffering();
                if (request.HasFormContentType == true)
                {
                    if (request.Form.Files.Count > 0)
                        return null;
                }

                using var memoryStream = new MemoryStream();
                await request.Body.CopyToAsync(memoryStream);
                requestBody = Encoding.UTF8.GetString(memoryStream.ToArray());
                request.Body.Seek(0, SeekOrigin.Begin);
                request.Body.Position = 0;
            }
            return requestBody;
        }

        public async Task<string> ReadResponseBodyStreamAsync(Stream bodyStream)
        {
            bodyStream.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(bodyStream).ReadToEndAsync();
            bodyStream.Seek(0, SeekOrigin.Begin);

            var (IsEncoded, ParsedText) = responseBody.VerifyBodyContent();

            return IsEncoded ? ParsedText : responseBody;
        }

        public async Task RevertResponseBodyStreamAsync(Stream bodyStream, Stream orginalBodyStream)
        {
            bodyStream.Seek(0, SeekOrigin.Begin);
            await bodyStream.CopyToAsync(orginalBodyStream);
        }

        public async Task HandleExceptionAsync(HttpContext context, System.Exception exception)
        {
            int httpStatusCode;
            string? exceptionMessage = default;
            string? jsonString;
            if (exception is ApiException apiException)
            {
                if (apiException.StatusCode == Status404NotFound)
                {
                    httpStatusCode = Status404NotFound;
                    jsonString = this.ConvertToNotFoundExceptionString();
                }
                else if (apiException.StatusCode == Status403Forbidden)
                {
                    httpStatusCode = Status403Forbidden;
                    jsonString = this.ConvertToForbiddenExceptionString();
                }
                else if (apiException.StatusCode == Status406NotAcceptable)
                {
                    httpStatusCode = Status406NotAcceptable;
                    jsonString = this.ConvertToNotAcceptableExceptionString();
                }
                else if (apiException.StatusCode == Status400BadRequest)
                {
                    httpStatusCode = Status400BadRequest;
                    jsonString = this.ConvertToFailJSONString(context, exception.Message, context.Request.Method);
                }
                else
                {
                    httpStatusCode = Status200OK;
                    jsonString = this.ConvertToFailJSONString(context, exception.Message, context.Request.Method, apiException.StatusCode);
                }
            }
            else if (exception is UnauthorizedAccessException)
            {
                httpStatusCode = Status401Unauthorized;
                jsonString = this.ConvertToUnauthorizedAccessExceptionString();
            }
            else if (exception is SingleSignOnException)
            {
                httpStatusCode = Status409Conflict;
                jsonString = this.ConvertToSingleSignOnExceptionString();
            }
            else
            {
                httpStatusCode = Status500InternalServerError;
                string? stackTrace = null;
                if (_options.IsDebug)
                {
                    exceptionMessage = $"{exceptionMessage} {exception.GetBaseException().Message}";
                    stackTrace = exception.StackTrace;
                }
                else
                {
                    exceptionMessage = ResponseMessage.Unhandled;
                }

                jsonString = ConvertToExceptionJSONString(exceptionMessage, stackTrace);

                this._logger.LogError(exception, "未知异常");
            }

            await WriteFormattedResponseToHttpContextAsync(context, httpStatusCode, jsonString);
        }

        public async Task HandleUnsuccessfulRequestAsync(HttpContext context, object body, int httpStatusCode)
        {
            var (IsEncoded, ParsedText) = body.ToString()!.VerifyBodyContent();

            var bodyText = IsEncoded ? ParsedText : body.ToString();

            var message = GetUnsucessfulErrorMessage(httpStatusCode);

            var apiResponse = new ApiResponse()
            {
                StatusCode = httpStatusCode,
                Message = !string.IsNullOrEmpty(body.ToString()) ? bodyText : message,
                Version = GetApiVersion()
            };

            var jsonString = JsonConvert.SerializeObject(apiResponse, _jsonSettings);

            await WriteFormattedResponseToHttpContextAsync(context, httpStatusCode, jsonString);
        }

        public async Task HandleSuccessfulRequestAsync(HttpContext context, object body)
        {
            var bodyText = !body.ToString().IsValidJson() ? this.ConvertToJSONString(body) : body.ToString();

            dynamic? bodyContent = JsonConvert.DeserializeObject<dynamic>(bodyText);

            Type? type = bodyContent?.GetType();

            string jsonString;
            if (type == typeof(JObject))
            {
                jsonString = ConvertToSuccessJSONString(context, bodyContent, context.Request.Method);
            }
            else
            {
                var validated = ValidateSingleValueType(bodyContent);
                object result = validated.Item1 ? validated.Item2 : bodyContent;
                jsonString = ConvertToSuccessJSONString(context, result, context.Request.Method);
            }

            await WriteFormattedResponseToHttpContextAsync(context, Status200OK, jsonString);
        }

        public async Task HandleNotApiRequestAsync(HttpContext context)
        {
            string configErrorText = ResponseMessage.NotApiOnly;
            context.Response.ContentLength = configErrorText != null ? Encoding.UTF8.GetByteCount(configErrorText) : 0;
            await context.Response.WriteAsync(configErrorText);
        }

        public bool IsSwagger(HttpContext context)
        {
            return context.Request.Path.StartsWithSegments(new PathString("/swagger"));
        }

        public bool IsApi(HttpContext context)
        {
            if (_options.IsApiOnly && !context.Request.Path.Value.Contains(".js") && !context.Request.Path.Value.Contains(".css"))
                return true;

            return context.Request.Path.StartsWithSegments(new PathString(_options.WrapWhenApiPathStartsWith));
        }

        public bool IsRequestSuccessful(int statusCode)
        {
            return (statusCode >= 200 && statusCode < 400);
        }

        #region Private Members

        private async Task WriteFormattedResponseToHttpContextAsync(HttpContext context, int httpStatusCode, string jsonString)
        {
            context.Response.StatusCode = httpStatusCode;
            context.Response.ContentType = TypeIdentifier.JSONHttpContentMediaType;
            context.Response.ContentLength = jsonString != null ? Encoding.UTF8.GetByteCount(jsonString) : 0;
            await context.Response.WriteAsync(jsonString);
        }

        private string ConvertToSuccessJSONString(HttpContext context, object content, string httpMethod)
        {
            var apiResponse = new ApiResponse()
            {
                StatusCode = Status200OK,
                Message = $"{httpMethod} {ResponseMessage.Success}",
                Version = GetApiVersion(),
                Data = new WrapperData()
                {
                    Success = true,
                    Result = _options.EnableCipher ? content.EncryptCBC(context) : content,
                }
            };

            return JsonConvert.SerializeObject(apiResponse, _jsonSettings);
        }

        private string ConvertToFailJSONString(HttpContext context, string message, string httpMethod, int errCodes = 10000)
        {
            var apiResponse = new ApiResponse()
            {
                StatusCode = Status200OK,
                Message = $"{httpMethod} {ResponseMessage.Success}",
                Version = GetApiVersion(),
                Data = new WrapperData()
                {
                    ErrorCode = errCodes,
                    Success = false,
                    Message = message
                }
            };

            return JsonConvert.SerializeObject(apiResponse, _jsonSettings);
        }

        private string ConvertToUnauthorizedAccessExceptionString()
        {
            var apiResponse = new ApiResponse()
            {
                StatusCode = Status401Unauthorized,
                Message = ResponseMessage.UnAuthorized,
                Version = GetApiVersion()
            };

            return JsonConvert.SerializeObject(apiResponse, _jsonSettings);
        }

        private string ConvertToSingleSignOnExceptionString()
        {
            var apiResponse = new ApiResponse()
            {
                StatusCode = Status409Conflict,
                Message = ResponseMessage.SingleSignOn,
                Version = GetApiVersion()
            };

            return JsonConvert.SerializeObject(apiResponse, _jsonSettings);
        }

        private string ConvertToPaymentRequiredExceptionString()
        {
            var apiResponse = new ApiResponse()
            {
                StatusCode = Status402PaymentRequired,
                Message = ResponseMessage.PaymentRequired,
                Version = GetApiVersion()
            };

            return JsonConvert.SerializeObject(apiResponse, _jsonSettings);
        }

        private string ConvertToForbiddenExceptionString()
        {
            var apiResponse = new ApiResponse()
            {
                StatusCode = Status403Forbidden,
                Message = ResponseMessage.UnAuthorized,
                Version = GetApiVersion()
            };

            return JsonConvert.SerializeObject(apiResponse, _jsonSettings);
        }

        private string ConvertToNotAcceptableExceptionString()
        {
            var apiResponse = new ApiResponse()
            {
                StatusCode = Status406NotAcceptable,
                Message = ResponseMessage.NotAcceptable,
                Version = GetApiVersion()
            };

            return JsonConvert.SerializeObject(apiResponse, _jsonSettings);
        }

        private string ConvertToNotFoundExceptionString()
        {
            var apiResponse = new ApiResponse()
            {
                StatusCode = Status404NotFound,
                Message = ResponseMessage.NotFound,
                Version = GetApiVersion()
            };

            return JsonConvert.SerializeObject(apiResponse, _jsonSettings);
        }

        private string ConvertToExceptionJSONString(string exceptionMessage, string? stackTrace)
        {
            var apiResponse = new ApiResponse()
            {
                StatusCode = Status500InternalServerError,
                Message = exceptionMessage,
                StackTrace = stackTrace,
                Version = GetApiVersion()
            };

            return JsonConvert.SerializeObject(apiResponse, _jsonSettings);
        }

        private string ConvertToJSONString(object rawJSON) => JsonConvert.SerializeObject(rawJSON, _jsonSettings);

        private string GetUnsucessfulErrorMessage(int statusCode) =>
           statusCode switch
           {
               Status204NoContent => ResponseMessage.NotContent,
               Status400BadRequest => ResponseMessage.BadRequest,
               Status401Unauthorized => ResponseMessage.UnAuthorized,
               Status404NotFound => ResponseMessage.NotFound,
               Status405MethodNotAllowed => ResponseMessage.MethodNotAllowed,
               Status415UnsupportedMediaType => ResponseMessage.MediaTypeNotSupported,
               _ => ResponseMessage.Unknown
           };

        private string GetApiVersion() => !_options.ShowApiVersion ? null : _options.ApiVersion;

        private (bool, object) ValidateSingleValueType(object value)
        {
            var result = value.ToString();
            if (result.IsWholeNumber()) { return (true, result.ToInt64()); }
            if (result.IsDecimalNumber()) { return (true, result.ToDecimal()); }
            if (result.IsBoolean()) { return (true, result.ToBoolean()); }

            return (false, value);
        }

        #endregion
    }

}
