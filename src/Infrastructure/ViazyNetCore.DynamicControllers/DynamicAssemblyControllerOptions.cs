using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.DynamicControllers
{
    public class DynamicAssemblyControllerOptions
    {
        /// <summary>
        /// Routing prefix for all APIs
        /// <para></para>
        /// Default value is null.
        /// </summary>
        public string? ApiPrefix { get; }

        /// <summary>
        /// API HTTP Verb.
        /// <para></para>
        /// Default value is null.
        /// </summary>
        public string? HttpVerb { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiPrefix">Routing prefix for all APIs</param>
        /// <param name="httpVerb">API HTTP Verb.</param>
        public DynamicAssemblyControllerOptions(string? apiPrefix = null, string? httpVerb = null)
        {
            ApiPrefix = apiPrefix;
            HttpVerb = httpVerb;
        }
    }
}
