namespace ViazyNetCore.AspNetCore.Mvc.Wrap
{
    internal class ResponseMessage
    {
        internal const string Success = "Request successful.";
        internal const string NotFound = "Request not found. The specified uri does not exist.";
        internal const string BadRequest = "Request invalid.";
        internal const string MethodNotAllowed = "Request responded with 'Method Not Allowed'.";
        internal const string NotContent = "Request no content. The specified uri does not contain any content.";
        internal const string Exception = "Request responded with exceptions.";
        internal const string UnAuthorized = "Request denied. Unauthorized access.";
        internal const string NotAcceptable = "Request denied. NotAcceptable access.";
        internal const string SingleSignOn = "Request denied. Conflicted access.";
        internal const string PaymentRequired = "Request denied. Required payment.";
        internal const string ValidationError = "Request responded with one or more validation errors.";
        internal const string Unknown = "Request cannot be processed. Please contact support.";
        internal const string Unhandled = "Unhandled Exception occurred. Unable to process the request.";
        internal const string MediaTypeNotSupported = "Unsupported Media Type.";
        internal const string NotApiOnly = @"HTML detected in the response body.
If you are combining API Controllers within your front-end projects like Angular, MVC, React, Blazor and other SPA frameworks that supports .NET Core, then set the AutoWrapperOptions IsApiOnly property to false. 
If you are using pure API and want to output HTML as part of your JSON object, then set BypassHTMLValidation property to true.";
    }
}
