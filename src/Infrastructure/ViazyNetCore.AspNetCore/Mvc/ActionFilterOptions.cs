using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.AspNetCore.Mvc
{
    public class ActionFilterOptions
    {
        /// <summary>
        /// Sets the Api version to be shown in the response. You must set the ShowApiVersion to true to see this value in the response.
        /// </summary>
        public string ApiVersion { get; set; } = "1.0.0.0";

        public string HttpHeaderLogIdName { get; set; } = "X-log-id";
        /// <summary>
        /// Shows the stack trace information in the responseException details.
        /// </summary>
        public bool IsDebug { get; set; } = false;
        /// <summary>
        /// Shows the Api Version attribute in the response.
        /// </summary>
        public bool ShowApiVersion { get; set; } = false;

        /// <summary>
        /// Use to indicate if the wrapper is used for API project only. 
        /// Set this to false when you want to use the wrapper within an Angular, MVC, React or Blazor projects.
        /// </summary>
        public bool IsApiOnly { get; set; } = true;

        /// <summary>
        /// Tells the wrapper to ignore validation for string that contains HTML
        /// </summary>
        public bool BypassHTMLValidation { get; set; } = false;

        /// <summary>
        /// Set the Api path segment to validate. The default value is '/api'. Only works if IsApiOnly is set to false.
        /// </summary>
        public string WrapWhenApiPathStartsWith { get; set; } = "/api";

        /// <summary>
        /// Tells the wrapper whether to enable request and response logging. Default is true.
        /// </summary>
        public bool EnableResponseLogging { get; set; } = false;

        /// <summary>
        /// Tells the wrapper whether to enable exception logging. Default is true.
        /// </summary>
        public bool EnableExceptionLogging { get; set; } = false;

        public bool EnableRequestDataOnException { get; set; } = true;

        public bool EnableRequestDataLogging { get; set; } = true;
    }
}
