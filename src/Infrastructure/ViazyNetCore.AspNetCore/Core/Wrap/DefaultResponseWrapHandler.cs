using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ViazyNetCore.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace ViazyNetCore.AspNetCore.Core.Wrap
{
    [ExposeServices(typeof(IResponseWrapHandler))]
    [Injection(Lifetime = ServiceLifetime.Transient, TryRegister = true, ReplaceServices = true)]
    public class DefaultResponseWrapHandler : IResponseWrapHandler
    {
        private readonly ISerializer<string> _serializer;
        private readonly ActionFilterOptions _options;
        internal const string JSONHttpContentMediaType = "application/json;charset=utf-8";

        public DefaultResponseWrapHandler(ISerializer<string> serializer, IOptions<ActionFilterOptions> options)
        {
            this._serializer = serializer;
            this._options = options.Value;
        }

        public async Task HandleExceptionAsync(HttpContext context, Exception exception, bool logStackTrace)
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
                    jsonString = this.ConvertToBadRequestExceptionString(exception.Message);
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
                if (logStackTrace)
                {
                    exceptionMessage = $"{exceptionMessage} {exception.GetBaseException().Message}";
                    stackTrace = exception.StackTrace;
                }
                else
                {
                    exceptionMessage = ResponseMessage.Unhandled;
                }

                jsonString = ConvertToExceptionJSONString(exceptionMessage, stackTrace);
            }
            await WriteFormattedResponseToHttpContextAsync(context, httpStatusCode, jsonString);
        }

        private async Task WriteFormattedResponseToHttpContextAsync(HttpContext context, int httpStatusCode, string jsonString)
        {
            context.Response.StatusCode = httpStatusCode;
            context.Response.ContentType = JSONHttpContentMediaType;
            context.Response.ContentLength = jsonString != null ? Encoding.UTF8.GetByteCount(jsonString) : 0;
            await context.Response.WriteAsync(jsonString);
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
                var validated = TryCheckSingleValueType(bodyContent, out object result);
                jsonString = ConvertToSuccessJSONString(context, result, context.Request.Method);
            }

            await WriteFormattedResponseToHttpContextAsync(context, Status200OK, jsonString);
        }

        #region Private Members

        public virtual string ConvertToSuccessJSONString(HttpContext context, object content, string httpMethod)
        {
            var apiResponse = new ApiResponse()
            {
                StatusCode = Status200OK,
                Message = $"{httpMethod} {ResponseMessage.Success}",
                Version = GetApiVersion(),
                Data = new WrapperData()
                {
                    Success = true,
                    Result = content,
                }
            };

            return _serializer.Serialize(apiResponse);
        }

        public virtual string ConvertToFailJSONString(HttpContext context, string message, string httpMethod, int errCodes = 10000)
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

            return _serializer.Serialize(apiResponse);
        }

        public virtual string ConvertToBadRequestExceptionString(object data)
        {
            var apiResponse = new ApiResponse()
            {
                StatusCode = Status400BadRequest,
                Message = ResponseMessage.BadRequest,
                Version = GetApiVersion(),
                Data = new WrapperData()
                {
                    Success = false,
                    Result = data,
                    Message = ResponseMessage.BadRequest
                }
            };
            return _serializer.Serialize(apiResponse);
        }

        public virtual string ConvertToUnauthorizedAccessExceptionString()
        {
            var apiResponse = new ApiResponse()
            {
                StatusCode = Status401Unauthorized,
                Message = ResponseMessage.UnAuthorized,
                Version = GetApiVersion()
            };
            return _serializer.Serialize(apiResponse);
        }

        public virtual string ConvertToSingleSignOnExceptionString()
        {
            var apiResponse = new ApiResponse()
            {
                StatusCode = Status409Conflict,
                Message = ResponseMessage.SingleSignOn,
                Version = GetApiVersion()
            };
            return _serializer.Serialize(apiResponse);
        }

        public virtual string ConvertToPaymentRequiredExceptionString()
        {
            var apiResponse = new ApiResponse()
            {
                StatusCode = Status402PaymentRequired,
                Message = ResponseMessage.PaymentRequired,
                Version = GetApiVersion()
            };
            return _serializer.Serialize(apiResponse);
        }

        public virtual string ConvertToForbiddenExceptionString()
        {
            var apiResponse = new ApiResponse()
            {
                StatusCode = Status403Forbidden,
                Message = ResponseMessage.UnAuthorized,
                Version = GetApiVersion()
            };

            return _serializer.Serialize(apiResponse);
        }

        public virtual string ConvertToNotAcceptableExceptionString()
        {
            var apiResponse = new ApiResponse()
            {
                StatusCode = Status406NotAcceptable,
                Message = ResponseMessage.NotAcceptable,
                Version = GetApiVersion()
            };

            return _serializer.Serialize(apiResponse);
        }

        public virtual string ConvertToNotFoundExceptionString()
        {
            var apiResponse = new ApiResponse()
            {
                StatusCode = Status404NotFound,
                Message = ResponseMessage.NotFound,
                Version = GetApiVersion()
            };

            return _serializer.Serialize(apiResponse);
        }

        public virtual string ConvertToExceptionJSONString(string exceptionMessage, string? stackTrace)
        {
            var apiResponse = new ApiResponse()
            {
                StatusCode = Status500InternalServerError,
                Message = exceptionMessage,
                StackTrace = stackTrace,
                Version = GetApiVersion()
            };

            return _serializer.Serialize(apiResponse);
        }

        private string ConvertToJSONString(object rawJSON) => _serializer.Serialize(rawJSON);

        public virtual string GetUnsucessfulErrorMessage(int statusCode) =>
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

        private string GetApiVersion()
        {
            return "1.0";
        }

        private bool TryCheckSingleValueType(object value, out object outValue)
        {
            outValue = null;
            var result = value.ToString();
            if (result.IsWholeNumber())
            {
                outValue = result.ToInt64();
                return true;
            }
            if (result.IsDecimalNumber())
            {
                outValue = result.ToDecimal();
                return true;
            }
            if (result.IsBoolean())
            {
                outValue = result.ToBoolean();
                return true;
            }

            return false;
        }
        #endregion
    }
}
